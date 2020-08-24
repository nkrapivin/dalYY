namespace dalYY
{
    partial class AllInstancesForm
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Instances");
            this.InstancesView = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // InstancesView
            // 
            this.InstancesView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InstancesView.Location = new System.Drawing.Point(0, 0);
            this.InstancesView.Margin = new System.Windows.Forms.Padding(0);
            this.InstancesView.Name = "InstancesView";
            treeNode1.Name = "Instances";
            treeNode1.Text = "Instances";
            this.InstancesView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.InstancesView.Size = new System.Drawing.Size(799, 449);
            this.InstancesView.TabIndex = 0;
            // 
            // AllInstancesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.InstancesView);
            this.Name = "AllInstancesForm";
            this.Text = "dalYY : All Instances";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView InstancesView;
    }
}