using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dalYY
{
    public partial class AllInstancesForm : Form
    {
        public AllInstancesForm()
        {
            InitializeComponent();
        }

        public void UpdateInstances(List<YYInstance> list)
        {
            var node = InstancesView.Nodes[0];
            node.Nodes.Clear();
            for (int i = 0; i < list.Count; i++)
            {
                var inst = list[i];
                if (inst.BuiltinVariables == null) continue;
                var item = node.Nodes.Add(inst.ToString());
                var bnvarnode = item.Nodes.Add("< Built-in Variables >");

                for (int j = 0; j < inst.BuiltinVariables.Count; j++)
                {
                    bnvarnode.Nodes.Add(inst.BuiltinVariables[j].ToString());
                }

                for (int j = 0; j < inst.InstVariables.Count; j++)
                {
                    item.Nodes.Add(inst.InstVariables[j].ToString());
                }
            }
        }
    }
}
