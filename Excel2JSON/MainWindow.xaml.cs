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
using Microsoft.Win32;
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



            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "XLSX files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                //foreach (string filename in openFileDialog.FileNames)
                //    lbFiles.Items.Add(Path.GetFileName(filename));
                file = openFileDialog.FileName;
            }




            var lib = Excel2Lib(file);

            string JsonPath = System.IO.Path.GetDirectoryName(file);
            string JsonName = System.IO.Path.GetFileNameWithoutExtension(file) + ".json" ;
            string JsonFile = System.IO.Path.Combine(JsonPath, JsonName);

           File.WriteAllText(JsonFile, lib.toJSON());

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


            //var OpaqueMaterial = Parse.Objects<OpaqueMaterial>((XSSFSheet) wb.GetSheet("OpaqueMaterial"));
            //var GlazingConstructionSimple =Parse.Objects<GlazingConstructionSimple>((XSSFSheet) wb.GetSheet("GlazingConstructionSimple"));
            //var ZoneLoad = Parse.Objects<ZoneLoad>((XSSFSheet) wb.GetSheet("ZoneLoad"));
            //var ZoneConditioning = Parse.Objects<ZoneConditioning>((XSSFSheet) wb.GetSheet("ZoneConditioning"));
            //var ZoneVentilation = Parse.Objects<ZoneVentilation>((XSSFSheet) wb.GetSheet("ZoneVentilation"));
            //var ZoneConstruction = Parse.Objects<ZoneConstruction>((XSSFSheet) wb.GetSheet("ZoneConstruction"));
            //var DomHotWater = Parse.Objects<DomHotWater>((XSSFSheet) wb.GetSheet("DomHotWater"));


            lib.OpaqueMaterials = Parse.Objects<OpaqueMaterial>((XSSFSheet)wb.GetSheet("OpaqueMaterial"));
            lib.GlazingConstructionsSimple = Parse.Objects<GlazingConstructionSimple>((XSSFSheet)wb.GetSheet("GlazingConstructionSimple"));
            lib.ZoneLoads = Parse.Objects<ZoneLoad>((XSSFSheet)wb.GetSheet("ZoneLoad"));
            lib.ZoneConditionings = Parse.Objects<ZoneConditioning>((XSSFSheet)wb.GetSheet("ZoneConditioning"));
            lib.ZoneVentilations = Parse.Objects<ZoneVentilation>((XSSFSheet)wb.GetSheet("ZoneVentilation"));
            lib.ZoneConstructions = Parse.Objects<ZoneConstruction>((XSSFSheet)wb.GetSheet("ZoneConstruction"));
            lib.DomHotWaters = Parse.Objects<DomHotWater>((XSSFSheet)wb.GetSheet("DomHotWater"));



            lib.OpaqueConstructions = Parse.Constructions((XSSFSheet) wb.GetSheet("Construction"), ref lib);


            return lib;

        }
    }
}
