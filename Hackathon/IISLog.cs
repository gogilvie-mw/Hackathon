using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Reflection;

namespace Hackathon
{
    class IISLog
    {
        private string dirName;
        public string FileName = "";
        public DateTime DateFileModified = Convert.ToDateTime("1/1/1920");
        
        private string regexPattern = @"^(\d{4}\-\d{2}\-\d{2})\s(\d{2}:\d{2}:\d{2})\s(\w+)\s((\d{1,3}\.){3}\d{1,3})[\w+,\s]*((\/\w+){1,}\.\w{2,4})((\s[-,\S]){1,}.*)";

        private string connectionString = ConfigurationManager.ConnectionStrings["mwPSR"].ConnectionString;

        #region Data Structures
        private List<IISLogRecord> logRecords = new List<IISLogRecord>();
        private Dictionary<string, int> usersKeyTable = new Dictionary<string, int>();
        private Dictionary<string, int> applicationKeyTable = new Dictionary<string, int>();
        private Dictionary<string, int> resourceKeyTable = new Dictionary<string, int>();
        #endregion

        #region Constructors
        public IISLog()
        {
            FillKeyTables();
        }

        public IISLog(string directoryName)
        {
            dirName = directoryName;
        }
        #endregion

        public void Load(ArrayList dirs)
        {
            ArrayList files = new ArrayList();
            foreach (string dir in dirs)
            {
                files = FileTool.GetFileNamesFromDirectory(dir);
                foreach (FileInfo file in files)
                {
                    string fullFileName = file.Directory + @"\" + file.Name;
                    ParsingFile(fullFileName);
                }
            }

            AddToMasterLogTable();

            logRecords.Clear();
            usersKeyTable.Clear();
            resourceKeyTable.Clear();
            applicationKeyTable.Clear();

        }

        #region Data Base Related functions
        public void AddToMasterLogTable()
        {
            try
            {
                DataTable masterTable = ConvertListToDataTable(ProcessListToIDs());
                    //ListToDataTable(ProcessListToIDs());
                //printData(masterTable);
                
                //printDataTable();

                try
                {
                    using (SqlBulkCopy sbc = new SqlBulkCopy(connectionString,SqlBulkCopyOptions.KeepIdentity))
                    {
                         
                        sbc.BatchSize = 10000;
                        sbc.NotifyAfter = 1000;

                        sbc.ColumnMappings.Add("UserID", "userID");
                        sbc.ColumnMappings.Add("AppID", "appID");
                        sbc.ColumnMappings.Add("Date", "Date");
                        sbc.ColumnMappings.Add("Time", "Time");
                        sbc.ColumnMappings.Add("ServingIPAddress", "Serving_ServerIP");
                        sbc.ColumnMappings.Add("TimeTaken", "timetaken");
                        sbc.ColumnMappings.Add("ResourceID", "resourceID");
                        sbc.ColumnMappings.Add("HttpCode", "HTTPResponseCode");
                        sbc.ColumnMappings.Add("Resource", "Requested_Resource");
                        sbc.ColumnMappings.Add("RequestingServerID", "request_ServerID");


                        sbc.DestinationTableName = "dbo.MasterLog";
                        sbc.WriteToServer(masterTable);
                    }
                }
                catch(Exception e){
                    Console.WriteLine("Bulk Insert Failed: " + e.Message + "\n " + e.StackTrace);
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine("Bulk Insert of IIS Log Failed\n\n" + e.StackTrace);
                Console.WriteLine(e.Message);
            }

        }

        private void MapColumns(DataTable infoTable, SqlBulkCopy bulkCopy)
        {

            foreach (DataColumn dc in infoTable.Columns)
            {
                bulkCopy.ColumnMappings.Add(dc.ColumnName,
                  dc.ColumnName);
                Console.WriteLine(dc.ColumnName + " " + dc.ColumnMapping);
            }
        }

        public void printDataTable(DataTable d)
        {
            DataTable table = d; // Get the data table.

            foreach (DataRow row in table.Rows) // Loop over the rows.
            {
                Console.WriteLine("--- Row ---"); // Print separator.
                foreach (var item in row.ItemArray) // Loop over the items.
                {
                    Console.Write("Item: "); // Print label.
                    Console.WriteLine(item); // Invokes ToString abstract method.
                }
            }
            //Console.Read(); // Pause.


        }

        private void printDataTable()
        {
            List<RecordID> p = ProcessListToIDs();

            foreach (RecordID ri in p)
            {
                ri.Print();
            }
        }

        private DataTable ListToDataTable(List<RecordID> list)
        {
            DataTable dt = CollectionHelper.ConvertTo<RecordID>(list);

            foreach(DataRow row in dt.Rows)
            {
                foreach(DataColumn column in dt.Columns)
                {
                    object value = row[column.ColumnName];
                }
            }

            return dt;
        }

        private DataTable ConvertListToDataTable(List<RecordID> list)
        {
            DataTable dt = new DataTable(typeof(RecordID).Name);

            //Get all properties
            PropertyInfo[] Props = typeof(RecordID).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach(PropertyInfo prop in Props)
            {
                dt.Columns.Add(prop.Name);
            }

            foreach (var recordID in list)
            {
                Object [] values = new Object[Props.Length];

                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(recordID);
                }
                dt.Rows.Add(values);
            }

            return dt;
        }

