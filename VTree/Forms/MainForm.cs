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
        private DirectoryScanner scanner { get; set; }

        private Thread scanThread;

        public delegate void DirectoryScanEventHander(DirectoryScanEventArgs e);

        public event DirectoryScanEventHander onStart;
        public event DirectoryScanEventHander onFinish;

        public MainForm(DirectoryScanner scanner, string directory, string xmlPath)
        {
            InitializeComponent();

            this.scanner = scanner;
            this.directoryTextBox.Text = directory;
            this.filePathTextBox.Text = xmlPath;

            this.scanThread = new Thread((object path) =>
            {
                Console.WriteLine("start scanning-" + Thread.CurrentThread.ManagedThreadId);

                DirectoryScanEventArgs e = new DirectoryScanEventArgs(
                    directoryTextBox.Text,
                    filePathTextBox.Text
                );

                this.onStart?.Invoke(e);

                try
                {
                    this.scanner.Scan(path.ToString());
                }
                finally
                {
                    this.onFinish?.Invoke(e);
                }
            });
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

            this.scanThread.Start(directoryTextBox.Text);
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
    }
}
