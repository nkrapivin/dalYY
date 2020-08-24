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
    public partial class ConnectionOutputForm : Form
    {
        public ConnectionOutputForm()
        {
            InitializeComponent();
        }

        public void SetText(string text)
        {
            ConnLogBox.Text = text;
        }

        public void Trace(string text)
        {
            ConnLogBox.Text += text + Environment.NewLine;
        }
    }
}
