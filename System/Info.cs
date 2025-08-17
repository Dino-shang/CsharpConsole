namespace ConsoleTest.System;

public class Info
{
    public static void PringDriveInfo()
    {
        DriveInfo[] drives = DriveInfo.GetDrives();
        string dir = "C:\\";
        foreach (DriveInfo driveInfo in drives)
        {
            Console.WriteLine(driveInfo.DriveFormat);
            Console.WriteLine(driveInfo.Name);
            Console.WriteLine(driveInfo.DriveType);
            Console.WriteLine(driveInfo.IsReady);
            Console.WriteLine(driveInfo.AvailableFreeSpace);
            Console.WriteLine(driveInfo.TotalFreeSpace);
            Console.WriteLine(driveInfo.RootDirectory);
            Console.WriteLine(driveInfo.TotalSize);
            Console.WriteLine(driveInfo.VolumeLabel);
        }
    }
}