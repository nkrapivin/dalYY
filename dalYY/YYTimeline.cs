using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dalYY
{
    public class YYTimeline
    {
        public List<YYScript> Scripts { get; set; }
        public string Name { get; set; }
        public int Index { get; set; }

        public YYTimeline(int index, int numSteps)
        {
            Index = index;
            Scripts = new List<YYScript>(numSteps);
        }
    }
}
