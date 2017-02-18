using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace VTree.Events
{
    public class DirectoryFoundEventArgs : EventArgs
    {
        public DirectoryInfo info { get; private set; }

        public DirectoryFoundEventArgs(DirectoryInfo info)
        {
            this.info = info;
        }
    }
}
