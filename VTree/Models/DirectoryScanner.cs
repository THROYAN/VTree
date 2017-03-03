using System;
using System.IO;
using System.Threading;
using VTree.Events;

namespace VTree.Models
{
    public class DirectoryScanner
    {
        public event EventHandler<FileFoundEventArgs> onFileFound;
        public event EventHandler<DirectoryFoundEventArgs> onDirectoryFound;
        public event EventHandler<ErrorEventArgs> onError;

        public DirectoryScanner()
        {
        }

        /// <summary>
        /// Scans directory recursively
        /// </summary>
        /// <param name="path">path to scan</param>
        public void Scan(string path)
        {
            this.onDirectoryFound?.Invoke(this, new DirectoryFoundEventArgs(
                new DirectoryInfo(path)
            ));

            this.scan(path);
        }

        private void scan(string path)
        {
            string[] files;
            try
            {
                files = Directory.GetFiles(path);
            }
            catch (Exception e)
            {
                this.onError?.Invoke(this, new ErrorEventArgs(e));

                return;
            }

            foreach (string file in Directory.GetFiles(path))
            {
                this.onFileFound?.Invoke(this, new FileFoundEventArgs(
                    new FileInfo(file)
                ));
            }

            string[] directories = Directory.GetDirectories(path);

            // two cycles to show all directories first
            foreach (string dir in directories)
            {


                // to call the event...
                this.onDirectoryFound?.Invoke(this, new DirectoryFoundEventArgs(
                    new DirectoryInfo(dir)
                ));
            }
            // search forward
            foreach (string dir in directories)
            {
                this.scan(dir);
            }
        }
    }
}
