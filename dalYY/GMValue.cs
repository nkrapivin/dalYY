using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace dalYY
{
	public class GMValue
	{
		public enum GMValueType : uint
		{
			Real,
			String,
			Array,
			Pointer,
			Vec3,
			Undefined,
			Object,
			Int32,
			Vec4,
			Matrix,
			Int64,
			Accessor,
			Null,
			Bool,
			Iterator,
			ArrayNode,
			GridNode,
			InstanceNode,
			DSGroupNode,
			Invalid
		}

		// DID SOMEONE JUST SAID JSON!?!?!?!?!??!?!?!?!?!? JAAAAAAAAAAAAAAAAAAAAAAAAAYSOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOONNNNNNNNNNNNNNNNNNNNNNNNNNNN!!!!!!!!!!!
		// but if seriously these masks are used by the debugger, when you click "View As..." and then a corresponding Data Structure.
		public enum GMJsonKind : uint
		{
			Mask = 4026531840u,
			Map = 2147483648u,
			List = 1073741824u
		}

		public string Name { get; set; }
		public GMValueType ValType { get; set; }
		public double Number { get; set; }
		public string String { get; set; }
		public ulong Pointer { get; set; }
		public bool IsUndefined { get; set; }
		public GMJsonKind Tag { get; set; }

        public override string ToString()
        {
            switch (ValType)
            {
				case GMValueType.String:
                    {
						return $"{Name} | {String}";
                    }
				default:
                    {
						return $"{Name} | {Number}";
                    }
            }
        }

		public string ReadStringUTF8(BinaryReader reader)
		{
			int len = reader.ReadInt32();
			string _out = Encoding.UTF8.GetString(reader.ReadBytes(len));
			reader.ReadByte(); // 0x00
			return _out;
		}

		public void ReadFromBuffer(BinaryReader reader)
        {
			uint typ = reader.ReadUInt32();
			ValType = (GMValueType)(typ & 0xFFFFFFF);
			IsUndefined = false;
			switch (ValType)
            {
				case GMValueType.Real:
				case GMValueType.Bool:
					Number = reader.ReadDouble();
					break;
				case GMValueType.Array:
				case GMValueType.Pointer:
				case GMValueType.Object:
				case GMValueType.Int64:
					Pointer = reader.ReadUInt64();
					break;
				case GMValueType.Undefined:
					String = "<undefined>";
					Number = 0;
					Pointer = 0;
					IsUndefined = true;
					break;
				case GMValueType.String:
					String = ReadStringUTF8(reader);
					break;
            }
        }
    }
}
