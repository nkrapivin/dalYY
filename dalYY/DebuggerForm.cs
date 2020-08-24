﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
        private DebuggerManager Manager { get; set; }

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
            Trace("Loading .yydebug...");
            GameData = new YYDebug(File.OpenRead(YYDEBUG_PATH));
            GameData.Load();
            Trace("Loaded!");

            DebugSocket = new RunnerSocket();
            DebugSocket.YYDbg = GameData;
            Manager = new DebuggerManager(DebugSocket);
            Connect();
        }

        private void Connect()
        {
            if (PORT > ushort.MaxValue || PORT < 1)
            {
                Trace("Invalid port! Out of allowed range (0-65535)");
                return;
            }
            debugWorker.RunWorkerAsync();
        }

        private void debugWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            var ret = DebugSocket.Connect(IP, PORT, worker);

            if (ret == RunnerConnErr.None)
            {
                while (true)
                {
                    bool isret  = Manager.IsRunning();
                    bool updret = Manager.Update(isret);
                }
            }
        }

        private void debugWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Trace(e.UserState as string);
        }

        private void debugTimer_Tick(object sender, EventArgs e)
        {
            labelFPS.Text = $"FPS: {Manager.FPS}";
            labelUsedMem.Text = $"Used MEM: {Manager.UsedMem}";
            labelFreeMem.Text = $"Free MEM: {Manager.FreeMem}";
            if (Manager.DebugOutput.Length > 2) dbgOutputBox.Text = Manager.DebugOutput;
        }
    }
}