        private DataTable FillDataTables(string query)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlDataAdapter dap = new SqlDataAdapter(query, connectionString);
                DataTable dt = new DataTable();

                dap.Fill(dt); 

                return dt;
                
            }

        }

        private void FillKeyTables()
        {
            DataTable user = FillDataTables(@"SELECT [UserID], [UserName] FROM [dbo].[User]");
            DataTable resource = FillDataTables(@"SELECT [ResourceID],[ResourceName] FROM [dbo].[Resource]");
            DataTable application = FillDataTables(@"SELECT [AppID], [Name] FROM [dbo].[Application]");

            foreach (DataRow dr in  user.Rows)
            {
                int value = Convert.ToInt32(dr["UserID"]);
                string key = Convert.ToString(dr["UserName"]);

                usersKeyTable.Add(key, value);
            }

            foreach (DataRow dr in resource.Rows)
            {
                int value = Convert.ToInt32(dr["ResourceID"]);
                string key = Convert.ToString(dr["ResourceName"]);

                resourceKeyTable.Add(key, value);
            }


            foreach (DataRow dr in application.Rows)
            {
                int value = Convert.ToInt32(dr["AppID"]);
                string key = Convert.ToString(dr["Name"]);

                applicationKeyTable.Add(key, value);
            }


        }
        #endregion



        private List<RecordID> ProcessListToIDs()
        {

            List<RecordID> idList = new List<RecordID>();

            foreach (var record in logRecords)
            {
                RecordID r = new RecordID();

                if (!String.IsNullOrEmpty(record.EmployeeUserName) && usersKeyTable.ContainsKey(record.EmployeeUserName))
                {
                    r.UserID = usersKeyTable[record.EmployeeUserName];
                }

                if (!String.IsNullOrEmpty(record.ResourceName) && resourceKeyTable.ContainsKey(record.ResourceName))
                {
                    r.ResourceID = resourceKeyTable[record.ResourceName];
                }

                if (!String.IsNullOrEmpty(record.ApplicationName) && applicationKeyTable.ContainsKey(record.ApplicationName))
                {
                    r.AppID = applicationKeyTable[record.ApplicationName];
                }

                if (!String.IsNullOrEmpty(record.RequestingServer) && record.RequestingServer.Equals(Convert.ToString("mw-vapp01")))
                {
                    r.RequestingServerID = 1;
                }
                else
                    if (!String.IsNullOrEmpty(record.RequestingServer) && record.RequestingServer.Equals(Convert.ToString("mw-vapp02")))
                    {
                        r.RequestingServerID = 2;
                    }

                if(!String.IsNullOrEmpty(record.ResourceName))
                {
                    r.Resource = record.ResourceName;
                }

                if (!String.IsNullOrEmpty(record.accessDate))
                {
                    r.Date = record.accessDate;
                }

                if (!String.IsNullOrEmpty(record.accessTime))
                {
                    r.Time = record.accessTime;
                }

                if (!String.IsNullOrEmpty(record.ServerIP))
                {
                    r.ServingIPAddress = record.ServerIP;
                }

                r.HttpCode = record.HTTPResponseCode;

                idList.Add(r);

                //r.Print();

            }


            return idList;

        }


        #region Parsing function
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
                        if (count > 5
                            )
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

                                    Match m3 = Regex.Match(m2.Groups[4].Value, @"^\shttp\w?:\/\/(\S+):\d*\/(\w+)(\S+\.\w{2,4})\S+\s(\d+)(\s(\S+)){1,}");

                                    if (m3.Success)
                                    {
                                        logRecord.RequestingServer = m3.Groups[1].Value;
                                        logRecord.ApplicationName = m3.Groups[2].Value;
                                        logRecord.RequestingPage = m3.Groups[3].Value;
                                        logRecord.HTTPResponseCode = Int32.Parse(m3.Groups[4].Value);
                                        logRecord.TimeTaken = m3.Groups[5].Value;
                                    }
                                    else
                                    {
                                       // Console.WriteLine("Third Match did not work!");
                                    }

                                }
                                else
                                {
                                   // Console.WriteLine("Second Match did not work!");
                                }

                                //Add the Record to the List.
                                logRecords.Add(logRecord);
                               // logRecord.PrintLogRecord();
                            }

                        }
                        else
                        {
                           // Console.WriteLine("Line did not match!");
                        }
                    }
                   // logRecords.Clear();
                }
            }
            catch (Exception e)
            {
                //Console.WriteLine("The file could not be read:");
                //Console.WriteLine(e.Message);
            }
        }
        #endregion
    }

}
