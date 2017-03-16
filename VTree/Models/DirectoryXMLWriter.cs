using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Xml;

namespace VTree.Models
{
    public class DirectoryXMLWriter
    {
        /// <summary>
        /// All posible permissions
        /// </summary>
        private Array allPermissions = Enum.GetValues(typeof(FileSystemRights));
        
        private XmlWriter writer;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer">XML writer with opened file</param>
        public DirectoryXMLWriter(XmlWriter writer)
        {
            this.writer = writer;
        }

        /// <summary>
        /// Use to start writing
        /// </summary>
        public void Start()
        {
            this.writer.WriteStartDocument();
            this.writer.WriteStartElement("ScanResults");
        }

        /// <summary>
        /// Use for finalize writing
        /// </summary>
        public void End()
        {
            this.writer.WriteEndElement();
            this.writer.WriteEndDocument();

            this.writer.Close();
        }

        public void StartDirectory()
        {
            this.writer.WriteStartElement("directory");
        }

        public void EndDirectory()
        {
            this.writer.WriteEndElement();
        }

        public void StartFile()
        {
            this.writer.WriteStartElement("file");
        }

        public void EndFile()
        {
            this.writer.WriteEndElement();
        }

        public void WriteDirectory(DirectoryInfo info)
        {
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
        }

        public void WriteFile(FileInfo info)
        {
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
        }

        private string getOwner(string path)
        {
            try
            {
                return
                    File.GetAccessControl(path)
                        // NTAccount for UF name
                        .GetOwner(typeof(NTAccount))
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
            Dictionary<FileSystemRights, bool> permissions = new Dictionary<FileSystemRights, bool>();

            // start with all rules denied
            foreach (FileSystemRights permission in this.allPermissions)
            {
                permissions[permission] = false;
            }

            try
            {
                // get all security rules (with identities)
                var rules = File.GetAccessControl(path).GetAccessRules(true, true, typeof(SecurityIdentifier));
                WindowsIdentity currentUser = WindowsIdentity.GetCurrent();

                foreach (FileSystemAccessRule fsRule in rules)
                {
                    // check if current user fits the rule
                    if (! (currentUser.User == fsRule.IdentityReference || currentUser.Groups.Contains(fsRule.IdentityReference)))
                    {
                        continue;
                    }

                    // check all containing rules (later deny will replace previous allows!)
                    foreach (FileSystemRights permission in this.allPermissions)
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
