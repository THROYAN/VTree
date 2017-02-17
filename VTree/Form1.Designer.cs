namespace VTree
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.directoryTree = new System.Windows.Forms.TreeView();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.filePathTextBox = new System.Windows.Forms.TextBox();
            this.directoryTextBox = new System.Windows.Forms.TextBox();
            this.selectDirectoryButton = new System.Windows.Forms.Button();
            this.selectFilePathButton = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog1_FileOk);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.directoryTree);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(573, 401);
            this.splitContainer1.SplitterDistance = 340;
            this.splitContainer1.TabIndex = 0;
            // 
            // directoryTree
            // 
            this.directoryTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.directoryTree.ImageIndex = 0;
            this.directoryTree.ImageList = this.imageList1;
            this.directoryTree.Location = new System.Drawing.Point(0, 0);
            this.directoryTree.Name = "directoryTree";
            this.directoryTree.SelectedImageIndex = 0;
            this.directoryTree.Size = new System.Drawing.Size(573, 340);
            this.directoryTree.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.filePathTextBox);
            this.splitContainer2.Panel1.Controls.Add(this.directoryTextBox);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.selectDirectoryButton);
            this.splitContainer2.Panel2.Controls.Add(this.selectFilePathButton);
            this.splitContainer2.Size = new System.Drawing.Size(573, 57);
            this.splitContainer2.SplitterDistance = 395;
            this.splitContainer2.TabIndex = 2;
            // 
            // filePathTextBox
            // 
            this.filePathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePathTextBox.Location = new System.Drawing.Point(3, 32);
            this.filePathTextBox.Name = "filePathTextBox";
            this.filePathTextBox.Size = new System.Drawing.Size(389, 22);
            this.filePathTextBox.TabIndex = 1;
            this.filePathTextBox.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // directoryTextBox
            // 
            this.directoryTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.directoryTextBox.Location = new System.Drawing.Point(3, 4);
            this.directoryTextBox.Name = "directoryTextBox";
            this.directoryTextBox.Size = new System.Drawing.Size(389, 22);
            this.directoryTextBox.TabIndex = 1;
            this.directoryTextBox.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            this.directoryTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBox_KeyUp);
            // 
            // selectDirectoryButton
            // 
            this.selectDirectoryButton.Location = new System.Drawing.Point(3, 3);
            this.selectDirectoryButton.Name = "selectDirectoryButton";
            this.selectDirectoryButton.Size = new System.Drawing.Size(168, 23);
            this.selectDirectoryButton.TabIndex = 0;
            this.selectDirectoryButton.Text = "Select folder";
            this.selectDirectoryButton.UseVisualStyleBackColor = true;
            this.selectDirectoryButton.Click += new System.EventHandler(this.selectFolderButton_Click);
            // 
            // selectFilePathButton
            // 
            this.selectFilePathButton.Location = new System.Drawing.Point(3, 32);
            this.selectFilePathButton.Name = "selectFilePathButton";
            this.selectFilePathButton.Size = new System.Drawing.Size(168, 23);
            this.selectFilePathButton.TabIndex = 0;
            this.selectFilePathButton.Text = "Select XML file path";
            this.selectFilePathButton.UseVisualStyleBackColor = true;
            this.selectFilePathButton.Click += new System.EventHandler(this.selectFilePathButton_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "file icon");
            this.imageList1.Images.SetKeyName(1, "directory icon");
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(573, 401);
            this.Controls.Add(this.splitContainer1);
            this.Name = "MainForm";
            this.Text = "Visual Tree";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TextBox filePathTextBox;
        private System.Windows.Forms.TextBox directoryTextBox;
        private System.Windows.Forms.Button selectDirectoryButton;
        private System.Windows.Forms.Button selectFilePathButton;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TreeView directoryTree;
        private System.Windows.Forms.ImageList imageList1;
    }
}

