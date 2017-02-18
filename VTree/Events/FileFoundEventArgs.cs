using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace VTree.Events
{
    public class FileFoundEventArgs : EventArgs
    {
        public FileInfo info { get; private set; }

        public FileFoundEventArgs(FileInfo info)
        {
            this.info = info;
        }
    }
}
