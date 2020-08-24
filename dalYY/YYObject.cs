using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dalYY
{
    public class YYObject
    {
        public int ID { get; set; }
        public int Flags { get; set; }
        public int SpriteIndex { get; set; }
        public int MaskIndex { get; set; }
        public int Depth { get; set; }
        public int Parent { get; set; }
        public string Name { get; set; }
        public List<YYEvent>[] Events { get; set; }
    }
}
