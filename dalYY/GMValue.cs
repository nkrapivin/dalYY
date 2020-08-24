namespace dalYY
{
    public struct GMValue
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

		public GMValueType ValType;
		public double Number;
		public string String;
		public ulong ArrayPtr;
		public GMJsonKind Tag;
	}
}
