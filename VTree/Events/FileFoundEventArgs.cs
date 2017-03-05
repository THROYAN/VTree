using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace VTree.Events
{
    public class FileFoundEventArgs : ItemFoundEventArgs
    {
        public new FileInfo Info { get { return base.Info as FileInfo; } }

        public FileFoundEventArgs(FileInfo Info, string ErrorMessage)
            : base(Info, ErrorMessage)
        {
        }
    }
}
