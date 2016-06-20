using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Data;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;


namespace ArchsimLib
{

    public static class DefaultLibrary
    {
        public readonly static string FileName = "ArchsimDefaultLibrary.json";
    }


    public static class ArchsimVersion
    {

        public const string ProductVersion = "4.0.2.8";
    }


    public static class Utilities
    {

        static public string AssemblyVersion
        {

            get
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                return fvi.FileVersion;
            }


        }


        static public string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
        public static void createDir(string path)
        {
            try
            {
                // If the directory doesn't exist, create it.
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                else
                {
                    System.IO.DirectoryInfo downloadedMessageInfo = new DirectoryInfo(path);

                    foreach (FileInfo file in downloadedMessageInfo.GetFiles())
                    {
                        file.Delete();
                    }
                    foreach (DirectoryInfo dir in downloadedMessageInfo.GetDirectories())
                    {
                        dir.Delete(true);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("createDir: " + ex.Message);
            }
        }




        public static bool IsJson(string input)
        {
            input = input.Trim();
            return input.StartsWith("{") && input.EndsWith("}")
                   || input.StartsWith("[") && input.EndsWith("]");
        }
        /// <summary> determines whether a string is formatted as a full file or directory path
        /// </summary>
        /// <param name="test_string">string to test</param>
        /// <returns>true or false</returns>
        public static bool StringIsPath(string test_string)
        {
            try
            {
                if (string.Equals(test_string, Path.GetFullPath(test_string), StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            catch { }

            return false;
        }
    }
}
