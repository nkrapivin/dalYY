using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dalYY
{
    public enum Platform : int
    {
        Win32,
        Mac,
        PSP,
        IOS,
        Android,
        Symbian,
        Linux,
        WinPhone,
        Tizen,
        Win8Native,
        WiiU,
        N3DS,
        PSVita,
        BB10,
        PS4,
        XboxOne,
        PS3
    }

    public class GameLayout
    {
        private const int GAMELAYOUT_VERSION = 9;

        public Platform PlatformCode { get; set; }
        public YYDebug GameYYDebug { get; set; }
        public List<YYObject> Objects { get; set; }

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

            PlatformCode = (Platform)reader.ReadInt32();

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
            Read_SCRT(reader);


            return true;
        }

        private string ReadString(BinaryReader reader, int count = 4)
        {
            byte[] dat = reader.ReadBytes(count);
            return Encoding.ASCII.GetString(dat);
        }

        private void Read_SCRT(BinaryReader reader)
        {

        }

        private void Read_OBJT(BinaryReader reader)
        {
            int len = reader.ReadInt32();
            Objects = new List<YYObject>(len);
            for (int i = 0; i < len; i++)
            {
                var obj = new YYObject();
                obj.ID = reader.ReadInt32();
                obj.Flags = reader.ReadInt32();
                obj.SpriteIndex = reader.ReadInt32();
                obj.MaskIndex = reader.ReadInt32();
                obj.Depth = reader.ReadInt32();
                obj.Parent = reader.ReadInt32();
                obj.Name = Read_String(reader);
                obj.Events = new List<YYEvent>[15];
                for (int j = 0; j <= 14; j++)
                {
                    obj.Events[j] = new List<YYEvent>();
                    int ev_len = reader.ReadInt32();
                    for (int k = 0; k < ev_len; k++)
                    {
                        var ev = new YYEvent();
                        ev.Load(reader, obj);
                        obj.Events[j].Add(ev);
                    }
                }
            }
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
