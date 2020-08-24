using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace dalYY
{
    public class YYScript
    {
        public string Source { get; set; }
        public string Name { get; set; }
        public uint UnknownIndex { get; set; }
        public int ScriptIndex { get; set; }
        public byte[] BytecodeBlob { get; set; }
    }

    public struct BCMapping
    {
        public uint BCOffset;
        public uint SCOffset;
    }

    public class YYDebug
    {
        private void Trace(string msg) => Debug.WriteLine(msg);

        private const int FORM_MAGIC = 1297239878;
        private const int SCPT_MAGIC = 1414546259;
        private const int DBGI_MAGIC = 1229406788;
        private const int INST_MAGIC = 1414745673;
        private const int LOCL_MAGIC = 1279479628;
        private const int STRG_MAGIC = 1196577875;

        private BinaryReader Reader { get; set; }

        private List<string> Code { get; set; }
        private List<uint> UnknownIndexes { get; set; }
        private List<string> Names { get; set; }

        public List<YYScript> Scripts { get; set; }
        public List<BCMapping> BytecodeMappings { get; private set; }

        public YYDebug(Stream stream)
        {
            Reader = new BinaryReader(stream);
            Trace("Class created!");
        }

        public void Load()
        {
            if (Reader.ReadInt32() != FORM_MAGIC)
            {
                throw new Exception("Invalid header, expected FORM.");
            }
            int fileLen = Reader.ReadInt32(); // length of the file except FORM header and length.

            Trace("Read header!");

            while (Reader.BaseStream.Position < fileLen)
            {
                switch (Reader.ReadInt32()) // read the supposed chunk header.
                {
                    case SCPT_MAGIC:
                        {
                            Load_SCPT();
                            break;
                        }
                    case LOCL_MAGIC:
                        {
                            Load_LOCL();
                            break;
                        }
                    case DBGI_MAGIC:
                        {
                            Load_DBGI();
                            break;
                        }
                    case INST_MAGIC: // always seems to be empty...
                    case STRG_MAGIC: // we never actually need to read the STRG chunk directly.
                    default: break;
                }
            }

            Scripts = new List<YYScript>(Code.Capacity);
            for (int i = 0; i < Code.Capacity; i++)
            {
                Scripts.Add(new YYScript() { Source = Code[i], Name = Names[i], UnknownIndex = UnknownIndexes[i], ScriptIndex = i });
            }

            Trace("Loading finished!");
        }

        private void Load_SCPT()
        {
            Trace("Loading chunk SCPT...");
            int chunkLen = Reader.ReadInt32();
            Trace("Chunk length " + chunkLen);

            int len = Reader.ReadInt32();
            Code = new List<string>(len);
            Trace("Reading " + len + " Scripts");
            for (int i = 0; i < len; i++)
            {
                Read_Script(Reader.ReadUInt32());
            }
        }

        private void Read_Script(uint obj_addr)
        {
            long prev_addr = Reader.BaseStream.Position;

            Reader.BaseStream.Seek(obj_addr, SeekOrigin.Begin);

            uint addr = Reader.ReadUInt32() - 4;

            Reader.BaseStream.Seek(addr, SeekOrigin.Begin);

            string codeString = Encoding.UTF8.GetString(Reader.ReadBytes(Reader.ReadInt32()));
            Debug.Assert(Reader.ReadByte() == 0);

            Code.Add(codeString);

            Reader.BaseStream.Seek(prev_addr, SeekOrigin.Begin);
        }

        private void Load_DBGI()
        {
            Trace("Loading chunk DBGI...");

            int chunkLen = Reader.ReadInt32();
            Trace("Chunk length " + chunkLen);

            int len = Reader.ReadInt32();
            BytecodeMappings = new List<BCMapping>(len);
            Trace("Reading " + len + " mappings");
            for (int i = 0; i < len; i++)
            {
                Read_Mapping(Reader.ReadUInt32());
            }
        }

        private void Read_Mapping(uint obj_addr)
        {
            long prev_addr = Reader.BaseStream.Position;
            Reader.BaseStream.Seek(obj_addr, SeekOrigin.Begin);

            uint bcOffset = Reader.ReadUInt32();
            uint scOffset = Reader.ReadUInt32();
            var bc = new BCMapping();
            bc.BCOffset = bcOffset;
            bc.SCOffset = scOffset;
            BytecodeMappings.Add(bc);

            Reader.BaseStream.Seek(prev_addr, SeekOrigin.Begin);
        }

        private void Load_LOCL()
        {
            Trace("Loading chunk LOCL...");
            int chunkLen = Reader.ReadInt32();
            Trace("Chunk length " + chunkLen);

            int len = Reader.ReadInt32();
            Names = new List<string>(len);
            UnknownIndexes = new List<uint>(len);
            Trace("Reading " + len + " names");
            for (int i = 0; i < len; i++)
            {
                Read_Name(Reader.ReadUInt32());
            }
        }

        private void Read_Name(uint obj_addr)
        {
            long prev_addr = Reader.BaseStream.Position;
            Reader.BaseStream.Seek(obj_addr, SeekOrigin.Begin);

            UnknownIndexes.Add(Reader.ReadUInt32()); // ???
            uint str_addr = Reader.ReadUInt32() - 4;

            Reader.BaseStream.Seek(str_addr, SeekOrigin.Begin);

            string codeString = Encoding.UTF8.GetString(Reader.ReadBytes(Reader.ReadInt32()));
            Debug.Assert(Reader.ReadByte() == 0);
            Names.Add(codeString);

            Reader.BaseStream.Seek(prev_addr, SeekOrigin.Begin);
        }
    }
}
