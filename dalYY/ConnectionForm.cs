using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace dalYY
{
    public partial class ConnectionForm : Form
    {
        private const int CONF_VERSION = 1;
        private const string CONF_FILE = "config.txt";
        private const string CONF_HEADER = "This is a 'Debugger a la YoYo' configuration file. Do not mess with it PLEASE.";
        private readonly string APPLICATION_DIR = AppDomain.CurrentDomain.BaseDirectory;
        private bool DoQuitApp { get; set; }

        public ConnectionForm()
        {
            InitializeComponent();
        }

        private void ConnectionForm_Shown(object sender, EventArgs e)
        {
            labelQuote.Text += GetRandomQuote();
            DoQuitApp = true;
            if (Debugger.IsAttached) Text += " (Unlike YoYo, I don't care if you have a debugger)";
            LoadConfig(APPLICATION_DIR + CONF_FILE);
        }

        private void LoadConfig(string path)
        {
            try
            {
                if (!File.Exists(path)) return;
                else
                {
                    string[] conf = File.ReadAllLines(path);
                    //string hdr = conf[0];
                    tbtIP.Text = conf[2];
                    tbtPort.Text = conf[3];
                    tbtYYDebug.Text = conf[4];
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("Could not load debugger configuration. Make sure the debugger can read it's own directory! Error: " + exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void SaveConfig(string path)
        {
            try
            {
                string conf = string.Empty;

                conf += CONF_HEADER + Environment.NewLine;
                conf += CONF_VERSION.ToString() + Environment.NewLine;
                conf += tbtIP.Text + Environment.NewLine;
                conf += tbtPort.Value.ToString() + Environment.NewLine;
                conf += tbtYYDebug.Text + Environment.NewLine;
                conf += "RESERVED1" + Environment.NewLine;
                conf += "RESERVED2" + Environment.NewLine;

                File.WriteAllText(path, conf);
            }
            catch (Exception exc)
            {
                MessageBox.Show("Could not save debugger configuration. Make sure the debugger can write to it's own directory! Error: " + exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private string GetRandomQuote()
        {
            string[] quotes =
            {
                "do not bother the pug",
                "black lives do matter",
                "#australiaDoesExist",
                "huh, a debugger?",
                "0% bad code (hopefully)",
                "Unsupported protocol version, got 12, expected NEIN!"
                // To Be Filled...
            };

            var index = new Random().Next(0, quotes.Length);
            return quotes[index];
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            SaveConfig(APPLICATION_DIR + CONF_FILE);
            var frm = new DebuggerForm();
            frm.SetParams(tbtIP.Text, (int)tbtPort.Value, tbtYYDebug.Text);
            frm.Show();
            DoQuitApp = false;
            Close();
        }

        private void btnChooseFile_Click(object sender, EventArgs e)
        {
            if (openYYDebugDialog.ShowDialog() == DialogResult.OK)
            {
                // a quick check to see if we can *actually* read the file.
                var stream = openYYDebugDialog.OpenFile();
                if (stream != null)
                {
                    // yup, we can read it.
                    tbtYYDebug.Text = openYYDebugDialog.FileName;
                    stream.Dispose();
                }
                else
                {
                    MessageBox.Show("Could not open .yydebug file. Make sure the debugger can read files from the chosen directory!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ConnectionForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (DoQuitApp) Application.Exit();
        }
    }
}
