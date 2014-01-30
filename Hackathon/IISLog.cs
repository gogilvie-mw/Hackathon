using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Hackathon
{
    class IISLog
    {
        private string dirName;
        public string FileName = "";
        public DateTime DateFileModified = Convert.ToDateTime("1/1/1920");
        

        private List<IISLogRecord> records = new List<IISLogRecord>();
        private string regexPattern = @"^(\d{4}\-\d{2}\-\d{2})\s(\d{2}:\d{2}:\d{2})\s(\w+)\s((\d{1,3}\.){3}\d{1,3})[\w+,\s]*((\/\w+){1,}\.\w{2,4})((\s[-,\S]){1,}.*)";

        public IISLog()
        {

        }

        public IISLog(string directoryName)
        {
            dirName = directoryName;
        }

        public void Load(ArrayList dirs)
        {
            ArrayList files = new ArrayList();
            foreach (string dir in dirs)
            {
                files = FileTool.GetFileNamesFromDirectory(dir);
                foreach (FileInfo file in files)
                {
                    string s = file.Directory + @"\" + file.Name;
                    ParsingFile(s);
                }
            }
        }



        public void ParsingFile(string fileName)
        {
            try
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    String line;
                    int count = 0;

                    while ((line = sr.ReadLine()) != null)
                    {
                        if (count > 15)
                            break;

                        count++;

                        Match safeLine = Regex.Match(line, @"^(\d{4}\-\d{2}\-\d{2})");

                        if (safeLine.Success)
                        {
                            IISLogRecord logRecord = new IISLogRecord();

                            Match m = Regex.Match(line, regexPattern);

                            if (m.Success)
                            {
                                logRecord.accessDate = m.Groups[1].Value;
                                logRecord.accessTime = m.Groups[2].Value;
                                logRecord.ServerIP = m.Groups[4].Value;
                                logRecord.LogDirectoryName = m.Groups[3].Value;
                                logRecord.ResourceName = m.Groups[6].Value;

                                Match m2 = Regex.Match(m.Groups[8].Value, @"^\s([-,\S]+)\s\d+\s([-,\S]+)\s(\d{1,3}\.){3}\d{1,3}(.*)");

                                if (m2.Success)
                                {
                                    logRecord.EmployeeUserName = m2.Groups[2].Value;

                                    Match m3 = Regex.Match(m2.Groups[4].Value, @"^\shttp\w?:\/\/(\S+):\d*\/(\w+)\S+\s(\d+)(\s(\S+)){1,}");

                                    if (m3.Success)
                                    {
                                        logRecord.RequestingServer = m3.Groups[1].Value;
                                        logRecord.ApplicationName = m3.Groups[2].Value;
                                        logRecord.HTTPResponseCode = Int32.Parse(m3.Groups[3].Value);
                                        logRecord.TimeTaken = m3.Groups[4].Value;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Third Match did not work!");
                                    }

                                }
                                else
                                {
                                    Console.WriteLine("Second Match did not work!");
                                }

                                //Add the Record to the List.
                                records.Add(logRecord);
                                logRecord.PrintLogRecord();
                            }

                        }
                        else
                        {
                            Console.WriteLine("Line did not match!");
                        }
                    }
                    records.Clear();

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }
    }

}
