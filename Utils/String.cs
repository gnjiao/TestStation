using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Utils
{
    public class String
    {
        public static string FilePostfix(string fullpath, string postfix)
        {
            string path = Path.GetDirectoryName(fullpath);
            string file = Path.GetFileNameWithoutExtension(fullpath);
            string ext = Path.GetExtension(fullpath);
            return path + @"\" + file + postfix + ext;
        }
    }
}