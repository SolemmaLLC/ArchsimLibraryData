using ArchsimLib;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excel2JSON
{
    public class ParseLib
    {

        public  static Library Excel2Lib(string file)
        {
            XSSFWorkbook wb;

            // get sheets list from xlsx
            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                wb = new XSSFWorkbook(fs);
            }


           
         


            //Logger.WriteLine("Worksheets in file:");
            for (int k = 0; k < wb.Count; k++)
            {
                //   Debug.WriteLine("Worksheet " + wb.GetSheetAt(k).SheetName);
                //  Logger.WriteLine("Worksheet " + wb.GetSheetAt(k).SheetName);
            }

            Library lib = LibraryDefaults.getHardCodedDefaultLib();

            //primitives
            foreach (var o in Parse.Objects<OpaqueMaterial>(wb ,"Material")) lib.Add(o);
            foreach (var o in Parse.Constructions(wb , "Construction", ref lib)) lib.Add(o);
            foreach (var o in Parse.Objects<GlazingConstructionSimple>(wb ,"GlazingConstructionSimple")) lib.Add(o);
            foreach (var o in Parse.Schedule(wb, "Schedule", ref lib)) lib.Add(o);
            foreach (var o in Parse.ArraySchedule(wb, "ArraySchedule", ref lib)) lib.Add(o);
           
            //zone defs
            foreach (var o in Parse.Objects<ZoneLoad>(wb ,"ZoneLoad")) lib.ZoneLoads.Add(o);
            foreach (var o in Parse.Objects<ZoneConditioning>(wb ,"ZoneConditioning")) lib.Add(o);
            foreach (var o in Parse.Objects<ZoneVentilation>(wb , "ZoneVentilation")) lib.Add(o);
            foreach (var o in Parse.Objects<ZoneConstruction>(wb , "ZoneConstruction")) lib.Add(o);
            foreach (var o in Parse.Objects<DomHotWater>( wb , "DomHotWater")) lib.Add(o);

            //zone and window
            foreach (var o in Parse.Objects<WindowSettings>(wb, "Window")) lib.Add(o);
            foreach (var o in Parse.Zone(wb , "Zone", ref lib)) lib.Add(o);

            //building

            foreach (var o in Parse.Objects<FloorDefinition>(wb ,"Building")) lib.Add(o);

            return lib;

        }


    }
}
