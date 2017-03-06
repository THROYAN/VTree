using System;
using System.IO;
using System.Threading;
using VTree.Events;

namespace VTree.Models
{
    public class DirectoryScanner
    {
        public event EventHandler<ItemFoundEventArgs> onItemFound;

        public DirectoryScanner()
        {
        }

        /// <summary>
        /// Scans directory recursively
        /// </summary>
        /// <param name="path">path to scan</param>
        public void Scan(string path)
        {
            this.onItemFound?.Invoke(this, new ItemFoundEventArgs(
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
            catch (Exception)
            {
                // should not be
                //this.onItemFound?.Invoke(this, new ItemFoundEventArgs(,e));

                return;
            }

            foreach (string file in Directory.GetFiles(path))
            {
                this.onItemFound?.Invoke(this, new ItemFoundEventArgs(
                    new FileInfo(file)
                ));
            }

            string[] directories = Directory.GetDirectories(path);

            // two cycles to show all directories first
            foreach (string dir in directories)
            {


                // to call the event...
                this.onItemFound?.Invoke(this, new ItemFoundEventArgs(
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
