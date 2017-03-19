using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace VTree.Models
{
    /// <summary>
    /// Class helper to collect a deep collection of directories
    /// </summary>
    public class DirectoryTree
    {
        private Dictionary<string, DirectoryModel> directoryList;

        public DirectoryTree()
        {
            this.directoryList = new Dictionary<string, DirectoryModel>();
        }

        public DirectoryModel GetRoot()
        {
            return this.directoryList.Count > 0
                    ? this.directoryList.Values.ElementAt(0) // will it work?
                    : null;

        }

        public void AddDirectory(DirectoryInfo directory)
        {
            DirectoryModel parent = null;
            if (this.directoryList.ContainsKey(directory.Parent.FullName))
            {
                parent = this.directoryList[directory.Parent.FullName];
            }

            if (parent == null && this.directoryList.Count != 0)
            {
                // looks like a problem
                Console.WriteLine("[WARNING] There is no parent for " + directory.FullName);
            }

            this.directoryList[directory.FullName] = new DirectoryModel(directory, parent);
            if (parent != null)
            {
                parent.Directories.Add(this.directoryList[directory.FullName]);
            }
        }

        public void AddFile(FileInfo file)
        {
            long size = file.Length;
            DirectoryModel directory = this.directoryList[file.DirectoryName];

            directory.Files.Add(file);

            // update sizes for all directories
            while (directory != null)
            {
                directory.Size += size;

                directory = directory.Parent;
            }
        }

        private DirectoryModel getDirectory(string path)
        {
            return this.directoryList[path];
        }
    }
}
