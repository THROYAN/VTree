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

namespace VTree
{
    public partial class MainForm : Form
    {
        public char directoryDelimiter = '\\';

        private DirectoryScanner scanner;
        private DirectoryXMLWriter xmlWriter;

        delegate void DrawFile(FileFoundEventArgs e);
        delegate void DrawDirectory(DirectoryFoundEventArgs e);

        public MainForm()
        {
            InitializeComponent();

            this.scanner = new DirectoryScanner();
            Thread writeToXMLThread = new Thread(new ThreadStart(() =>
            {
                this.scanner.onDirectoryFound += this.tryWriteDirectoryToXML;
                this.scanner.onFileFound += this.tryWriteFileToXML;
            }));
            Thread drawTreeThread = new Thread(new ThreadStart(() =>
            {
                this.scanner.onDirectoryFound += this.drawDirectory;
                this.scanner.onFileFound += this.drawFile;
            }));
            writeToXMLThread.Start();
            drawTreeThread.Start();

            this.Text = Thread.CurrentThread.ManagedThreadId.ToString();
        }

        private TreeNode getNodeByPath(string path)
        {
            TreeNode node = this.directoryTree.Nodes[0];
            if (! path.StartsWith(node.Text))
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
                if (! found)
                {
                    throw new Exception("Directory not found");
                }
            }

            return node;
        }

        private void drawFile(FileFoundEventArgs e)
        {
            if (this.directoryTree.InvokeRequired)
            {
                DrawFile callback = new DrawFile(this.drawFile);
                this.Invoke(callback, new object[] { e });

                return;
            }

            TreeNode fileNode = new TreeNode(e.info.Name, 0, 0);
            this.getNodeByPath(e.info.DirectoryName)
                .Nodes.Add(fileNode);
        }

        private void drawDirectory(DirectoryFoundEventArgs e)
        {
            if (this.directoryTree.InvokeRequired)
            {
                DrawDirectory callback = new DrawDirectory(this.drawDirectory);
                this.Invoke(callback, new object[] { e });

                return;
            }

            TreeNode directoryNode = new TreeNode(e.info.Name, 1, 1);
            this.getNodeByPath(e.info.Parent.FullName)
                .Nodes.Add(directoryNode);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
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

            tryStartTree();
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

            this.xmlWriter = new DirectoryXMLWriter(
                XmlWriter.Create(filePathTextBox.Text)
            );
            this.xmlWriter.Start(new DirectoryInfo(directoryTextBox.Text));

            this.directoryTree.Nodes.Clear();
            this.directoryTree.Nodes.Add(
                directoryTextBox.Text,
                directoryTextBox.Text,
                1
            );
            new Thread(new ThreadStart(() =>
            {
                this.scanner.Scan(directoryTextBox.Text);
            })).Start();
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

        private void tryWriteDirectoryToXML(DirectoryFoundEventArgs e)
        {
            // check if we selected file...
            if (this.xmlWriter == null)
            {
                return;
            }

            this.xmlWriter.WriteDirectory(e.info);
        }

        private void tryWriteFileToXML(FileFoundEventArgs e)
        {
            // check if we selected file...
            if (this.xmlWriter == null)
            {
                return;
            }

            this.xmlWriter.WriteFile(e.info);
        }
    }
}
