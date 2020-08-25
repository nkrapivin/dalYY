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

    public struct Keyword
    {
        public string Content;
        public bool IsFunction;
    }

    public class GameLayout
    {
        private const int GAMELAYOUT_VERSION = 9;

        public Platform PlatformCode { get; set; }
        public YYDebug GameYYDebug { get; set; }
        public List<YYObject> Objects { get; set; }

        public ulong StringBasePointer { get; set; }
        public List<Keyword> Keywords { get; set; }
        public Dictionary<int, string> Variables { get; set; }
        public Dictionary<string, Dictionary<int, string>> LocalVariables { get; set; }
        public List<string> Strings { get; set; }
        public List<YYRoom> Rooms { get; set; }
        public List<YYTimeline> Timelines { get; set; }

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

            if (ReadString(reader) != "CNUF")
            {
                Console.WriteLine("Wrong chunk name. Expected FUNC");
                return false;
            }
            Read_FUNC(reader);

            if (ReadString(reader) != "DOCC")
            {
                Console.WriteLine("Wrong chunk name. Expected CCOD");
                return false;
            }
            Read_CreationCode(reader);

            if (ReadString(reader) != "NLMT")
            {
                Console.WriteLine("Wrong chunk name. Expected TMLN");
                return false;
            }
            Read_TimelineCode(reader);

            return true;
        }

        private void Read_TimelineCode(BinaryReader reader)
        {
            int len = reader.ReadInt32();
            if (len <= 0) return;

            Timelines = new List<YYTimeline>(len);
            for (int i = 0; i < len; i++)
            {
                string name = Read_String(reader);
                int numSteps = reader.ReadInt32();
                var timeline = new YYTimeline(i, numSteps);
                timeline.Name = name;
                Timelines.Add(timeline);

                for (int j = 0; j < numSteps; j++)
                {
                    int step = reader.ReadInt32();
                    int index = reader.ReadInt32();
                    ulong bcAddr = reader.ReadUInt64();
                    var script = GameYYDebug.Scripts[index];
                    script.BaseAddr = bcAddr;
                    //script.Name = $"Step {step}";
                    script.Type = YYScriptType.TimelineAction;
                    timeline.Scripts.Add(script);
                }
            }
        }

        private void Read_CreationCode(BinaryReader reader)
        {
            int len = reader.ReadInt32();
            Rooms = new List<YYRoom>(len);
            for (int i = 0; i < len; i++)
            {
                string name = Read_String(reader);
                var room = new YYRoom(name);
                Rooms.Add(room);
                uint index = reader.ReadUInt32();
                if (index != uint.MaxValue)
                {
                    var script = GameYYDebug.Scripts[(int)index];
                    script.BaseAddr = reader.ReadUInt64();
                    script.Name = $"{name}.RoomCreationCode";
                    script.Type = YYScriptType.RoomCreation;
                    room.Scripts.Add(script);
                }
                int length = reader.ReadInt32();
                for (int j = 0; j < length; j++)
                {
                    index = reader.ReadUInt32();
                    var script = GameYYDebug.Scripts[(int)index];
                    script.BaseAddr = reader.ReadUInt64();
                    script.Name = Read_String(reader);
                    reader.ReadUInt32();
                    script.Type = YYScriptType.InstanceCreation;
                    room.Scripts.Add(script);
                }
            }
        }

        private void Read_FUNC(BinaryReader reader)
        {
            int len;

            Keywords = new List<Keyword>();
            Variables = new Dictionary<int, string>();
            LocalVariables = new Dictionary<string, Dictionary<int, string>>();
            Strings = new List<string>();

            // Built-in functions
            len = reader.ReadInt32();
            for (int i = 0; i < len; i++)
            {
                string keyword = Read_String(reader);
                var _k = new Keyword() { Content = keyword, IsFunction = true };
                Keywords.Add(_k);
            }

            // Built-in variables
            len = reader.ReadInt32();
            for (int i = 0; i < len; i++)
            {
                string keyword = Read_String(reader);
                var _k = new Keyword() { Content = keyword, IsFunction = false };
                Keywords.Add(_k);
            }

            // Internal variables
            len = reader.ReadInt32();
            for (int i = 0; i < len; i++)
            {
                int key = reader.ReadInt32();
                string value = Read_String(reader);
                var keyword = new Keyword() { Content = value, IsFunction = false };
                Keywords.Add(keyword);
                Variables.Add(key, value);
            }

            // Other variables??
            len = reader.ReadInt32();
            for (int i = 0; i < len; i++)
            {
                int key = reader.ReadInt32();
                string value = Read_String(reader);
                var keyword = new Keyword() { Content = value, IsFunction = false };
                Keywords.Add(keyword);
                Variables.Add(key, value);
            }

            // Local variables
            len = reader.ReadInt32();
            for (int i = 0; i < len; i++)
            {
                string codeEntry = Read_String(reader);
                int length = reader.ReadInt32();
                LocalVariables.Add(codeEntry, new Dictionary<int, string>());
                for (int j = 0; j < length; j++)
                {
                    int key = reader.ReadInt32();
                    string value = Read_String(reader);
                    LocalVariables[codeEntry].Add(key, value);
                    var keyword = new Keyword() { Content = value, IsFunction = false };
                    Keywords.Add(keyword);
                }
            }

            StringBasePointer = reader.ReadUInt64();
            len = reader.ReadInt32();
            for (int i = 0; i < len; i++)
            {
                string str = Read_String(reader);
                Strings.Add(str);
            }
        }

        private string ReadString(BinaryReader reader, int count = 4)
        {
            byte[] dat = reader.ReadBytes(count);
            return Encoding.ASCII.GetString(dat);
        }

        private void Read_SCRT(BinaryReader reader)
        {
            int len = reader.ReadInt32();
            for (int i = 0; i < len; i++)
            {
                int index = reader.ReadInt32();
                if (index >= 0)
                {
                    var _script = GameYYDebug.Scripts[index];
                    var base_addr = reader.ReadUInt64();
                    reader.ReadUInt64();
                    string display_name = Read_String(reader);
                    _script.Type = YYScriptType.Script;
                    
                }
                else
                {
                    var _name = Read_String(reader);
                }
            }
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


                Objects.Add(obj);
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
