using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;

namespace VTree.Models
{
    class DirectoryXMLWriter
    {
        public XmlWriter writer;

        public DirectoryXMLWriter(XmlWriter writer)
        {
            this.writer = writer;
        }

        public void Start(DirectoryInfo info)
        {
            this.writer.WriteStartElement("ScanResults");
            this.WriteDirectory(info);
        }

        public void End()
        {
            this.writer.WriteEndElement();
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
            this.writer.WriteStartElement("name");
            this.writer.WriteString(info.FullName);
            this.writer.WriteEndElement();
            // created
            this.writer.WriteStartElement("createdAt");
            this.writer.WriteString(info.CreationTime.ToString());
            this.writer.WriteEndElement();

            this.writer.WriteEndElement();
        }

        public void WriteFile(FileInfo info)
        {
            this.writer.WriteStartElement("file");

            // name
            this.writer.WriteStartElement("name");
            this.writer.WriteString(info.FullName);
            this.writer.WriteEndElement();
            // created
            this.writer.WriteStartElement("createdAt");
            this.writer.WriteString(info.CreationTime.ToString());
            this.writer.WriteEndElement();

            this.writer.WriteEndElement();
        }
    }
}
