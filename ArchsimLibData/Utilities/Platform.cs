using System;
using System.Diagnostics;

namespace ArchsimLib
{
    public static class Platform
    {
        public static  bool isWin() { 
        OperatingSystem os = Environment.OSVersion;
        PlatformID pid = os.Platform;
            switch (pid)
            {
                case PlatformID.Win32NT:
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                case PlatformID.WinCE:
                    Debug.WriteLine("I'm on windows!");
                    return true;
                    //break;
                case PlatformID.Unix:
                    Debug.WriteLine("I'm a linux box!");
                    return false;
                    //break;
                case PlatformID.MacOSX:
                    Debug.WriteLine("I'm a mac!");
                    return false;
                    //break;
                default:
                    Debug.WriteLine("No Idea what I'm on!");
                    return false;
                    //break;
            }
    }

        public static bool isMac()
        {
            OperatingSystem os = Environment.OSVersion;
            PlatformID pid = os.Platform;
            switch (pid)
            {
                case PlatformID.Win32NT:
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                case PlatformID.WinCE:
                    Debug.WriteLine("I'm on windows!");
                    return false;
                 
                case PlatformID.Unix:
                    Debug.WriteLine("I'm a linux box!");
                    return false;
                    
                case PlatformID.MacOSX:
                    Debug.WriteLine("I'm a mac!");
                    return true;
                   
                default:
                    Debug.WriteLine("No Idea what I'm on!");
                    return false;
                   
            }
        }
    }
}
