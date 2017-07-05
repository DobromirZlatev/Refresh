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
        static List<string> MonitoredPaths = new List<string>()
        {
            @"C:\Users\dobromir\Downloads\",
            @"C:\Users\dobromir\Pictures\",
            @"C:\Users\dobromir\Documents\"
        };
        static void Main(string[] args)
        {
            DailyFolders df = new DailyFolders(@"C:\Users\dobromir\Desktop\",
                @"F:\WorkBackup\",
                MonitoredPaths,
                true);

            //try
            //{
            //    UserCredential credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
            //    new ClientSecrets
            //    {
            //        ClientId = "241811363389-gkb4q30v85brccom3sur5spaap24scm4.apps.googleusercontent.com",
            //        ClientSecret = "Kn9cWTMBTyFr7u7aQ6dRAALB",
            //    },
            //    new[] { CalendarService.Scope.Calendar },
            //    "DobromirZlatev",
            //    CancellationToken.None).Result;
            //    var service = new CalendarService(new BaseClientService.Initializer()
            //    {
            //        HttpClientInitializer = credential,
            //        ApplicationName = "CalendarQuickstart",
            //    });
            //    Event myEvent = new Event
            //    {
            //        Summary = "Appointment",
            //        Location = "Somewhere",
            //        Start = new EventDateTime()
            //        {
            //            DateTime = DateTime.Now.AddHours(1),
            //            TimeZone = "Europe/London"
            //        },
            //        End = new EventDateTime()
            //        {
            //            DateTime = DateTime.Now.AddHours(2),
            //            TimeZone = "Europe/London"
            //        },
            //        Recurrence = new String[] { "RRULE:FREQ=WEEKLY;BYDAY=MO" },
            //        Attendees = new List<EventAttendee>()
            //        {
            //            new EventAttendee() { Email = "dobromirzlatessv@gmail.com" }
            //        }
            //    };

            //    Event recurringEvent = service.Events.Insert(myEvent, "primary").Execute();
            //}
            //catch(Exception ex)
            //{
            //    Console.WriteLine(ex);
            //}

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
