using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            this.writer.WriteEndElement();
        }

        public void WriteFile(FileInfo info)
        {
            this.writer.WriteStartElement("file");

            // name
            this.writer.WriteElementString("name", info.FullName);
            // created
            this.writer.WriteElementString("createdAt", info.CreationTime.ToString());

            this.writer.WriteEndElement();
        }
    }
}
