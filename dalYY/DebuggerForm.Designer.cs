namespace dalYY
{
    partial class DebuggerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.LogBox = new System.Windows.Forms.RichTextBox();
            this.debugWorker = new System.ComponentModel.BackgroundWorker();
            this.labelFPS = new System.Windows.Forms.Label();
            this.labelUsedMem = new System.Windows.Forms.Label();
            this.labelFreeMem = new System.Windows.Forms.Label();
            this.debugTimer = new System.Windows.Forms.Timer(this.components);
            this.dbgOutputBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // LogBox
            // 
            this.LogBox.Location = new System.Drawing.Point(13, 13);
            this.LogBox.Name = "LogBox";
            this.LogBox.Size = new System.Drawing.Size(396, 399);
            this.LogBox.TabIndex = 0;
            this.LogBox.Text = "";
            // 
            // debugWorker
            // 
            this.debugWorker.WorkerReportsProgress = true;
            this.debugWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.debugWorker_DoWork);
            this.debugWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.debugWorker_ProgressChanged);
            // 
            // labelFPS
            // 
            this.labelFPS.AutoSize = true;
            this.labelFPS.Location = new System.Drawing.Point(10, 415);
            this.labelFPS.Name = "labelFPS";
            this.labelFPS.Size = new System.Drawing.Size(39, 13);
            this.labelFPS.TabIndex = 1;
            this.labelFPS.Text = "FPS: 0";
            // 
            // labelUsedMem
            // 
            this.labelUsedMem.AutoSize = true;
            this.labelUsedMem.Location = new System.Drawing.Point(372, 415);
            this.labelUsedMem.Name = "labelUsedMem";
            this.labelUsedMem.Size = new System.Drawing.Size(71, 13);
            this.labelUsedMem.TabIndex = 2;
            this.labelUsedMem.Text = "Used RAM: 0";
            // 
            // labelFreeMem
            // 
            this.labelFreeMem.AutoSize = true;
            this.labelFreeMem.Location = new System.Drawing.Point(721, 415);
            this.labelFreeMem.Name = "labelFreeMem";
            this.labelFreeMem.Size = new System.Drawing.Size(67, 13);
            this.labelFreeMem.TabIndex = 3;
            this.labelFreeMem.Text = "Free RAM: 0";
            // 
            // debugTimer
            // 
            this.debugTimer.Enabled = true;
            this.debugTimer.Interval = 120;
            this.debugTimer.Tick += new System.EventHandler(this.debugTimer_Tick);
            // 
            // dbgOutputBox
            // 
            this.dbgOutputBox.Location = new System.Drawing.Point(416, 13);
            this.dbgOutputBox.Name = "dbgOutputBox";
            this.dbgOutputBox.Size = new System.Drawing.Size(372, 399);
            this.dbgOutputBox.TabIndex = 4;
            this.dbgOutputBox.Text = "";
            // 
            // DebuggerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dbgOutputBox);
            this.Controls.Add(this.labelFreeMem);
            this.Controls.Add(this.labelUsedMem);
            this.Controls.Add(this.labelFPS);
            this.Controls.Add(this.LogBox);
            this.Name = "DebuggerForm";
            this.Text = "dalYY : Debugger Form ";
            this.Shown += new System.EventHandler(this.DebuggerForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox LogBox;
        private System.ComponentModel.BackgroundWorker debugWorker;
        private System.Windows.Forms.Label labelFPS;
        private System.Windows.Forms.Label labelUsedMem;
        private System.Windows.Forms.Label labelFreeMem;
        private System.Windows.Forms.Timer debugTimer;
        private System.Windows.Forms.RichTextBox dbgOutputBox;
    }
}