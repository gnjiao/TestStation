using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Utils
{
    public class XmlReader
    {
        private readonly XmlDocument _xml = null;
        private Logger _logger = new Logger("XmlReader");
        public XmlReader(string path)
        {
            _xml = new XmlDocument();
            _xml.Load(path);
        }
        public string GetItem(string[] nodes)
        {
            lock (_xml)
            {
                XmlNode[] xmlNodes = new XmlNode[nodes.Length];

                XmlNode temp = _xml;
                for (int i = 0; i < nodes.Length; i++)
                {
                    xmlNodes[i] = temp.SelectSingleNode(nodes[i]);
                    if (xmlNodes[i] != null)
                    {
                        temp = xmlNodes[i];
                    }
                    else
                    {
                        string output = "";
                        for (int j = 0; j <= i; j++)
                        {
                            output += nodes[j];
                            if (j < i)
                            {
                                output += "->";
                            }
                        }
                        _logger.Info("Failed to find " + output);

                        return "N/A";
                    }
                }

                return xmlNodes[xmlNodes.Length - 1].InnerText;
            }
        }
    }
    public class XmlSerializer
    {
        private static Dictionary<string, object> _fileLock = new Dictionary<string, object>();
        public static void Save<T>(string file, T subject)
        {
            if (!_fileLock.ContainsKey(file))
            {
                _fileLock[file] = new object();
            }

            lock (_fileLock[file])
            {
                new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None).Close();
                using (var fs = new FileStream(file, FileMode.Truncate, FileAccess.Write, FileShare.None))
                {
                    System.Xml.Serialization.XmlSerializer s = new System.Xml.Serialization.XmlSerializer(typeof(T));
                    s.Serialize(fs, subject);
                }
            }
        }
        public static void Load<T>(string file, out T subject)
        {
            if (File.Exists(file))
            {
                using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    System.Xml.Serialization.XmlSerializer s = new System.Xml.Serialization.XmlSerializer(typeof(T));
                    subject = (T)s.Deserialize(fs);
                }
            }
            else
            {
                subject = default(T);
            }
        }
    }
}