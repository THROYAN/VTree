using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace VTree.Events
{
    public class ItemFoundEventArgs : EventArgs
    {
        public FileSystemInfo Info { get; private set; }

        public string ErrorMessage { get; private set; }

        public ItemFoundEventArgs(FileSystemInfo Info, string ErrorMessage = "")
        {
            this.Info = Info;
            this.ErrorMessage = ErrorMessage;
        }
    }
}
