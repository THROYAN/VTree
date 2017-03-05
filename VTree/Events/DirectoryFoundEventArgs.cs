using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace VTree.Events
{
    public class DirectoryFoundEventArgs : ItemFoundEventArgs
    {
        public new DirectoryInfo Info { get { return base.Info as DirectoryInfo; } }

        public DirectoryFoundEventArgs(DirectoryInfo Info, string ErrorMessage)
            : base(Info, ErrorMessage)
        {
        }
    }
}
