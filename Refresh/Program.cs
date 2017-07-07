using System;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Google.Apis.Auth.OAuth2;
using System.Threading;
using Google.Apis.Calendar.v3.Data;

namespace Refresh
{
    class Program
    {
        static string YourUserName = "dobromir";
        static List<string> MonitoredPaths = new List<string>()
        {
            @"C:\Users\"+YourUserName+@"\Downloads\",
            @"C:\Users\"+YourUserName+@"\Pictures\"
            //@"C:\Users\"+YourUserName+@"\Documents\"
        };
        static void Main(string[] args)
        {
            DailyFolders df = new DailyFolders(@"C:\Users\"+ YourUserName + @"\Desktop\", @"C:\RefreshBackup\", MonitoredPaths, false);


            try
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Managing...");
                df.ManageFolders();
                Console.WriteLine("Managing Complete. (" + df.ManageChanges + " " + df.ChangeOrChanges(df.ManageChanges) + ")\n");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("Moving Files/Folders from Monitored Locations... " + "\n");
                df.MoveFromMonitored();
                Console.WriteLine("Moving Files/Folders from Monitored Locations Complete. (" + df.MoveMonitoredChanges + " " + df.ChangeOrChanges(df.MoveMonitoredChanges) + ")\n");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Archiving...");
                df.ArchiveFolders();

                Console.WriteLine("Archive Complete. (" + df.ArchiveChanges + " " + df.ChangeOrChanges(df.ArchiveChanges) + ")\n");

            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message.ToString());
                Console.Read();
            }
            finally
            {
                Console.ForegroundColor = ConsoleColor.Green;
                df.Changes = df.ManageChanges + df.MoveMonitoredChanges + df.ArchiveChanges;
                Console.WriteLine("All Done! (" + df.GetTotalChanges() + " total " + df.ChangeOrChanges(df.Changes) + ")");
                Console.Read();
            }
        }
    }
}
