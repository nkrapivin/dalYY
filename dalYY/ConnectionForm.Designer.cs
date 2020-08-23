namespace dalYY
{
    partial class ConnectionForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelIP = new System.Windows.Forms.Label();
            this.labelYYDebugFile = new System.Windows.Forms.Label();
            this.labelPort = new System.Windows.Forms.Label();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.tbtIP = new System.Windows.Forms.TextBox();
            this.tbtPort = new System.Windows.Forms.TextBox();
            this.tbtYYDebug = new System.Windows.Forms.TextBox();
            this.btnChooseFile = new System.Windows.Forms.Button();
            this.labelQuote = new System.Windows.Forms.Label();
            this.openYYDebugDialog = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // labelIP
            // 
            this.labelIP.AutoSize = true;
            this.labelIP.Location = new System.Drawing.Point(5, 31);
            this.labelIP.Name = "labelIP";
            this.labelIP.Size = new System.Drawing.Size(64, 13);
            this.labelIP.TabIndex = 0;
            this.labelIP.Text = "IP Address: ";
            // 
            // labelYYDebugFile
            // 
            this.labelYYDebugFile.AutoSize = true;
            this.labelYYDebugFile.Location = new System.Drawing.Point(5, 83);
            this.labelYYDebugFile.Name = "labelYYDebugFile";
            this.labelYYDebugFile.Size = new System.Drawing.Size(72, 13);
            this.labelYYDebugFile.TabIndex = 1;
            this.labelYYDebugFile.Text = ".yydebug file: ";
            // 
            // labelPort
            // 
            this.labelPort.AutoSize = true;
            this.labelPort.Location = new System.Drawing.Point(5, 57);
            this.labelPort.Name = "labelPort";
            this.labelPort.Size = new System.Drawing.Size(121, 13);
            this.labelPort.TabIndex = 2;
            this.labelPort.Text = "Debug Port (def. 6502): ";
            // 
            // buttonConnect
            // 
            this.buttonConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonConnect.Location = new System.Drawing.Point(620, 106);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(75, 23);
            this.buttonConnect.TabIndex = 3;
            this.buttonConnect.Text = "Connect!";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // tbtIP
            // 
            this.tbtIP.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbtIP.Location = new System.Drawing.Point(133, 28);
            this.tbtIP.Name = "tbtIP";
            this.tbtIP.Size = new System.Drawing.Size(562, 20);
            this.tbtIP.TabIndex = 4;
            this.tbtIP.Text = "127.0.0.1";
            // 
            // tbtPort
            // 
            this.tbtPort.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbtPort.Location = new System.Drawing.Point(133, 54);
            this.tbtPort.MaxLength = 5;
            this.tbtPort.Name = "tbtPort";
            this.tbtPort.Size = new System.Drawing.Size(562, 20);
            this.tbtPort.TabIndex = 5;
            this.tbtPort.Text = "6502";
            // 
            // tbtYYDebug
            // 
            this.tbtYYDebug.Location = new System.Drawing.Point(133, 80);
            this.tbtYYDebug.Name = "tbtYYDebug";
            this.tbtYYDebug.Size = new System.Drawing.Size(532, 20);
            this.tbtYYDebug.TabIndex = 6;
            // 
            // btnChooseFile
            // 
            this.btnChooseFile.Location = new System.Drawing.Point(671, 80);
            this.btnChooseFile.MaximumSize = new System.Drawing.Size(24, 20);
            this.btnChooseFile.Name = "btnChooseFile";
            this.btnChooseFile.Size = new System.Drawing.Size(24, 20);
            this.btnChooseFile.TabIndex = 7;
            this.btnChooseFile.Text = "...";
            this.btnChooseFile.UseVisualStyleBackColor = true;
            this.btnChooseFile.Click += new System.EventHandler(this.btnChooseFile_Click);
            // 
            // labelQuote
            // 
            this.labelQuote.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelQuote.AutoSize = true;
            this.labelQuote.Location = new System.Drawing.Point(12, 119);
            this.labelQuote.Name = "labelQuote";
            this.labelQuote.Size = new System.Drawing.Size(85, 13);
            this.labelQuote.TabIndex = 8;
            this.labelQuote.Text = "Random Quote: ";
            // 
            // openYYDebugDialog
            // 
            this.openYYDebugDialog.DefaultExt = "yydebug";
            this.openYYDebugDialog.FileName = "game.yydebug";
            this.openYYDebugDialog.Filter = "Debugger Files|*.yydebug|All Files|*.*";
            this.openYYDebugDialog.Title = "Choose your .yydebug file...";
            // 
            // ConnectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(707, 141);
            this.Controls.Add(this.labelQuote);
            this.Controls.Add(this.btnChooseFile);
            this.Controls.Add(this.tbtYYDebug);
            this.Controls.Add(this.tbtPort);
            this.Controls.Add(this.tbtIP);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.labelPort);
            this.Controls.Add(this.labelYYDebugFile);
            this.Controls.Add(this.labelIP);
            this.MinimumSize = new System.Drawing.Size(723, 180);
            this.Name = "ConnectionForm";
            this.Text = "dalYY : Connection Setup";
            this.Shown += new System.EventHandler(this.ConnectionForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelIP;
        private System.Windows.Forms.Label labelYYDebugFile;
        private System.Windows.Forms.Label labelPort;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.TextBox tbtIP;
        private System.Windows.Forms.TextBox tbtPort;
        private System.Windows.Forms.TextBox tbtYYDebug;
        private System.Windows.Forms.Button btnChooseFile;
        private System.Windows.Forms.Label labelQuote;
        private System.Windows.Forms.OpenFileDialog openYYDebugDialog;
    }
}

