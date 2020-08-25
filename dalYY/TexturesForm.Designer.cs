namespace dalYY
{
    partial class TexturesForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TexturesForm));
            this.pingasBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pingasBox)).BeginInit();
            this.SuspendLayout();
            // 
            // pingasBox
            // 
            this.pingasBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pingasBox.Image = ((System.Drawing.Image)(resources.GetObject("pingasBox.Image")));
            this.pingasBox.Location = new System.Drawing.Point(0, 0);
            this.pingasBox.Margin = new System.Windows.Forms.Padding(0);
            this.pingasBox.Name = "pingasBox";
            this.pingasBox.Size = new System.Drawing.Size(1021, 720);
            this.pingasBox.TabIndex = 0;
            this.pingasBox.TabStop = false;
            // 
            // TexturesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1021, 720);
            this.Controls.Add(this.pingasBox);
            this.Name = "TexturesForm";
            this.Text = "dalYY : Textures / Surfaces";
            ((System.ComponentModel.ISupportInitialize)(this.pingasBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pingasBox;
    }
}