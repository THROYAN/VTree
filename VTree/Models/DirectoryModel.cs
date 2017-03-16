using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace VTree.Models
{
    /// <summary>
    /// Just to avoid conflict with System.IO
    /// </summary>
    public class DirectoryModel
    {
        public DirectoryInfo Info { get; private set; }
        public List<DirectoryModel> Directories { get; private set; }
        public List<FileInfo> Files { get; private set; }
        public long Size { get; set; }
        public DirectoryModel Parent { get; private set; }

        public DirectoryModel(DirectoryInfo info, DirectoryModel parent = null)
        {
            this.Info = info;
            this.Directories = new List<DirectoryModel>();
            this.Files = new List<FileInfo>();
            this.Size = 0;
            this.Parent = parent;
        }
    }
}
