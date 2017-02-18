using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VTree.Events
{
    public class DirectoryScanEventArgs : EventArgs
    {
        public string XMLFilePath { get; private set; }
        public string DirectoryPath { get; private set; }

        public DirectoryScanEventArgs(string directoryPath, string xmlFilePath)
        {
            this.DirectoryPath = directoryPath;
            this.XMLFilePath = xmlFilePath;
        }
    }
}
