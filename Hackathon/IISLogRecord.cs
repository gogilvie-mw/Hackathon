using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon
{
    class IISLogRecord
    {
        public string ServerIP {get; set;}
        public string EmployeeUserName { get; set; }
        public string ApplicationName { get; set; }
        public int HTTPResponseCode { get; set; }
        public string accessDate { get; set; }
        public string accessTime { get; set; }
        public string LogDirectoryName{get; set;}
        public string ResourceName { get; set; }
        public string RequestingServer { get; set; }
        public string TimeTaken { get; set; }
        public string RequestingPage { get; set; }

        public void PrintLogRecord()
        {
            Console.WriteLine();
            Console.WriteLine("ServerIP: " + ServerIP);
            Console.WriteLine("EmployeeUserName: " + EmployeeUserName);
            Console.WriteLine("ApplicationName: " + ApplicationName);
            Console.WriteLine("HTTPResponseCode: " + HTTPResponseCode);
            Console.WriteLine("accessDate: " + accessDate);
            Console.WriteLine("accessTime: " + accessTime);
            Console.WriteLine("LogDirectoryName: " + LogDirectoryName);
            Console.WriteLine("ResourceName: " + ResourceName);
            Console.WriteLine("RequestingServer: " + RequestingServer);
            Console.WriteLine("TimeTaken: " + TimeTaken);
            Console.WriteLine("RequestingPage: " + RequestingPage);

        }
    }
}
