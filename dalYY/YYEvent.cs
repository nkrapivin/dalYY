using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dalYY
{
    public class YYEvent
    {
        public int EventIndex { get; set; }
        public int SubEvent { get; set; }
        public string ScriptName { get; set; }
        public int ScriptIndex { get; set; }
        public ulong ScriptBaseAddr { get; set; }
        public YYObject Object { get; set; }

        public void Load(BinaryReader reader, YYObject _obj = null)
        {
            SubEvent = reader.ReadInt32();
            ScriptIndex = reader.ReadInt32();
            ScriptBaseAddr = reader.ReadUInt64();
            ScriptName = Read_String(reader);
            Object = _obj;
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
