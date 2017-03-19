using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using VTree.Models;
using VTree.Events;
using System.Xml;

namespace VTree.Forms
{
    public partial class MainForm : Form
    {
        public char directoryDelimiter = '\\';

        private DirectoryScanner scanner { get; set; }

        private Thread scanThread;

        public delegate void DirectoryScanEventHander(DirectoryScanEventArgs e);

        public event DirectoryScanEventHander onScanStart;
        public event DirectoryScanEventHander onScanFinish;

        public MainForm(DirectoryScanner scanner, string directory, string xmlPath)
        {
            InitializeComponent();

            this.scanner = scanner;
            this.directoryTextBox.Text = directory;
            this.filePathTextBox.Text = xmlPath;

            // scan thread initialization
            this.scanThread = new Thread(this.startScanner);

            this.scanner.onItemFound += (object sender, ItemFoundEventArgs e) =>
            {
                 if (e.Info is DirectoryInfo)
                 {
                     this.drawDirectory(sender, e);
                 }
                 else
                 {
                     this.drawFile(sender, e);
                 }
            };
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = Thread.CurrentThread.ManagedThreadId.ToString();
        }

        private void selectFolderButton_Click(object sender, EventArgs e)
        {
            // select folder
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                directoryTextBox.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void selectFilePathButton_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            if (e.Cancel)
            {
                return;
            }

            filePathTextBox.Text = saveFileDialog1.FileName;
        }

        /**
         * validate and start generating tree
         */
        private void tryStartTree()
        {
            filePathTextBox.BackColor = Form.DefaultBackColor;
            directoryTextBox.BackColor = Form.DefaultBackColor;
            bool isValid = true;

            // todo: check if file is writeable
            if (!(filePathTextBox.Text.Length > 0))
            {
                filePathTextBox.BackColor = Color.Red;

                isValid = false;
            }

            if (! Directory.Exists(directoryTextBox.Text))
            {
                directoryTextBox.BackColor = Color.Red;

                isValid = false;
            }

            if (! isValid)
            {
                return;
            }

            // disable second start!
            this.startButton.Enabled = false;

            if (this.scanThread.IsAlive || this.scanThread.ThreadState == ThreadState.Stopped)
            {
                // stop previous scan and start new one
                this.scanThread.Abort();
                this.scanThread = new Thread(this.startScanner);
            }

            // clear tree first
            this.directoryTree.Nodes.Clear();

            this.scanThread.Start(directoryTextBox.Text);
        }

        private void startScanner(object path)
        {
            Console.WriteLine("start scanning-" + Thread.CurrentThread.ManagedThreadId);

            DirectoryScanEventArgs e = new DirectoryScanEventArgs(
                directoryTextBox.Text,
                filePathTextBox.Text
            );

            this.onScanStart?.Invoke(e);

            try
            {
                // todo: manage errors
                this.scanner.Scan(path.ToString());
            }
            finally
            {
                this.onScanFinish?.Invoke(e);

                // enable start button again
                this.BeginInvoke(new Action(() =>
                {
                    this.startButton.Enabled = true;
                }));
            }
        }

        private void textBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                tryStartTree();
            }
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;

            textBox.BackColor = Form.DefaultBackColor;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            this.tryStartTree();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.scanThread.IsAlive)
            {
                this.scanThread.Abort();
            }
        }

        private TreeNode getNodeByPath(string path)
        {
            TreeNode node = this.directoryTree.Nodes[0];
            if (!path.StartsWith(node.Text))
            {
                throw new Exception("Something got wrong");
            }
            if (path == node.Text)
            {
                return node;
            }

            string rightPart = path.Substring(node.Text.Length)
                    .Trim(this.directoryDelimiter); // trim slashes
            string[] parts = rightPart.Split(this.directoryDelimiter);

            foreach (string dir in parts)
            {
                bool found = false;
                foreach (TreeNode child in node.Nodes)
                {
                    if (child.Text == dir)
                    {
                        node = child;
                        found = true;

                        break;
                    }
                }
                if (!found)
                {
                    throw new Exception("Directory not found");
                }
            }

            return node;
        }

        public void InitializeTree()
        {
            this.directoryTree.Nodes.Clear();
        }

        private void drawFile(object sender, ItemFoundEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() => this.drawFile(sender, e)));

                return;
            }
            FileInfo info = e.Info as FileInfo;
            TreeNode fileNode = new TreeNode(info.Name, 0, 0);
            this.getNodeByPath(info.DirectoryName)
                .Nodes.Add(fileNode);
        }

        private void drawDirectory(object sender, ItemFoundEventArgs e)
        {
            if (this.InvokeRequired)
            {
                Console.WriteLine("drawdirectory requires invoke-" + Thread.CurrentThread.ManagedThreadId);
                this.BeginInvoke(new Action(() => this.drawDirectory(sender, e)));

                return;
            }

            Console.WriteLine("drawdirectory-" + Thread.CurrentThread.ManagedThreadId);
            // root
            if (this.directoryTree.Nodes.Count == 0)
            {
                this.directoryTree.Nodes.Add(new TreeNode(e.Info.FullName, 1, 1));

                return;
            }
            DirectoryInfo info = e.Info as DirectoryInfo;
            TreeNode directoryNode = new TreeNode(info.Name, 1, 1);
            this.getNodeByPath(info.Parent.FullName)
                .Nodes.Add(directoryNode);
        }
    }
}
