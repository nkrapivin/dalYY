﻿namespace dalYY
{
    partial class DebugOutputForm
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
            this.DebugLogBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // DebugLogBox
            // 
            this.DebugLogBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DebugLogBox.Location = new System.Drawing.Point(0, 0);
            this.DebugLogBox.Margin = new System.Windows.Forms.Padding(0);
            this.DebugLogBox.Name = "DebugLogBox";
            this.DebugLogBox.Size = new System.Drawing.Size(800, 448);
            this.DebugLogBox.TabIndex = 0;
            this.DebugLogBox.Text = "";
            // 
            // DebugOutputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.DebugLogBox);
            this.Name = "DebugOutputForm";
            this.Text = "dalYY : Debug Output";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox DebugLogBox;
    }
}