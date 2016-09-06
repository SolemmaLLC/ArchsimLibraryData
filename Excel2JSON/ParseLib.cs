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
            Library lib = LibraryDefaults.getHardCodedDefaultLib();


            XSSFWorkbook wb;

            // get sheets list from xlsx
            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                wb = new XSSFWorkbook(fs);
            }


            Parse Wr = new Parse(wb, ref lib);
         


            //Logger.WriteLine("Worksheets in file:");
            for (int k = 0; k < wb.Count; k++)
            {
                //   Debug.WriteLine("Worksheet " + wb.GetSheetAt(k).SheetName);
                //  Logger.WriteLine("Worksheet " + wb.GetSheetAt(k).SheetName);
            }

           

            //primitives
            foreach (var o in Wr.Objects<OpaqueMaterial>("Material")) lib.Add(o);
            foreach (var o in Wr.Constructions( "Construction")) lib.Add(o);
            foreach (var o in Wr.Objects<GlazingConstructionSimple>("GlazingConstructionSimple")) lib.Add(o);
            foreach (var o in Wr.Schedule("Schedule")) lib.Add(o);
            foreach (var o in Wr.ArraySchedule("ArraySchedule")) lib.Add(o);
           
            //zone defs
            foreach (var o in Wr.Objects<ZoneLoad>("ZoneLoad")) lib.ZoneLoads.Add(o);
            foreach (var o in Wr.Objects<ZoneConditioning>("ZoneConditioning")) lib.Add(o);
            foreach (var o in Wr.Objects<ZoneVentilation>( "ZoneVentilation")) lib.Add(o);
            foreach (var o in Wr.Objects<ZoneConstruction>("ZoneConstruction")) lib.Add(o);
            foreach (var o in Wr.Objects<DomHotWater>("DomHotWater")) lib.Add(o);

            //zone and window
            foreach (var o in Wr.Objects<WindowSettings>("Window")) lib.Add(o);
            foreach (var o in Wr.Zone("Zone")) lib.Add(o);

            //building
            foreach (var o in Wr.Objects<FloorDefinition>("Building")) lib.Add(o);

            return lib;

        }


    }
}
