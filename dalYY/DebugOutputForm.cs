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
    public partial class DebugOutputForm : Form
    {
        public StringBuilder Builder { get; set; }

        public DebugOutputForm()
        {
            InitializeComponent();
            Builder = new StringBuilder();
        }

        public void SetText(string text)
        {
            if (text.Length <= 0) return;

            string value = text.Replace("\n", Environment.NewLine);
            Builder.Append(value);

            DebugLogBox.Text = Builder.ToString();
        }

        public void AddText(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return;
            DebugLogBox.Text += text;
        }
    }
}
