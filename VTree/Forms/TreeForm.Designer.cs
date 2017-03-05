namespace VTree.Forms
{
    partial class TreeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TreeForm));
            this.directoryTree = new System.Windows.Forms.TreeView();
            this.iconList = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // directoryTree
            // 
            this.directoryTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.directoryTree.ImageIndex = 0;
            this.directoryTree.ImageList = this.iconList;
            this.directoryTree.Location = new System.Drawing.Point(0, 0);
            this.directoryTree.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.directoryTree.Name = "directoryTree";
            this.directoryTree.SelectedImageIndex = 0;
            this.directoryTree.Size = new System.Drawing.Size(212, 206);
            this.directoryTree.TabIndex = 0;
            // 
            // iconList
            // 
            this.iconList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("iconList.ImageStream")));
            this.iconList.TransparentColor = System.Drawing.Color.Transparent;
            this.iconList.Images.SetKeyName(0, "file-ico.png");
            this.iconList.Images.SetKeyName(1, "folder-ico.png");
            this.iconList.Images.SetKeyName(2, "error.png");
            // 
            // TreeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(212, 206);
            this.Controls.Add(this.directoryTree);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "TreeForm";
            this.Text = "TreeForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TreeForm_FormClosing);
            this.Load += new System.EventHandler(this.TreeForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView directoryTree;
        private System.Windows.Forms.ImageList iconList;
    }
}