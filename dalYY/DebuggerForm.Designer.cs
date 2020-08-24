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
            this.debugWorker = new System.ComponentModel.BackgroundWorker();
            this.debugTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // debugWorker
            // 
            this.debugWorker.WorkerReportsProgress = true;
            this.debugWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.debugWorker_DoWork);
            this.debugWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.debugWorker_ProgressChanged);
            // 
            // debugTimer
            // 
            this.debugTimer.Enabled = true;
            this.debugTimer.Interval = 120;
            this.debugTimer.Tick += new System.EventHandler(this.debugTimer_Tick);
            // 
            // DebuggerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1469, 705);
            this.IsMdiContainer = true;
            this.Name = "DebuggerForm";
            this.Text = "dalYY : Debugger Form ";
            this.Shown += new System.EventHandler(this.DebuggerForm_Shown);
            this.ResumeLayout(false);

        }

        #endregion
        private System.ComponentModel.BackgroundWorker debugWorker;
        private System.Windows.Forms.Timer debugTimer;
    }
}