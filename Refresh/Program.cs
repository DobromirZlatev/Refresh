using System;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;

namespace Refresh
{
    class Program
    {
        static string InitialPath = @"C:\Users\dobromir\Desktop\";
        static string OneDrivePath = @"C:\Users\dobromir\WorkBackup\";
        static List<string> MonitoredPaths = new List<string>()
        {
            @"C:\Users\dobromir\Downloads\",
            @"C:\Users\dobromir\Pictures\"
        };
        static int manageChanges = 0;
        static int moveMonitoredChanges = 0;
        static int archiveChanges = 0;
        static int changes = 0;
        static void Main(string[] args)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Yellow;

                Console.WriteLine("Managing...");
                ManageFolders();
                Console.WriteLine("Managing Complete. (" + manageChanges + " " + GetChange_s(manageChanges) + ")\n");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("Moving Files/Folders from Monitored Locations... " + "\n");
                MoveFromMonitored();
                Console.WriteLine("Moving Files/Folders from Monitored Locations Complete. (" + moveMonitoredChanges + " " + GetChange_s(moveMonitoredChanges) + ")\n");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Archiving...");
                ArchiveFolders();
                Console.WriteLine("Archive Complete. (" + archiveChanges + " " + GetChange_s(archiveChanges) + ")\n");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.ToString());
                Console.Read();
            }
            finally
            {
                Console.ForegroundColor = ConsoleColor.Green;
                changes = manageChanges + moveMonitoredChanges + archiveChanges;
                Console.WriteLine("All Done! (" + GetTotalChanges() + " total " + GetChange_s(changes) + ")");
                Console.Read();
            }
        }

        static int GetTotalChanges()
        {
            return manageChanges + moveMonitoredChanges + archiveChanges;
        }

        static string GetChange_s(int numberOfChanges)
        {
            if (numberOfChanges == 1)
            {
                return "change";
            }
            else
            {
                return "changes";
            }
        }
        static string GetDayPath(DateTime date)
        {
            return InitialPath + GetDayName(date);
        }
        static string GetMonthPath(DateTime date)
        {
            return InitialPath + GetMonthName(date);
        }
        static string GetYearPath(DateTime date)
        {
            return InitialPath + GetYearName(date);
        }
        static string GetDayName(DateTime date)
        {
            var result = date.Day.ToString();
            if (result.Length == 1)
            {
                result = "0" + result;
            }
            return result;
        }
        static string GetMonthName(DateTime date)
        {
            switch (date.Month)
            {
                case 1:
                    return "January";
                case 2:
                    return "February";
                case 3:
                    return "March";
                case 4:
                    return "April";
                case 5:
                    return "May";
                case 6:
                    return "June";
                case 7:
                    return "July";
                case 8:
                    return "August";
                case 9:
                    return "September";
                case 10:
                    return "October";
                case 11:
                    return "November";
                case 12:
                    return "December";
                default:
                    return "Error";
            }
        }
        static string GetYearName(DateTime date)
        {
            return date.Year.ToString();
        }
        static void ArchiveFolders()
        {
            for (int i = 0; i < 10; i++)
            {
                string file = GetYearPath(DateTime.Now.AddYears(-i));
                string destFile = Path.Combine(OneDrivePath, GetYearName(DateTime.Now.AddYears(-i)));
                //Copy year folders
                if (Directory.Exists(file))
                {
                    DirectoryInfo fileInfo = FileSystem.GetDirectoryInfo(file);
                    DirectoryInfo destFileInfo = FileSystem.GetDirectoryInfo(destFile);
                    if (FileSystem.DirectoryExists(destFile))
                    {
                        if (fileInfo.LastWriteTime > destFileInfo.LastWriteTime)
                        {
                            FileSystem.DeleteDirectory(destFile, DeleteDirectoryOption.DeleteAllContents);
                            FileSystem.CopyDirectory(file, destFile);
                            archiveChanges++;
                            Console.WriteLine("[Backup Folder Updated] \n" + destFile);
                        }
                    }
                    else
                    {
                        FileSystem.CopyDirectory(file, destFile);
                        archiveChanges++;
                        Console.WriteLine("[Backup Folder Created] \n " + destFile);
                    }
                }
            }

            //Copy this month folder
            if (Directory.Exists(GetMonthPath(DateTime.Now)))
            {
                string file = GetMonthPath(DateTime.Now);
                string destFile = Path.Combine(OneDrivePath, GetMonthName(DateTime.Now));
                DirectoryInfo fileInfo = FileSystem.GetDirectoryInfo(file);
                DirectoryInfo destFileInfo = FileSystem.GetDirectoryInfo(destFile);
                if (FileSystem.DirectoryExists(destFile))
                {
                    if (fileInfo.LastWriteTime > destFileInfo.LastWriteTime)
                    {
                        FileSystem.DeleteDirectory(destFile, DeleteDirectoryOption.DeleteAllContents);
                        //FileSystem.CopyDirectory(file, destFile);
                        FileSystem.CopyDirectory(file, destFile);
                        archiveChanges++;
                        Console.WriteLine("[Backup Folder Updated] \n " + destFile);
                    }
                    else
                    {
                    }
                }
                else if (FileSystem.DirectoryExists(file))
                {
                    FileSystem.CopyDirectory(file, destFile);
                    archiveChanges++;
                    Console.WriteLine("[Backup Folder Created] \n " + destFile);
                }
            }

            //Copy todays folder to OneDrive's month folder.
            if (Directory.Exists(GetDayPath(DateTime.Now)))
            {
                string file = GetDayPath(DateTime.Now);
                string destFile = Path.Combine(OneDrivePath, GetMonthName(DateTime.Now), GetDayName(DateTime.Now));
                DirectoryInfo fileInfo = FileSystem.GetDirectoryInfo(file);
                DirectoryInfo destFileInfo = FileSystem.GetDirectoryInfo(destFile);
                if (FileSystem.DirectoryExists(destFile))
                {
                    if (fileInfo.LastWriteTime > destFileInfo.LastWriteTime)
                    {
                        FileSystem.DeleteDirectory(destFile, DeleteDirectoryOption.DeleteAllContents);
                        FileSystem.CopyDirectory(file, destFile);
                        archiveChanges++;
                        Console.WriteLine("[Backup Folder Updated] \n " + destFile);
                    }
                }
                else if (FileSystem.DirectoryExists(file))
                {
                    FileSystem.CopyDirectory(file, destFile);
                    archiveChanges++;
                    Console.WriteLine("[Backup Folder Created] \n " + destFile);
                }
            }

        }
        static void ManageFolders()
        {
            for (int i = 1; i <= 10; i++)
            {
                //moves old day folders
                if (Directory.Exists(GetDayPath(DateTime.Now.AddDays(-i))))
                {
                    if (Directory.Exists(GetMonthPath(DateTime.Now.AddDays(-i)) + "\\" + GetDayName(DateTime.Now.AddDays(-i))))
                    {
                        throw new Exception("\n\nDay " + GetDayName(DateTime.Now.AddDays(-i)) + " exists both in \n" + InitialPath + "\n and in \n" + GetMonthPath(DateTime.Now.AddDays(-i)) + " . Please review manually.\n\n");
                    }
                    Directory.Move(GetDayPath(DateTime.Now.AddDays(-i)), GetMonthPath(DateTime.Now.AddDays(-i)) + "\\" + GetDayName(DateTime.Now.AddDays(-i)));
                    manageChanges++;
                    Console.WriteLine("[Folder Moved] \n " + GetDayPath(DateTime.Now.AddDays(-i)) + " -> " + GetMonthPath(DateTime.Now.AddDays(-i)) + "\\" + GetDayName(DateTime.Now.AddDays(-i)));
                }
            }
            for (int i = 1; i <= 10; i++)
            {
                //moves old month folders
                if (Directory.Exists(GetMonthPath(DateTime.Now.AddMonths(-i))))
                {
                    if (Directory.Exists(GetYearPath(DateTime.Now.AddMonths(-i)) + "\\" + GetMonthName(DateTime.Now.AddMonths(-i))))
                    {
                        throw new Exception("\n\nMonth " + GetMonthName(DateTime.Now.AddMonths(-i)) + " exists both in \n" + InitialPath + "\n and in \n" + GetYearPath(DateTime.Now.AddMonths(-i)) + " . Please review manually.\n\n");
                    }
                    Directory.Move(GetMonthPath(DateTime.Now.AddMonths(-i)), GetYearPath(DateTime.Now.AddMonths(-i)) + "\\" + GetMonthName(DateTime.Now.AddMonths(-i)));
                    manageChanges++;
                    Console.WriteLine("[Folder Moved] \n " + GetMonthPath(DateTime.Now.AddMonths(-i)) + " -> " + GetYearPath(DateTime.Now.AddMonths(-i)) + "\\" + GetMonthName(DateTime.Now.AddMonths(-i)));
                }
            }


            //create current year
            if (!Directory.Exists(GetYearPath(DateTime.Now)))
            {
                Directory.CreateDirectory(GetYearPath(DateTime.Now));
                manageChanges++;
                Console.WriteLine("[Folder Created] \n " + GetYearPath(DateTime.Now));
            }

            //create current month
            if (!Directory.Exists(GetMonthPath(DateTime.Now)))
            {
                if (Directory.Exists(GetYearPath(DateTime.Now) + "\\" + GetMonthName(DateTime.Now)))
                {
                    Directory.Move(GetYearPath(DateTime.Now) + "\\" + GetMonthName(DateTime.Now), GetMonthPath(DateTime.Now));
                    manageChanges++;
                    Console.WriteLine("[Folder Moved] \n " + GetYearPath(DateTime.Now) + "\\" + GetMonthName(DateTime.Now) + " -> " + GetMonthPath(DateTime.Now));
                }
                else
                {
                    Directory.CreateDirectory(GetMonthPath(DateTime.Now));
                    manageChanges++;
                    Console.WriteLine("[Folder Created] \n " + GetMonthPath(DateTime.Now));
                }
            }

            //create current day
            if (!Directory.Exists(GetDayPath(DateTime.Now)))
            {
                if (Directory.Exists(GetMonthPath(DateTime.Now) + "\\" + GetDayName(DateTime.Now)))
                {
                    Directory.Move(GetMonthPath(DateTime.Now) + "\\" + GetDayName(DateTime.Now), GetDayPath(DateTime.Now));
                    manageChanges++;
                    Console.WriteLine("[Folder Moved] \n " + GetMonthPath(DateTime.Now) + "\\" + GetDayName(DateTime.Now) + " -> " + GetDayPath(DateTime.Now));
                }
                else
                {
                    Directory.CreateDirectory(GetDayPath(DateTime.Now));
                    manageChanges++;
                    Console.WriteLine("[Folder Created] \n " + GetDayPath(DateTime.Now));

                }
            }


        }

        static string MoveFromMonitored()
        {
            string folders = string.Empty;
            string destPath = string.Empty;
            foreach (string folder in MonitoredPaths)
            {
                folders += "[" + folder + "] ";
                //move new folders from download folder for easy access in today's folder
                foreach (var path in Directory.GetDirectories(folder))
                {
                    string folderName = new DirectoryInfo(path).Name;
                    //if folder was downloaded/edited in last 1 min
                    destPath = GetDayPath(DateTime.Now) + "\\" + folderName;
                    if (DateTime.Now.AddMinutes(-10) < new DirectoryInfo(path).LastWriteTime || DateTime.Now.AddMinutes(-10) < new DirectoryInfo(path).CreationTime)
                    {
                        Directory.Move(path, destPath);
                        moveMonitoredChanges++;
                        Console.WriteLine("[Monitored Folder Moved] \n " + path + " -> " + destPath);
                    }
                }
                //move new files from download folder for easy access in today's folder
                foreach (var path in Directory.GetFiles(folder))
                {
                    string fileName = Path.GetFileName(path);
                    //if file was downloaded/edited in last 10 min
                    destPath = GetDayPath(DateTime.Now) + "\\" + fileName;
                    if (DateTime.Now.AddMinutes(-10) < File.GetLastWriteTime(path) || DateTime.Now.AddMinutes(-10) < new DirectoryInfo(path).CreationTime)
                    {
                        File.Move(path, destPath);
                        moveMonitoredChanges++;
                        Console.WriteLine("[Monitored File Moved] \n " + path + " -> " + destPath);
                    }
                }
            }
            //get only files from desktop to avoid problems with refresh folders.
            foreach (var path in Directory.GetFiles(InitialPath))
            {
                string fileName = Path.GetFileName(path);
                //if file was downloaded/edited in last 10 min
                destPath = GetDayPath(DateTime.Now) + "\\" + fileName;
                if (DateTime.Now.AddMinutes(-10) < File.GetLastWriteTime(path) || DateTime.Now.AddMinutes(-10) < new DirectoryInfo(path).CreationTime)
                {
                    File.Move(path, destPath);
                    moveMonitoredChanges++;
                    Console.WriteLine("[Monitored File Moved] \n " + path + " -> " + destPath);
                }
            }
            return folders;
        }
    }
}
