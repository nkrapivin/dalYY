namespace dalYY
{
    partial class ConnectionOutputForm
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
            this.ConnLogBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // ConnLogBox
            // 
            this.ConnLogBox.Location = new System.Drawing.Point(0, 0);
            this.ConnLogBox.Margin = new System.Windows.Forms.Padding(0);
            this.ConnLogBox.Name = "ConnLogBox";
            this.ConnLogBox.Size = new System.Drawing.Size(798, 448);
            this.ConnLogBox.TabIndex = 0;
            this.ConnLogBox.Text = "";
            // 
            // ConnectionOutputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ConnLogBox);
            this.Name = "ConnectionOutputForm";
            this.Text = "dalYY : Connection Output";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox ConnLogBox;
    }
}