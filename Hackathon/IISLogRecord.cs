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

        public void PrintLogRecord()
        {
            Console.WriteLine(EmployeeUserName + " " + ApplicationName + " " + HTTPResponseCode +
                " " + accessDate + " " + accessTime + " " + ResourceName + " " + RequestingServer
                + " " + ServerIP + " " + LogDirectoryName +  " " + TimeTaken);
        }
    }
}
