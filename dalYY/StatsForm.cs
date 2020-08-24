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
    public partial class StatsForm : Form
    {
        private string OrigText { get; set; }

        public StatsForm()
        {
            InitializeComponent();
            OrigText = labelStats.Text;
        }

        public void SetStats(int fps, uint used, ulong free, RunnerSocketState state)
        {
            labelStats.Text = string.Format(OrigText, fps, used, free, state);
        }
    }
}
