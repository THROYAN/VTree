using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
        public char directoryDelimiter = '\\';

        private DirectoryScanner scanner;

        public TreeForm(DirectoryScanner scanner)
        {
            this.scanner = scanner;

            Console.WriteLine("treeform construct-" + Thread.CurrentThread.ManagedThreadId);
            InitializeComponent();

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

        private void TreeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //this.Hide();
            //e.Cancel = true;
            this.scanner.onItemFound -= this.drawDirectory;
            this.scanner.onItemFound -= this.drawFile;
        }
    }
}
