using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dalYY
{
    public partial class DebuggerForm : Form
    {
        private void Trace(string msg) => LogBox.Text += msg + Environment.NewLine;

        private string IP { get; set; }
        private int PORT { get; set; }
        private string YYDEBUG_PATH { get; set; }
        private YYDebug GameData { get; set; }
        private RunnerSocket DebugSocket { get; set; }

        public DebuggerForm()
        {
            InitializeComponent();
        }

        public void SetParams(string ip, int port, string yydebug_path)
        {
            IP = ip;
            PORT = port;
            YYDEBUG_PATH = yydebug_path;
        }

        private void DebuggerForm_Shown(object sender, EventArgs e)
        {
            GameData = new YYDebug(File.OpenRead(YYDEBUG_PATH));
            GameData.Load();
            Trace("yydebug loaded!");
            Connect();
        }

        private void Connect()
        {
            if (PORT > ushort.MaxValue || PORT < 1)
            {
                Trace("Invalid port!");
                return;
            }

            Trace("Trying to connect to the Runner...");

            DebugSocket = new RunnerSocket();
            DebugSocket.YYDbg = GameData;
            DebugSocket.Connect(IP, PORT);
        }
    }
}
