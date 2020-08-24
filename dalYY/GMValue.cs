using System.Runtime.CompilerServices;

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
		public ulong ArrayPtr { get; set; }
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
    }
}
