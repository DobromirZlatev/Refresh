using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Refresh
{
    class DailyFolders
    {
        public int ManageChanges { get; set; }
        public int MoveMonitoredChanges { get; set; }
        public int ArchiveChanges { get; set; }
        public int Changes { get; set; }
        string InitialPath { get; set; }
        string BackupPath { get; set; }
        List<string> MonitoredPaths { get; set; }
        bool MonitorInitialPath { get; set; }
        public DailyFolders(string initialPath, string backupPath, List<string> monitoredPaths, bool monitorInitialPath)
        {
            InitialPath = initialPath;
            BackupPath = backupPath;
            MonitoredPaths = monitoredPaths;
            MonitorInitialPath = monitorInitialPath;
        }

        public int GetTotalChanges()
        {
            return ManageChanges + MoveMonitoredChanges + ArchiveChanges;
        }

        public string GetChange_s(int numberOfChanges)
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
        string GetDayPath(DateTime date)
        {
            return InitialPath + GetDayName(date);
        }
        string GetMonthPath(DateTime date)
        {
            return InitialPath + GetMonthName(date);
        }
        string GetYearPath(DateTime date)
        {
            return InitialPath + GetYearName(date);
        }
        string GetDayName(DateTime date)
        {
            var result = date.Day.ToString();
            if (result.Length == 1)
            {
                result = "0" + result;
            }
            return result;
        }
        string GetMonthName(DateTime date)
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
        string GetYearName(DateTime date)
        {
            return date.Year.ToString();
        }
        public void ArchiveFolders()
        {
            for (int i = 0; i < 10; i++)
            {
                string file = GetYearPath(DateTime.Now.AddYears(-i));
                string destFile = Path.Combine(BackupPath, GetYearName(DateTime.Now.AddYears(-i)));
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
                            ArchiveChanges++;
                            Console.WriteLine("[Backup Folder Updated] \n" + destFile);
                        }
                    }
                    else
                    {
                        FileSystem.CopyDirectory(file, destFile);
                        ArchiveChanges++;
                        Console.WriteLine("[Backup Folder Created] \n " + destFile);
                    }
                }
            }

            //Copy this month folder
            if (Directory.Exists(GetMonthPath(DateTime.Now)))
            {
                string file = GetMonthPath(DateTime.Now);
                string destFile = Path.Combine(BackupPath, GetMonthName(DateTime.Now));
                DirectoryInfo fileInfo = FileSystem.GetDirectoryInfo(file);
                DirectoryInfo destFileInfo = FileSystem.GetDirectoryInfo(destFile);
                if (FileSystem.DirectoryExists(destFile))
                {
                    if (fileInfo.LastWriteTime > destFileInfo.LastWriteTime)
                    {
                        FileSystem.DeleteDirectory(destFile, DeleteDirectoryOption.DeleteAllContents);
                        //FileSystem.CopyDirectory(file, destFile);
                        FileSystem.CopyDirectory(file, destFile);
                        ArchiveChanges++;
                        Console.WriteLine("[Backup Folder Updated] \n " + destFile);
                    }
                    else
                    {
                    }
                }
                else if (FileSystem.DirectoryExists(file))
                {
                    FileSystem.CopyDirectory(file, destFile);
                    ArchiveChanges++;
                    Console.WriteLine("[Backup Folder Created] \n " + destFile);
                }
            }

            //Copy todays folder to OneDrive's month folder.
            if (Directory.Exists(GetDayPath(DateTime.Now)))
            {
                string file = GetDayPath(DateTime.Now);
                string destFile = Path.Combine(BackupPath, GetMonthName(DateTime.Now), GetDayName(DateTime.Now));
                DirectoryInfo fileInfo = FileSystem.GetDirectoryInfo(file);
                DirectoryInfo destFileInfo = FileSystem.GetDirectoryInfo(destFile);
                if (FileSystem.DirectoryExists(destFile))
                {
                    if (fileInfo.LastWriteTime > destFileInfo.LastWriteTime)
                    {
                        FileSystem.DeleteDirectory(destFile, DeleteDirectoryOption.DeleteAllContents);
                        FileSystem.CopyDirectory(file, destFile);
                        ArchiveChanges++;
                        Console.WriteLine("[Backup Folder Updated] \n " + destFile);
                    }
                }
                else if (FileSystem.DirectoryExists(file))
                {
                    FileSystem.CopyDirectory(file, destFile);
                    ArchiveChanges++;
                    Console.WriteLine("[Backup Folder Created] \n " + destFile);
                }
            }

        }
        public void ManageFolders()
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
                    ManageChanges++;
                    Console.WriteLine("[Folder Moved] \n " + GetDayPath(DateTime.Now.AddDays(-i)) + " -> " + GetMonthPath(DateTime.Now.AddDays(-i)) + "\\" + GetDayName(DateTime.Now.AddDays(-i)));
                }
            }
            Thread.Sleep(1000);
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
                    ManageChanges++;
                    Console.WriteLine("[Folder Moved] \n " + GetMonthPath(DateTime.Now.AddMonths(-i)) + " -> " + GetYearPath(DateTime.Now.AddMonths(-i)) + "\\" + GetMonthName(DateTime.Now.AddMonths(-i)));
                }
            }

            //create current year
            if (!Directory.Exists(GetYearPath(DateTime.Now)))
            {
                Directory.CreateDirectory(GetYearPath(DateTime.Now));
                ManageChanges++;
                Console.WriteLine("[Folder Created] \n " + GetYearPath(DateTime.Now));
            }
            //create current month
            if (!Directory.Exists(GetMonthPath(DateTime.Now)))
            {
                if (Directory.Exists(GetYearPath(DateTime.Now) + "\\" + GetMonthName(DateTime.Now)))
                {
                    Directory.Move(GetYearPath(DateTime.Now) + "\\" + GetMonthName(DateTime.Now), GetMonthPath(DateTime.Now));
                    ManageChanges++;
                    Console.WriteLine("[Folder Moved] \n " + GetYearPath(DateTime.Now) + "\\" + GetMonthName(DateTime.Now) + " -> " + GetMonthPath(DateTime.Now));
                }
                else
                {
                    Directory.CreateDirectory(GetMonthPath(DateTime.Now));
                    ManageChanges++;
                    Console.WriteLine("[Folder Created] \n " + GetMonthPath(DateTime.Now));
                }
            }
            //create current day
            if (!Directory.Exists(GetDayPath(DateTime.Now)))
            {
                if (Directory.Exists(GetMonthPath(DateTime.Now) + "\\" + GetDayName(DateTime.Now)))
                {
                    Directory.Move(GetMonthPath(DateTime.Now) + "\\" + GetDayName(DateTime.Now), GetDayPath(DateTime.Now));
                    ManageChanges++;
                    Console.WriteLine("[Folder Moved] \n " + GetMonthPath(DateTime.Now) + "\\" + GetDayName(DateTime.Now) + " -> " + GetDayPath(DateTime.Now));
                }
                else
                {
                    Directory.CreateDirectory(GetDayPath(DateTime.Now));
                    ManageChanges++;
                    Console.WriteLine("[Folder Created] \n " + GetDayPath(DateTime.Now));
                }
            }


        }

        public string MoveFromMonitored()
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
                        MoveMonitoredChanges++;
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
                        MoveMonitoredChanges++;
                        Console.WriteLine("[Monitored File Moved] \n " + path + " -> " + destPath);
                    }
                }
            }
            //get files only (no folders) from desktop to avoid problems with refresh folders. (separately)
            if (MonitorInitialPath)
            {
                foreach (var path in Directory.GetFiles(InitialPath))
                {
                    string fileName = Path.GetFileName(path);
                    //if file was downloaded/edited in last 10 min
                    destPath = GetDayPath(DateTime.Now) + "\\" + fileName;
                    if (DateTime.Now.AddMinutes(-10) < File.GetLastWriteTime(path) || DateTime.Now.AddMinutes(-10) < new DirectoryInfo(path).CreationTime)
                    {
                        File.Move(path, destPath);
                        MoveMonitoredChanges++;
                        Console.WriteLine("[Monitored File Moved] \n " + path + " -> " + destPath);
                    }
                }
            }
            return folders;
        }
    }
}
