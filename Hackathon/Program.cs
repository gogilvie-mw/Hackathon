using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using System.Configuration;
using System.Text.RegularExpressions;


namespace Hackathon
{
    class Program
    {

        //Reads file line-by-line from top to bottom. Returns each line.
        private void readFile(string fileName)
        {
            try
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    String line;
                    int count = 0;

                    while ((line = sr.ReadLine()) != null)
                    {
                        if (count > 50)
                            break;

                        count++;

                        Match safeLine = Regex.Match(line, @"^(\d{4}\-\d{2}\-\d{2})");

                        if (safeLine.Success)
                        {

                            Match m = Regex.Match(line, @"^(\d{4}\-\d{2}\-\d{2})\s(\d{2}:\d{2}:\d{2})\s(\w+)\s((\d{1,3}\.){3}\d{1,3})[\w+,\s]*((\/\w+){1,}\.\w{2,4})((\s[-,\S]){1,}.*)");

                            if (m.Success)
                            {  
                                string date = m.Groups[1].Value;
                                string time = m.Groups[2].Value;
                                string dir = m.Groups[3].Value;
                                string serverIP = m.Groups[4].Value;
                                string res = m.Groups[6].Value;
                                Console.WriteLine(date + " " + time + " " + dir + " " + serverIP + " " + res);
                            }

                        }
                        else
                        {
                            Console.WriteLine("Line did not match!");
                        }
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
        //private DateTime GetDateModified(String fileName)
        //{
        //    return File.GetLastWriteTime(fileName);
        //}

        ////Checks if the list of files already exists in our database. Prints a list of files in a directory.
        //private ArrayList GetFileNamesFromDirectory(String directoryName)
        //{
        //    var fileList = new ArrayList();

        //    DirectoryInfo di = new DirectoryInfo(directoryName); 

        //    foreach (var fi in di.GetFiles())
        //    {
        //        fileList.Add(fi);
        //        Console.WriteLine(fi.Name);
        //    }

        //    return fileList;
        //}

        static void Main(string[] args)
        {
            string [] appKeys = ConfigurationManager.AppSettings.AllKeys;
 
            //Program p = new Program();

            //foreach (string s in appKeys)   //int i = 0; i < appKeys.Length; i++)
            //{
            //    Console.Write("Key: " + s); 
            //    p.GetFileNamesFromDirectory(ConfigurationManager.AppSettings[s]);
            //}

          

            //p.readFile(@"C:\Visual Studio\Hackathon\Hackathon\Enterprise_IIS_Logs.txt");
            //DateTime d = p.GetDateModified(@"C:\Users\gawaineo\Desktop\Hackathon\vapp02\W3SVC1078817113\ex130723.log");
            //Console.WriteLine(d);

            //p.GetFileNamesFromDirectory(@"C:\Users\gawaineo\Desktop\Hackathon\vapp02\W3SVC1078817113");

            IISLog psr = new IISLog();
            psr.ParsingFile(@"C:\Visual Studio\Hackathon\Hackathon\Enterprise_IIS_Logs.txt");

            Console.WriteLine("\n\nPress Enter to continue...");
            Console.ReadLine();

        }
    }
}
