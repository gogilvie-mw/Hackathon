using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon
{
    class RecordID
    {
        public int UserID {get; set;}
        public int AppID {get; set;}
        public int HttpCode {get; set;}
        public string Date {get; set;}
        public string Time {get; set;}
        public string Resource {get; set;}
        public int RequestingServerID {get; set;}
        public int RequestPageID {get; set;}
        public string ServingIPAddress { get; set; }
        public int TimeTaken { get; set; }
        public int ResourceID { get; set; }

        public RecordID()
        {
            UserID = -1;
            AppID = -1;
            HttpCode = -1;
            Date = "1/1/2000";
            Time = "55:55:55";
            Resource = "nothing";
            RequestingServerID = -1;
            RequestPageID = -1;
            ServingIPAddress = "1.1.1.1";
            TimeTaken = -1;
            ResourceID = -1;
        }

        public void Print()
        {
            Console.WriteLine();
            Console.WriteLine("UserID: " + UserID);
            Console.WriteLine("AppID: " + AppID);
            Console.WriteLine("HTTPCode: " + HttpCode);
            Console.WriteLine("Date: " + Date);
            Console.WriteLine("Time: " + Time);
            Console.WriteLine("Resource: "+ Resource);
            Console.WriteLine("RequestingServer: " + RequestingServerID);
            Console.WriteLine("RequestPage: " + RequestPageID);
            Console.WriteLine("ServerIP: " + ServingIPAddress);
            Console.WriteLine("TimeTaken: " + TimeTaken);
            Console.WriteLine("ResourceID: " + ResourceID);

        }
    }
}
