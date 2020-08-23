using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

namespace dalYY
{
    public enum RunnerConnErr
	{
		None,
		NullArgument,
		SocketError,
		GeneralException,
		LoginError,
		InvalidGameStructure
	}

	public enum RunnerSocketState
	{
		None,
		Connected,
		Error
	}

	public enum RunnerCommand : int
	{
		None,
		Ping,
		GetGameData,
		StopTarget,
		StartTarget,
		SingleStep,
		SingleStepLine,
		GetInstanceData,
		GetJSInstanceData,
		StartBreakpoint,
		GetWatches,
		GetUpdate,
		GetArrays,
		GetStructures,
		RestartTarget,
		GetSelectedInstance,
		GetBuffers,
		PokeStructure,
		GetTextures,
		BatchCommand,
		CommandCount,
		NetworkCommandSizer = int.MaxValue
	}

	public struct RunnerNetworkPacket
	{
		public const uint PK_HEADER = 3188834526;
		public uint Header;
		public int Size;
		public int PacketSize;
		public RunnerCommand Command;

		public uint Arg1;
		public uint Arg2;
		public uint Arg3;
		public uint Arg4;
		public uint Arg5;
		public uint Arg6;
		public uint Arg7;
	}

	public struct RunnerLoginPacket
	{
		public const uint NETWORK_SIG1 = 3405691582u;
		public const uint NETWORK_SIG2 = 3735924747u;
		public const  int NETWORK_GAMEID = 296824970;
		public uint SIG1;
		public uint SIG2;
		public int Size;
		public int GameID;
	}

	public class RunnerSocket
	{
		public RunnerSocketState State { get; private set; }
		private Socket Sender { get; set; }
		private byte[] CommData { get; set; }
		private byte[] HeaderData { get; set; }
		private MemoryStream HeaderStream { get; set; }
		private BinaryReader HeaderReader { get; set; }
		private GameLayout GLData { get; set; }
		public YYDebug YYDbg { get; set; }

		public bool IsSenderConnected()
		{
			if (Sender != null)
			{
				return Sender.Connected;
			}
			return false;
		}

		private byte[] getBytes(RunnerNetworkPacket str)
		{
			int size = Marshal.SizeOf(str);
			byte[] arr = new byte[size];

			IntPtr ptr = Marshal.AllocHGlobal(size);
			Marshal.StructureToPtr(str, ptr, true);
			Marshal.Copy(ptr, arr, 0, size);
			Marshal.FreeHGlobal(ptr);
			return arr;
		}

		private byte[] getBytes(RunnerLoginPacket str)
		{
			int size = Marshal.SizeOf(str);
			byte[] arr = new byte[size];

			IntPtr ptr = Marshal.AllocHGlobal(size);
			Marshal.StructureToPtr(str, ptr, true);
			Marshal.Copy(ptr, arr, 0, size);
			Marshal.FreeHGlobal(ptr);
			return arr;
		}

		private bool SendCommand(params int[] Command)
        {
			if (State != RunnerSocketState.Connected)
				return false;
			if (Command.Length == 0)
				return false;


			RunnerNetworkPacket pk = new RunnerNetworkPacket();
			pk.Header = RunnerNetworkPacket.PK_HEADER;
			pk.Size = Marshal.SizeOf(pk);
			pk.PacketSize = pk.Size;
			pk.Command = (RunnerCommand)Command[0];
			byte[] data = getBytes(pk);
			int tries = 4;
			while (tries > 0)
            {
				try
                {
					Sender.Send(data, 0, pk.Size, SocketFlags.None);
					tries = 0;
                }
				catch
                {
					tries--;
                }
            }

			if (tries < 0)
            {
				return false;
            }

			return true;
        }

		public int Recieve(int _timeout = 5000)
        {
			if (State != RunnerSocketState.Connected)
				return 0;

			Sender.ReceiveTimeout = _timeout;
			try
            {
				int dataRec = Sender.Receive(HeaderData, 0, 8, SocketFlags.None);
				uint dataHeader = HeaderReader.ReadUInt32();
				uint dataSize = HeaderReader.ReadUInt32() - 8;
				if (dataRec != 8)
                {
					Console.WriteLine("Failed to read packet header!");
                }
				else
                {
					int num = 0;
					if (dataHeader == RunnerNetworkPacket.PK_HEADER)
                    {
						if (dataSize > HeaderData.Length)
                        {
							HeaderStream.SetLength(dataSize);
                        }

						while (num < dataSize)
                        {
							dataRec = Sender.Receive(HeaderData, num, (int)(dataSize - num), SocketFlags.None);
							num += dataRec;
							if (dataRec > 0) continue;
							break;
						}

						if (num > 0) return num;

						State = RunnerSocketState.Error;
						return 0;
					}
					
                }

            }
			catch { return 0; }

			return 0;
        }

		public RunnerConnErr Connect(string _host, int _port)
		{
			Sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

			try
            {
				CommData = new byte[1048576];
				Sender.ReceiveTimeout = -1;
				Sender.SendTimeout = 1000;
				Sender.Connect(_host, _port);
				int len = Sender.Receive(CommData);
				string hello = Encoding.ASCII.GetString(CommData, 0, len - 1);
				if (hello != "GM:Studio-Connect")
                {
					Sender.Disconnect(false);
					Sender.Shutdown(SocketShutdown.Both);
					Sender.Close();
					State = RunnerSocketState.Error;
					return RunnerConnErr.LoginError;
				}

				var loginPacket = new RunnerLoginPacket();
				loginPacket.SIG1 = RunnerLoginPacket.NETWORK_SIG1;
				loginPacket.SIG2 = RunnerLoginPacket.NETWORK_SIG2;
				loginPacket.Size = Marshal.SizeOf(loginPacket);
				loginPacket.GameID = RunnerLoginPacket.NETWORK_GAMEID;

				byte[] data = getBytes(loginPacket);
				Sender.Send(data, 0, loginPacket.Size, SocketFlags.None);

				len = Sender.Receive(CommData);
				State = RunnerSocketState.Connected;
				if (!SendCommand((int)RunnerCommand.GetGameData))
                {
					throw new Exception("Could not send GetGameData command!");
                }

				HeaderData = new byte[268435456];
				HeaderStream = new MemoryStream(HeaderData);
				HeaderReader = new BinaryReader(HeaderStream);

				len = Recieve();
				if (len > 0)
				{
					byte[] glDat = new byte[len];
					for (var i = 0; i < len; i++) glDat[i] = HeaderData[i];
					File.WriteAllBytes("gameLayout.iff", glDat);
					GLData = new GameLayout();
					GLData.GameYYDebug = YYDbg;
					if (!GLData.Load(glDat))
					{
						Console.WriteLine("Failed to parse GameLayout structure. Is your internet weird?");
						Console.WriteLine("PS: meow!");
					}
					else
						Console.WriteLine("GameLayout Loaded!");
				}
			}
			catch { }

			return RunnerConnErr.None;
		}
	}

}
