using System;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;

namespace Refresh
{
    class Program
    {
        static List<string> MonitoredPaths = new List<string>()
        {
            @"C:\Users\dobromir\Downloads\",
            @"C:\Users\dobromir\Pictures\"
        };
        static void Main(string[] args)
        {
            DailyFolders df = new DailyFolders(@"C:\Users\dobromir\Desktop\", 
                @"F:\WorkBackup\", 
                new List<string>() { @"C:\Users\dobromir\Downloads\", @"C:\Users\dobromir\Pictures\" }, 
                true);

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
