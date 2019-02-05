namespace Senparc.Scf.Core.Utility
{
    public class IoUtility
    {
        //COCONET .net core不支持FileSystemRights，需要继续改进

        //// Adds an ACL entry on the specified directory for the specified account.
        //public static void AddDirectorySecurity(string FileName, string User, FileSystemRights Rights, AccessControlType ControlType)
        //{
        //    // Create a new DirectoryInfo object.
        //    DirectoryInfo dInfo = new DirectoryInfo(FileName);

        //    // Get a DirectorySecurity object that represents the 
        //    // current security settings.
        //    DirectorySecurity dSecurity = dInfo.GetAccessControl();

        //    // Add the FileSystemAccessRule to the security settings. 
        //    dSecurity.AddAccessRule(new FileSystemAccessRule(User,
        //                                                    Rights,
        //                                                    ControlType));

        //    // Set the new access settings.
        //    dInfo.SetAccessControl(dSecurity);

        //}

        //// Removes an ACL entry on the specified directory for the specified account.
        //public static void RemoveDirectorySecurity(string FileName, string User, FileSystemRights Rights, AccessControlType ControlType)
        //{
        //    // Create a new DirectoryInfo object.
        //    DirectoryInfo dInfo = new DirectoryInfo(FileName);

        //    // Get a DirectorySecurity object that represents the 
        //    // current security settings.
        //    DirectorySecurity dSecurity = dInfo.GetAccessControl();

        //    // Add the FileSystemAccessRule to the security settings. 
        //    dSecurity.RemoveAccessRule(new FileSystemAccessRule(User,
        //                                                    Rights,
        //                                                    ControlType));

        //    // Set the new access settings.
        //    dInfo.SetAccessControl(dSecurity);

        //}

    }
}
