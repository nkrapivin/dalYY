using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace dalYY
{
    public class DebuggerManager
    {
        private RunnerSocket Socket { get; set; }
        private uint SendID { get; set; }
        private long LastPingMs { get; set; }

        public bool IsRecording { get; set; }
        public bool MaxRoomSpeed { get; set; }

        private bool IsBatching { get; set; }
        private long BatchSizeOff { get; set; }
        private long BatchCmdOff { get; set; }
        private int BatchCmdCount { get; set; }
        private MemoryStream BatchStream { get; set; }
        private BinaryWriter BatchWriter { get; set; }


        public int FPS { get; set; }
        public uint UsedMem { get; set; }
        public ulong FreeMem { get; set; }
        public string DebugOutput { get; set; }

        public DebuggerManager(RunnerSocket sck)
        {
            Socket = sck;
            DebugOutput = string.Empty;
        }

        public bool ReadResults()
        {
            var reader = Socket.HeaderReader;
            var maincmd = (RunnerCommand)reader.ReadInt32();
            switch (maincmd)
            {
                case RunnerCommand.BatchCommand:
                    {
                        int num = reader.ReadInt32();
                        for (int i = 0; i < num; i++)
                        {
                            var cmd = (RunnerCommand)reader.ReadInt32();
                            ReadResult(cmd, reader);
                        }
                        break;
                    }
                default:
                    {
                        ReadResult(maincmd, reader);
                        break;
                    }
            }

            return true;
        }

        public void ReadResult(RunnerCommand cmd, BinaryReader reader)
        {
            switch (cmd)
            {
                case RunnerCommand.Ping:
                    {
                        ReadPingResults(reader);
                        break;
                    }
                case RunnerCommand.GetUpdate:
                    {
                        ReadUpdateResults(reader);
                        break;
                    }
                default: break;
            }
        }

        public void ReadUpdateResults(BinaryReader reader)
        {
            uint flag = reader.ReadUInt32();
            //Console.WriteLine(flag);
        }

        public void ReadPingResults(BinaryReader reader)
        {
            uint pingRunning = reader.ReadUInt32();
            uint sendID = reader.ReadUInt32();

            if (SendID != sendID)
            {
                Console.WriteLine("SendID mismatch! One of the sides is ROLLING AROUND AT THE TOP SPEED OF SOUND");
            }

            FPS = reader.ReadInt32();
            UsedMem = reader.ReadUInt32();
            FreeMem = reader.ReadUInt64();
            DebugOutput = ReadString(reader);
        }

        public string ReadString(BinaryReader reader)
        {
            int num = reader.ReadInt32();
            string text = "";
            for (num--; num > 0; num--)
            {
                text += (char)(reader.ReadByte() & 0xFF);
            }
            reader.ReadByte();

            return text;
        }

        public bool Update(bool run)
        {
            if (!run) return false;

            BeginBatch();
            AddCommand(RunnerCommand.GetUpdate, 1);
            EndBatch();
            SendBatch();
            bool ret = Recieve();

            if (ret) ReadResults();

            return ret;
        }

        public bool IsRunning()
        {
            long time = DateTime.Now.Ticks / 10000;
            if (time < LastPingMs + 500) return true;

            LastPingMs = time;
            SendID++;

            int run_params = 0;
            if (IsRecording) run_params |= 1;
            if (MaxRoomSpeed) run_params |= 2;

            BeginBatch();
            AddCommand(RunnerCommand.Ping, SendID, run_params);
            EndBatch();
            SendBatch();
            bool ret = Recieve();

            if (ret) ReadResults();

            return ret;
        }

        public void BeginBatch()
        {
            IsBatching = true;
            BatchStream = new MemoryStream();
            BatchWriter = new BinaryWriter(BatchStream);

            var pk = new RunnerNetworkPacket();

            BatchWriter.Write(RunnerNetworkPacket.PK_HEADER);
            BatchWriter.Write(Marshal.SizeOf(pk));
            BatchSizeOff = BatchStream.Position;
            BatchWriter.Write(0);
            BatchWriter.Write((int)RunnerCommand.BatchCommand);
            BatchCmdOff = BatchStream.Position;
            BatchCmdCount = 0;
            BatchWriter.Write(BatchCmdCount);

        }

        public void AddCommand(RunnerCommand cmd, params object[] args)
        {
            if (IsBatching)
            {
                switch (cmd)
                {
                    case RunnerCommand.Ping:
                        {
                            BatchCmdCount++;
                            BatchWriter.Write((int)RunnerCommand.Ping);
                            BatchWriter.Write((uint)args[0]); // sendID
                            BatchWriter.Write((int)args[1]); // flags
                            break;
                        }
                    case RunnerCommand.GetUpdate:
                        {
                            BatchCmdCount++;
                            BatchWriter.Write((int)RunnerCommand.GetUpdate);
                            BatchWriter.Write((int)args[0]); // flags
                            break;
                        }
                }
            }
        }

        public void EndBatch()
        {
            if (IsBatching)
            {
                IsBatching = false;
                long pos = BatchStream.Position;
                BatchStream.Seek(BatchSizeOff, SeekOrigin.Begin);
                BatchWriter.Write((uint)pos);
                BatchStream.Seek(BatchCmdOff, SeekOrigin.Begin);
                BatchWriter.Write(BatchCmdCount);
                BatchStream.Seek(pos, SeekOrigin.Begin);
            }
        }

        public bool SendBatch()
        {
            byte[] data = BatchStream.ToArray();
            bool ret = Socket.SendBuffer(data);

            return ret;
        }

        public bool Recieve()
        {
            if (BatchCmdCount > 0)
            {
                int num = Socket.Recieve();
                if (num <= 0) return false;

                return true;
            }

            return false;
        }
    }
}
