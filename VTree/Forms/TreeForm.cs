using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using VTree.Events;
using VTree.Models;

namespace VTree.Forms
{
    public partial class TreeForm : Form
    {
        delegate void DrawDirectory(DirectoryFoundEventArgs e);
        delegate void DrawFile(FileFoundEventArgs e);

        public char directoryDelimiter = '\\';

        public TreeForm(DirectoryScanner scanner)
        {
            InitializeComponent();
            
            scanner.onDirectoryFound += this.drawDirectory;
            scanner.onFileFound += this.drawFile;

            this.Text = Thread.CurrentThread.ManagedThreadId.ToString();
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

        public void initializeTree()
        {
            this.directoryTree.Nodes.Clear();
        }

        private void drawFile(FileFoundEventArgs e)
        {
            if (this.InvokeRequired)
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
            if (this.InvokeRequired)
            {
                DrawDirectory callback = new DrawDirectory(this.drawDirectory);
                this.Invoke(callback, new object[] { e });

                return;
            }

            Console.WriteLine("4-" + Thread.CurrentThread.ManagedThreadId);
            // root
            if (this.directoryTree.Nodes.Count == 0)
            {
                this.directoryTree.Nodes.Add(new TreeNode(e.info.FullName, 1, 1));

                return;
            }
            TreeNode directoryNode = new TreeNode(e.info.Name, 1, 1);
            this.getNodeByPath(e.info.Parent.FullName)
                .Nodes.Add(directoryNode);
        }

        private void TreeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true; // this cancels the close event.
        }
    }
}
