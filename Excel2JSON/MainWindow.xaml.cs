using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Excel;
using System.Diagnostics;
using System.IO;
using ArchsimLib;
using NPOI.XSSF.UserModel;

namespace Excel2JSON
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenBth_Click(object sender, RoutedEventArgs e)
        {
            string file =
                @"C:\Users\Timur\Dropbox (Personal)\_CSHARP_PROJECTS\ArchsimLibraryData\160603_ExcelLibraryEditor.xlsx";


           var lib = Excel2Lib(file);


            //int i = 0;
            //while (sh.GetRow(i) != null)
            //{
            //    // write row value
            //    for (int j = 0; j < sh.GetRow(i).Cells.Count; j++)
            //    {
            //        var cell = sh.GetRow(i).GetCell(j);

            //        if (cell != null)
            //        {
            //            // TODO: you can add more cell types capability, e. g. formula
            //            switch (cell.CellType)
            //            {
            //                case NPOI.SS.UserModel.CellType.Numeric:
            //                    Debug.WriteLine( sh.GetRow(i).GetCell(j).NumericCellValue );
            //                    break;
            //                case NPOI.SS.UserModel.CellType.String:
            //                    Debug.WriteLine( sh.GetRow(i).GetCell(j).StringCellValue );
            //                    break;
            //            }
            //        }
            //    }

            //    i++;
            //}



            //var worksheets = Workbook.Worksheets(@"C:\Users\Timur\Dropbox (Personal)\_CSHARP_PROJECTS\ArchsimLibraryData\160603_ExcelLibraryEditor.xlsx").ToArray();

            //var OpaqueMaterial = Parse.Objects<OpaqueMaterial>(worksheets[0]);

            //var GlazingConstructionSimple = Parse.Objects<GlazingConstructionSimple>(worksheets[2]);

            //var ZoneLoad = Parse.Objects<ZoneLoad>(worksheets[3]);

            //var ZoneConditioning = Parse.Objects<ZoneConditioning>(worksheets[4]);

            //var ZoneVentilation = Parse.Objects<ZoneVentilation>(worksheets[5]);

            //var ZoneConstruction = Parse.Objects<ZoneConstruction>(worksheets[6]);

            //var DomHotWater = Parse.Objects<DomHotWater>(worksheets[7]);





            //for (int k = 0; k < worksheets.Length; k++)
            //{

            //    var worksheet = worksheets[k];

            //    Debug.WriteLine("Worksheet " + k);

            //    if (k == 0) Parse.Materials(worksheets);


            //}
        }

        private static Library Excel2Lib(string file)
        {
            XSSFWorkbook wb;


            // get sheets list from xlsx
            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                wb = new XSSFWorkbook(fs);
            }


            for (int k = 0; k < wb.Count; k++)
            {
                Debug.WriteLine("Worksheet " + wb.GetSheetAt(k).SheetName);
            }
             
            Library lib = new Library();


            //var OpaqueMaterial = Parse.ObjectsNew<OpaqueMaterial>((XSSFSheet) wb.GetSheet("OpaqueMaterial"));
            //var GlazingConstructionSimple =Parse.ObjectsNew<GlazingConstructionSimple>((XSSFSheet) wb.GetSheet("GlazingConstructionSimple"));
            //var ZoneLoad = Parse.ObjectsNew<ZoneLoad>((XSSFSheet) wb.GetSheet("ZoneLoad"));
            //var ZoneConditioning = Parse.ObjectsNew<ZoneConditioning>((XSSFSheet) wb.GetSheet("ZoneConditioning"));
            //var ZoneVentilation = Parse.ObjectsNew<ZoneVentilation>((XSSFSheet) wb.GetSheet("ZoneVentilation"));
            //var ZoneConstruction = Parse.ObjectsNew<ZoneConstruction>((XSSFSheet) wb.GetSheet("ZoneConstruction"));
            //var DomHotWater = Parse.ObjectsNew<DomHotWater>((XSSFSheet) wb.GetSheet("DomHotWater"));


            lib.OpaqueMaterials = Parse.ObjectsNew<OpaqueMaterial>((XSSFSheet)wb.GetSheet("OpaqueMaterial"));
            lib.GlazingConstructionsSimple = Parse.ObjectsNew<GlazingConstructionSimple>((XSSFSheet)wb.GetSheet("GlazingConstructionSimple"));
            lib.ZoneLoads = Parse.ObjectsNew<ZoneLoad>((XSSFSheet)wb.GetSheet("ZoneLoad"));
            lib.ZoneConditionings = Parse.ObjectsNew<ZoneConditioning>((XSSFSheet)wb.GetSheet("ZoneConditioning"));
            lib.ZoneVentilations = Parse.ObjectsNew<ZoneVentilation>((XSSFSheet)wb.GetSheet("ZoneVentilation"));
            lib.ZoneConstructions = Parse.ObjectsNew<ZoneConstruction>((XSSFSheet)wb.GetSheet("ZoneConstruction"));
            lib.DomHotWaters = Parse.ObjectsNew<DomHotWater>((XSSFSheet)wb.GetSheet("DomHotWater"));



            lib.OpaqueConstructions = Parse.Constructions((XSSFSheet) wb.GetSheet("Construction"), ref lib);


            return lib;

        }
    }
}
