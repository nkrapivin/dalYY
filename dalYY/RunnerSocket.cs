using System;
using System.ComponentModel;
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
		public BinaryReader HeaderReader { get; set; }
		private GameLayout GLData { get; set; }
		public YYDebug YYDbg { get; set; }

		public bool IsSenderConnected()
		{
			if (Sender != null)
			{
				return Sender.Connected && State == RunnerSocketState.Connected;
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

		public bool SendBuffer(byte[] buf)
        {
			if (State != RunnerSocketState.Connected)
				return false;
			if (buf.Length < 1)
				return false;

			int tries = 4;
			while (tries > 0)
            {
				try
                {
					Sender.Send(buf, 0, buf.Length, SocketFlags.None);
					tries = 0;
                }
				catch
                {
					tries--;
                }
            }

			if (tries < 0) return false;

			return true;
        }

		public bool SendCommand(params int[] Command)
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
			if (Command.Length > 1) pk.Arg1 = (uint)Command[1];
			if (Command.Length > 2) pk.Arg2 = (uint)Command[2];
			if (Command.Length > 3) pk.Arg3 = (uint)Command[3];
			if (Command.Length > 4) pk.Arg4 = (uint)Command[4];
			if (Command.Length > 5) pk.Arg5 = (uint)Command[5];
			if (Command.Length > 6) pk.Arg6 = (uint)Command[6];
			if (Command.Length > 7) pk.Arg7 = (uint)Command[7];

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
				HeaderReader.BaseStream.Seek(0, SeekOrigin.Begin);
				uint dataHeader = HeaderReader.ReadUInt32();
				int dataSize = HeaderReader.ReadInt32() - 8;
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
							dataRec = Sender.Receive(HeaderData, num, dataSize - num, SocketFlags.None);
							num += dataRec;
							if (dataRec > 0) continue;
							break;
						}

						if (num > 0) return num;

						State = RunnerSocketState.Error;
						return 0;
					}
					else
                    {
						Console.WriteLine("Wrong packet header!");
                    }
                }

            }
			catch { return 0; }

			return 0;
        }

		public byte[] GetCommandResult()
        {
			int len = Recieve();
			if (len > 0)
            {
				byte[] _out = new byte[len];
				for (int i = 0; i < len; i++) _out[i] = HeaderData[i];
				return _out;
            }

			return null;
        }

		public RunnerConnErr Connect(string _host, int _port, BackgroundWorker worker = null)
		{
			Sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

			try
            {
				worker?.ReportProgress(0, "Setting up socket...");
				CommData = new byte[1048576];
				Sender.ReceiveTimeout = -1;
				Sender.SendTimeout = 1000;
				Sender.Connect(_host, _port);

				worker?.ReportProgress(0, "Connecting...");

				int len = Sender.Receive(CommData);
				string hello = Encoding.ASCII.GetString(CommData, 0, len - 1);

				worker?.ReportProgress(0, "Recieved hello!");
				if (hello != "GM:Studio-Connect")
                {
					Sender.Disconnect(false);
					Sender.Shutdown(SocketShutdown.Both);
					Sender.Close();
					State = RunnerSocketState.Error;
					return RunnerConnErr.LoginError;
				}

				worker?.ReportProgress(0, "Logging in...");
				var loginPacket = new RunnerLoginPacket();
				loginPacket.SIG1 = RunnerLoginPacket.NETWORK_SIG1;
				loginPacket.SIG2 = RunnerLoginPacket.NETWORK_SIG2;
				loginPacket.Size = Marshal.SizeOf(loginPacket);
				loginPacket.GameID = RunnerLoginPacket.NETWORK_GAMEID;

				byte[] data = getBytes(loginPacket);
				Sender.Send(data, 0, loginPacket.Size, SocketFlags.None);

				worker?.ReportProgress(0, "Sent login packet");

				len = Sender.Receive(CommData);
				State = RunnerSocketState.Connected;

				worker?.ReportProgress(0, "Sending GetGameData packet.");
				if (!SendCommand((int)RunnerCommand.GetGameData))
                {
					throw new Exception("Could not send GetGameData command!");
                }

				HeaderData = new byte[1048576];
				HeaderStream = new MemoryStream(HeaderData);
				HeaderReader = new BinaryReader(HeaderStream);

				len = Recieve();
				if (len > 0)
				{
					worker?.ReportProgress(0, "Loading game layout...");
					byte[] glDat = new byte[len];
					for (var i = 0; i < len; i++) glDat[i] = HeaderData[i];
					GLData = new GameLayout();
					GLData.GameYYDebug = YYDbg;
					if (!GLData.Load(glDat))
					{
						worker?.ReportProgress(0, "Failed to parse game layout");
						State = RunnerSocketState.Error;
						if (Sender.Connected) Sender.Disconnect(false);
						return RunnerConnErr.InvalidGameStructure;
					}
					worker?.ReportProgress(0, "Game layout parsed ok.");
				}
			}
			catch (ArgumentNullException e)
			{
				worker?.ReportProgress(0, $"Argument is null! {e.Message}");
				State = RunnerSocketState.Error;
				if (Sender.Connected) Sender.Disconnect(false);
				return RunnerConnErr.NullArgument;
			}
			catch (SocketException e)
            {
				worker?.ReportProgress(0, $"Socket exception! {e.Message}");
				State = RunnerSocketState.Error;
				if (Sender.Connected) Sender.Disconnect(false);
				return RunnerConnErr.SocketError;
			}
			catch (Exception e)
            {
				worker?.ReportProgress(0, $"General exception! {e.Message}");
				State = RunnerSocketState.Error;
				if (Sender.Connected) Sender.Disconnect(false);
				return RunnerConnErr.GeneralException;
			}

			worker?.ReportProgress(0, "Done!");
			for (int i = 0; i < HeaderData.Length; i++) HeaderData[i] = 0;
			HeaderStream.Seek(0, SeekOrigin.Begin);
			return RunnerConnErr.None;
		}

		public void Quit()
        {
			if (State == RunnerSocketState.None) return;

			if (Sender != null)
            {
				State = RunnerSocketState.None;
				Sender.Shutdown(SocketShutdown.Both);
				Sender.Close();
            }
        }

		public void Disconnect()
        {
			if (State != RunnerSocketState.None)
            {
				Sender.Disconnect(false);
				State = RunnerSocketState.None;
            }
        }
	}

}
