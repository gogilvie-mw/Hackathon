using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;



namespace Hackathon
{
    class Program
    {
        private DateTime dateFileModified;

        struct IISFile
        {
            public string FileName;
            public DateTime DateModified;
        }

        //Represents all important data in line of the IIS file.
        struct RowEntry
        {
            public string ServerIP;
            public string EmployeeUserName;
            public string AppName;
            public int ResponseCode;
            public DateTime accessTime;
        }

        //Reads file line-by-line from top to bottom. Returns each line.
        private void readFile(string fileName)
        {
            try
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    String line;

                    while ((line = sr.ReadLine()) != null)
                    {
                        //Comment out the line below when not testing.
                        Console.WriteLine(line);
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

        }

        //Returns the date a file was last modified.
        private DateTime GetDateModified(String fileName)
        {
            return File.GetLastWriteTime(fileName);
        }

        //Checks if the list of files already exists in our database. Prints a list of files in a directory.
        private ArrayList GetFileNamesFromDirectory(String directoryName)
        {
            var fileList = new ArrayList();


            DirectoryInfo di = new DirectoryInfo(directoryName);

            foreach (var fi in di.GetFiles())
            {
                fileList.Add(fi);
                Console.WriteLine(fi.Name);
            }

            return fileList;
        }

        static void Main(string[] args)
        {
            Program p = new Program();

            //p.readFile(@"C:\Visual Studio\Hackathon\Hackathon\testfile.txt");
            //DateTime d = p.GetDateModified(@"C:\Users\gawaineo\Desktop\Hackathon\vapp02\W3SVC1078817113\ex130723.log");
            //Console.WriteLine(d);

            p.GetFileNamesFromDirectory(@"C:\Users\gawaineo\Desktop\Hackathon\vapp02\W3SVC1078817113");

            Console.WriteLine("\n\nPress Enter to continue...");
            Console.ReadLine();

        }
    }
}
