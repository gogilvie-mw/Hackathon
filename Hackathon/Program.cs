using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using System.Configuration;



namespace Hackathon
{
    class Program
    {


        static void Main(string[] args)
        {
            string [] appKeys = ConfigurationManager.AppSettings.AllKeys;
            ArrayList dirList = new ArrayList();


            foreach (string s in appKeys)  
            {
                dirList.Add(ConfigurationManager.AppSettings[s]);
            }

            IISLog psr = new IISLog();
            psr.Load(dirList);


            //p.readFile(@"C:\Visual Studio\Hackathon\Hackathon\Enterprise_IIS_Logs.txt");
            //DateTime d = p.GetDateModified(@"C:\Users\gawaineo\Desktop\Hackathon\vapp02\W3SVC1078817113\ex130723.log");
            //Console.WriteLine(d);

            //p.GetFileNamesFromDirectory(@"C:\Users\gawaineo\Desktop\Hackathon\vapp02\W3SVC1078817113");

           

            //psr.ParsingFile(@"C:\Visual Studio\Hackathon\Hackathon\Enterprise_IIS_Logs.txt");

            Console.WriteLine("\n\nPress Enter to continue...");
            Console.ReadLine();

        }
    }
}
