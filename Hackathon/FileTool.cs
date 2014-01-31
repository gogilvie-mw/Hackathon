using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon
{
    class FileTool
    {
        
        public static DateTime GetDateModified(String fileName)
        {
            return File.GetLastWriteTime(fileName);
        }

        
        public static ArrayList GetFileNamesFromDirectory(String directoryName)
        {
            var fileList = new ArrayList();

            DirectoryInfo di = new DirectoryInfo(directoryName);

            foreach (var fi in di.GetFiles())
            {
                fileList.Add(fi);
                //Console.WriteLine(fi.Name);
            }

            return fileList;
        }

        
    }
}
