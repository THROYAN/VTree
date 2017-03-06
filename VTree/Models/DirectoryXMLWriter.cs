using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Xml;

namespace VTree.Models
{
    public class DirectoryXMLWriter
    {
        public XmlWriter writer;

        public DirectoryXMLWriter(XmlWriter writer)
        {
            this.writer = writer;
        }

        public void Start()
        {
            this.writer.WriteStartDocument();
            this.writer.WriteStartElement("ScanResults");
        }

        public void End()
        {
            this.writer.WriteEndElement();
            this.writer.WriteEndDocument();

            this.writer.Close();
        }

        public void WriteDirectory(DirectoryInfo info)
        {
            this.writer.WriteStartElement("directory");

            // thread id (debug)
            this.writer.WriteAttributeString(
                "threadId",
                Thread.CurrentThread.ManagedThreadId.ToString()
            );
       
            // name
            this.writer.WriteElementString("name", info.FullName);
            // created
            this.writer.WriteElementString("createdAt", info.CreationTime.ToString());
            // updated
            this.writer.WriteElementString("updatedAt", info.LastWriteTime.ToString());
            // accessed
            this.writer.WriteElementString("accessedAt", info.LastAccessTime.ToString());
            // attributes
            this.writer.WriteElementString("attributes", info.Attributes.ToString());
            // size (todo)
            // this.writer.WriteElementString("size", in);
            // owner
            this.writer.WriteElementString(
                "owner",
                getOwner(info.FullName)
            );
            // permissions for current user
            this.writer.WriteElementString(
                "permissions",
                getPermissions(info.FullName)
            );

            this.writer.WriteEndElement();
        }

        public void WriteFile(FileInfo info)
        {
            this.writer.WriteStartElement("file");

            // name
            this.writer.WriteElementString("name", info.FullName);
            // created
            this.writer.WriteElementString("createdAt", info.CreationTime.ToString());
            // updated
            this.writer.WriteElementString("updatedAt", info.LastWriteTime.ToString());
            // accessed
            this.writer.WriteElementString("accessedAt", info.LastAccessTime.ToString());
            // attributes
            this.writer.WriteElementString("attributes", info.Attributes.ToString());
            // size
            this.writer.WriteElementString("size", info.Length.ToString() + " bytes");
            // owner
            this.writer.WriteElementString(
                "owner",
                getOwner(info.FullName)
            );
            // permissions for current user
            this.writer.WriteElementString(
                "permissions",
                getPermissions(info.FullName)
            );

            this.writer.WriteEndElement();
        }

        private string getOwner(string path)
        {
            try
            {
                return
                    File.GetAccessControl(path)
                        .GetOwner(typeof(System.Security.Principal.NTAccount))
                        .ToString();
            }
            catch
            {
                // just text from Windows UI
                return "Unable to display current owner.";
            }
        }

        private string getPermissions(string path)
        {
            var allPermissions = Enum.GetValues(typeof(FileSystemRights));
            Dictionary<FileSystemRights, bool> permissions = new Dictionary<FileSystemRights, bool>();

            foreach (FileSystemRights permission in allPermissions)
            {
                permissions[permission] = false;
            }

            try
            {
                var rules = File.GetAccessControl(path).GetAccessRules(true, true, typeof(System.Security.Principal.NTAccount));

                foreach (FileSystemAccessRule fsRule in rules)
                {
                    foreach (FileSystemRights permission in allPermissions)
                    {
                        if (fsRule.FileSystemRights.HasFlag(permission))
                        {
                            permissions[permission] = fsRule.AccessControlType == AccessControlType.Allow;
                        }
                    }
                }
            }
            catch
            {
                //return "";
            }

            // hah, just get list of all allowed right as string...
            return String.Join(", ", (permissions.Where(keyValuePair =>
            {
                return keyValuePair.Value;
            }).Select<KeyValuePair<FileSystemRights, bool>, FileSystemRights>(keyValuePair =>
            {
                return keyValuePair.Key;
            })));
        }
    }
}
