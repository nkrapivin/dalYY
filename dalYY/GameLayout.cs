using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dalYY
{
    public class GameLayout
    {
        private const int GAMELAYOUT_VERSION = 9;

        public int PlatformCode { get; set; }
        public YYDebug GameYYDebug { get; set; }

        public bool Load(byte[] dat)
        {
            var reader = new BinaryReader(new MemoryStream(dat));

            RunnerCommand cmd = (RunnerCommand)reader.ReadInt32();
            if (cmd != RunnerCommand.GetGameData)
            {
                Console.WriteLine("Invalid command, expected GetGameData!");
                return false;
            }

            if (ReadString(reader) != "_REV")
            {
                Console.WriteLine("Wrong chunk name. Expected VER_");
                return false;
            }

            int GL_version = reader.ReadInt32();
            if (GL_version != GAMELAYOUT_VERSION)
            {
                Console.WriteLine("Unsupported GameLayout version, expected 9");
                return false;
            }

            PlatformCode = reader.ReadInt32();

            // And now, the fun part begins. Where we load the actual data.

            if (ReadString(reader) != "EDOC")
            {
                Console.WriteLine("Wrong chunk name. Expected CODE");
                return false;
            }
            Read_VMCode(reader);

            if (ReadString(reader) != "_JBO")
            {
                Console.WriteLine("Wrong chunk name. Expected OBJ_");
                return false;
            }
            Read_OBJT(reader);

            if (ReadString(reader) != "TRCS")
            {
                Console.WriteLine("Wrong chunk name. Expected SCRT");
                return false;
            }



            return true;
        }

        private string ReadString(BinaryReader reader, int count = 4)
        {
            byte[] dat = reader.ReadBytes(count);
            return Encoding.ASCII.GetString(dat);
        }

        private void Read_OBJT(BinaryReader reader)
        {

        }

        private void Read_VMCode(BinaryReader reader)
        {
            int num = reader.ReadInt32();
            for (int i = 0; i < num; i++)
            {
                int index = reader.ReadInt32();
                var script = GameYYDebug.Scripts[index];
                var name = Read_String(reader);
                if (name != script.Name)
                {
                    throw new Exception("Script name mismatch, what the heck?");
                }

                script.BytecodeBlob = Read_BCBlob(reader);
            }
        }

        private byte[] Read_BCBlob(BinaryReader reader)
        {
            int size = reader.ReadInt32();

            byte[] dat = reader.ReadBytes(size);

            return dat;
        }

        private string Read_String(BinaryReader reader)
        {
            uint num = reader.ReadUInt32();
            string text = string.Empty;
            for (num--; num > 0; num--)
            {
                text += (char)(reader.ReadByte() & 0xFF);
            }

            reader.ReadByte();

            return text;
        }
    }
}
