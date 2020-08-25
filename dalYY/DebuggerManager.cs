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

        public List<YYInstance> AllInstances { get; set; }
        public List<GMValue> Globals { get; set; }

        public DebuggerManager(RunnerSocket sck)
        {
            Socket = sck;
            DebugOutput = string.Empty;
            AllInstances = new List<YYInstance>();
            Globals = new List<GMValue>();
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
                case RunnerCommand.GetInstanceData:
                    {
                        ReadInstanceResults(reader);
                        break;
                    }
                default: break;
            }
        }

        public void ReadInstanceResults(BinaryReader reader)
        {
            int len = reader.ReadInt32();
            for (int i = 0; i < len; i++)
            {
                bool exists = reader.ReadInt32() != 0;
                if (exists)
                {
                    var __inst = new YYInstance();
                    __inst.Serialize(reader);
                    AllInstances.Add(__inst);
                }
                else
                {
                    Console.WriteLine("No instance exists for ID " + i.ToString());
                }
            }
        }

        public bool ReadUpdateResults(BinaryReader reader)
        {
            bool update_kind = reader.ReadUInt32() != 0;
            bool ret = (!update_kind) ? ReadRunningUpdate(reader) : ReadStoppedUpdate(reader);
            return ret;
        }

        public void ReadGlobals(BinaryReader reader)
        {
            Globals.Clear();
            uint len = reader.ReadUInt32();
            if (len != 0)
            {
                for (int i = 0; i < len; i++)
                {

                }
            }
        }

        public bool ReadRunningUpdate(BinaryReader reader)
        {
            //Console.WriteLine("ReadRunningUpdate()");

            return true;
        }

        public bool ReadStoppedUpdate(BinaryReader reader)
        {
            //Console.WriteLine("ReadStoppedUpdate()");

            return true;
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
            AddCommand(RunnerCommand.GetUpdate, 0x20);
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
                    case RunnerCommand.GetInstanceData:
                        {
                            BatchCmdCount++;
                            BatchWriter.Write((int)RunnerCommand.GetInstanceData);

                            break;
                        }
                }
            }
        }

        public bool StopRunner()
        {
            bool result = Socket.SendCommand((int)RunnerCommand.StopTarget);
            return result;
        }

        public bool RestartRunner()
        {
            bool result = Socket.SendCommand((int)RunnerCommand.RestartTarget);
            return result;
        }

        public bool ContinueRunner()
        {
            bool result = Socket.SendCommand((int)RunnerCommand.StartTarget);
            return result;
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
