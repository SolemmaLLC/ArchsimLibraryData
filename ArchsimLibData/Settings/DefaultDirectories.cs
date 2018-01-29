using System;
using System.Diagnostics;
using System.IO;

namespace ArchsimLib
{
    public static class DefaultFilesAndDirectories
    {


        private const string LibraryFileName = "ArchsimDefaultLibrary.json";



		public static string LibraryFilePath
		{
			get
			{
				if (Platform.isWin())
				{
					return Utilities.AssemblyDirectory + @"\" + DefaultFilesAndDirectories.LibraryFileName;
				}
				else
				{
					return Utilities.AssemblyDirectory + @"/" + DefaultFilesAndDirectories.LibraryFileName;
				}
			}
		}

        public static string WeatherFiles
        {
            get
            {
                if (Platform.isWin()) 
                { 
                    return @"C:\DIVA\WeatherData"; 
                }
                else 
                {
                    return @"/Applications/DIVA/WeatherData";
                    //return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "DIVA/WeatherData"); 
                }
            }
        }


        public static string WorkingDir
        {
            get
            {
                if (Platform.isWin())
                {
                    string path = @"C:\DIVA\Temp";
                    Utilities.createDir(path);
                    return path;
                }
                else {
                    return Environment.GetFolderPath(Environment.SpecialFolder.Desktop); 
                }

                // return Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }
        }


        public static string EnergyPlusReadESOVars
        
        {
            get
            {
                if (Platform.isWin())
                {
                    return @"C:\DIVA\EnergyPlusV8-4-0\PostProcess\ReadVarsESO.exe";
                }
                else { return @"/Applications/DIVA/EnergyPlus-8-5-0/PostProcess/ReadVarsESO"; }
            }
        }

        public static string EnergyPlusExpandObjects

        {
            get
            {
                if (Platform.isWin())
                {
                    return @"C:\DIVA\EnergyPlusV8-4-0\ExpandObjects.exe";
                }
                else { return @"/Applications/DIVA/EnergyPlus-8-5-0/ExpandObjects"; }
            }
        }


        public static string EnergyPlus
        {
            get
            {
                if (Platform.isWin())
                {
                    return @"C:\DIVA\EnergyPlusV8-4-0";  // EPDIR = @"C:\DIVA\EnergyPlusV8-4-0"; 
                }
                else { return @"/Applications/DIVA/EnergyPlus-8-5-0"; }
            }
        }

        public static string EnergyPlusPixel
        {
            get
            {
                if (Platform.isWin())
                {
                    return @"C:\DIVA\EnergyPlusV8-5-Pixel"; 
               }
               else 
                { 
                    return @"/Applications/DIVA/EnergyPlus-8-5-Pixel"; 
                }
            }
        }
        public static string EnergyPlusPixelShim
        {
            get
            {
                if (Platform.isWin())
                {

                    return "Epshim.exe";
                }
                else
                {
                    return "Epshim";
                }
            }
        }

        public static string EnergyPlusExecutable
        {
            get
            {
                if (Platform.isWin())
                {
                    //return "Epl-run.bat";
                    return "energyplus.exe";
                }
                else
                {
                    return "energyplus";
                }
            }
        }

		public static string EnergyPlusArgs(string path, string filename, string weather)
        {
			//if (Platform.isWin())
			//{  
                //return "\"" + path + @"\" + filename + "\"" + " " + "\"" + path + @"\" + filename + "\"" + " " + "idf " + "\"" + weather + "\"" + " EP N nolimit N Y 0 N";
                return " -r -w \"" + Path.GetFullPath(weather) + "\" -d \"" + Path.GetFullPath(path) + "\" -p \"" + filename + "\" \"" + Path.Combine(path,  filename + ".idf") + "\"";
            //}
			//else {
            //return " -r -w \"" + Path.GetFullPath(weather) + "\" -d \"" + Path.GetFullPath(path) + "\" -p \"" + filename + "\" \"" + Path.Combine(path, filename + ".idf") + "\"";
            //}
        }



        public static string EnergyPlusPixelShimArgs(string path, string filename, string weather)
        {
           //if (Platform.isWin())
           // {
                return Path.Combine(EnergyPlusPixel, EnergyPlusExecutable) + " " + Path.Combine(path, filename + ".idf") + " " +Path.GetFullPath(weather) +
                " -ep " + 
                " -r -w \"" + Path.GetFullPath(weather) + "\" -d \"" + Path.GetFullPath(path) + "\" -p \"" + filename + "\" \"" + Path.Combine(path, filename + ".idf") + "\"";

                //return "\"" + path + @"\" + filename + "\"" + " " + "\"" + path + @"\" + filename + "\"" + " " + "idf " + "\"" + weather + "\"" + " EP N nolimit N Y 0 N";
          //  }
          //  else
          //  {
          //      return " -r -w \"" + Path.GetFullPath(weather) + "\" -d \"" + Path.GetFullPath(path) + "\" -p \"" + filename + "\" \"" + Path.Combine(path, filename + ".idf") + "\"";
          //  }
        }



    }
}


//Usage: energyplus[options][input - file]
//Options:
//  -a, --annual Force annual simulation
//  -d, --output-directory ARG   Output directory path(default: current
//                               directory)
//  -D, --design-day Force design-day-only simulation
//  -h, --help Display help information
//  -i, --idd ARG                Input data dictionary path(default: Energy+.idd
//                               in executable directory)
//  -m, --epmacro Run EPMacro prior to simulation
//  -p, --output-prefix ARG      Prefix for output file names(default: eplus)
//  -r, --readvars Run ReadVarsESO after simulation
//  -s, --output-suffix ARG      Suffix style for output file names(default: L)
//                                  L: Legacy(e.g., eplustbl.csv)
//                                  C: Capital(e.g., eplusTable.csv)
//                                  D: Dash(e.g., eplus-table.csv)
//  -v, --version Display version information
//  -w, --weather ARG            Weather file path(default: in.epw in current
//                               directory)
//  -x, --expandobjects Run ExpandObjects prior to simulation
//Example: energyplus -w weather.epw -r input.idf