using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dalYY
{
    public class YYVariable
    {
        public string Name { get; private set; }
        public GMValue.GMValueType VarType { get; private set; }
        public string ValueStr { get; set; }
        public double ValueDbl { get; set; }
        public ulong ValueArr { get; set; }
        public ulong ValueObj { get; set; }
        public bool ReadOnly { get; set; }

    }
}
