using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dalYY
{
    public class YYRoom
    {
        public string Name { get; set; }
        public List<YYScript> Scripts { get; set; }

        public YYRoom(string _name)
        {
            Name = _name;
            Scripts = new List<YYScript>();
        }

        public YYScript ScriptByName(string _name)
        {
            foreach (YYScript scr in Scripts)
            {
                if (scr.Name.Equals(_name))
                {
                    return scr;
                }
            }

            return null;
        }
    }
}
