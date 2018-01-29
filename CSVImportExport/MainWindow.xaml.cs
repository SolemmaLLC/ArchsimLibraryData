using ArchsimLib;
using CsvHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CSVImportExport
{
        
    

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window

    { 
        public void writeOpaqueConstructionsCSV(string fp, List<OpaqueConstruction> records)
        {
            using (var sw = new StreamWriter(fp))
            {
                // var records = lib.OpaqueMaterials;
                var csv = new CsvWriter(sw);
                //csv.WriteRecords(records);
                csv.WriteHeader(typeof(OpaqueConstruction));
                csv.WriteField("LayerCount");
                csv.WriteField("Layers");
               
                csv.NextRecord();

                foreach (var record in records)
                {
                    //Write entire current record
                    csv.WriteRecord(record);
                    csv.WriteField(record.Layers.Count);
                    foreach (var l in record.Layers)
                    {

                        csv.WriteField(l.Thickness);

                    }
                    foreach (var l in record.Layers) {

                        csv.WriteField(l.Material.Name);

                    }

                    csv.NextRecord();
                }
            }
        }
        public List<OpaqueConstruction> readOpaqueConstructionsCSV(string fp, List<OpaqueMaterial> mat)
        {
            var records = new List<OpaqueConstruction>();
            using (var sr = new StreamReader(fp))
            {

                var csv = new CsvReader(sr);
                while (csv.Read())
                {
                    try
                    {
                        bool foundAllMaterials = true;
                        OpaqueConstruction record = csv.GetRecord<OpaqueConstruction>();
                        int layerCnt = csv.GetField<int>("LayerCount");
                        int layerStartAt = csv.GetFieldIndex("LayerCount")+1;
                        for (int i = layerStartAt; i< layerStartAt+layerCnt; i++)
                        {
                            var thick = csv.GetField<double>(i);
                            var name = csv.GetField<string>(i + layerCnt);

                            if (mat.Any(x => x.Name == name)) {

                                var m = mat.First(x => x.Name == name);
                                record.Layers.Add(new Layer<OpaqueMaterial>(thick, m));

                            }
                            else { foundAllMaterials = false; }

                        }
                        if (foundAllMaterials) records.Add(record);
                        else { System.Windows.MessageBox.Show(record.Name + " contains materials not found in library"); }
                    }
                    catch (Exception ex) { Debug.WriteLine(ex.Message); }
                }
            }
            return records;
        }

        public void writeOpaqueMaterialCSV(string fp, List<OpaqueMaterial> records) {
            using (var sw = new StreamWriter(fp))
            {
               // var records = lib.OpaqueMaterials;
                var csv = new CsvWriter(sw);
                //csv.WriteRecords(records);
                csv.WriteHeader(typeof(OpaqueMaterial));
                csv.WriteField("TemperatureArray");
                csv.WriteField("EnthalpyArray");
                csv.WriteField("VariableConductivityArray");
                csv.NextRecord();

                foreach (var record in records)
                {
                    //Write entire current record
                    csv.WriteRecord(record);
                    if (record.VariableConductivity || record.PhaseChange)
                    {
                        //write record field by field
                        csv.WriteField(JsonConvert.SerializeObject(record.TemperatureArray));
                        csv.WriteField(JsonConvert.SerializeObject(record.EnthalpyArray));
                        csv.WriteField(JsonConvert.SerializeObject(record.VariableConductivityArray));
                    }
                    csv.NextRecord();
                }
            }
        }
        public List<OpaqueMaterial> readOpaqueMaterialCSV(string fp)
        {
            var records = new List<OpaqueMaterial>();
            using (var sr = new StreamReader(fp))
            {
                
                var csv = new CsvReader(sr);
                while (csv.Read())
                {
                    try
                    {
                        OpaqueMaterial record = csv.GetRecord<OpaqueMaterial>();
                        if (record.VariableConductivity || record.PhaseChange)
                        {
                            record.TemperatureArray = JsonConvert.DeserializeObject<List<double>>(csv.GetField<string>("TemperatureArray"));
                            record.EnthalpyArray = JsonConvert.DeserializeObject<List<double>>(csv.GetField<string>("EnthalpyArray"));
                            record.VariableConductivityArray = JsonConvert.DeserializeObject<List<double>>(csv.GetField<string>("VariableConductivityArray"));
                        }
                        records.Add(record);
                    }
                    catch (Exception ex) { Debug.WriteLine(ex.Message); }
                }
            }
            return records;
        }

        public void writeGlazingMaterialCSV(string fp, List<GlazingMaterial> records)
        {
            using (var sw = new StreamWriter(fp))
            {
                
                var csv = new CsvWriter(sw);
                csv.WriteRecords(records);
                
            }
        }
        public List<GlazingMaterial> readGlazingMaterialCSV(string fp)
        {
            var records = new List<GlazingMaterial>();
            using (var sr = new StreamReader(fp))
            {

                var csv = new CsvReader(sr);
                records = csv.GetRecords<GlazingMaterial>().ToList();
                
            }
            return records;
        }

        public void writeLibCSV<T>(string fp, List<T> records)
        {
            using (var sw = new StreamWriter(fp))
            {

                var csv = new CsvWriter(sw);
                csv.WriteRecords(records);

            }
        }
        public List<T> readLibCSV<T>(string fp)
        {
            var records = new List<T>();
            using (var sr = new StreamReader(fp))
            {

                var csv = new CsvReader(sr);
                records = csv.GetRecords<T>().ToList();

            }
            return records;
        }



        public MainWindow()
        {
            InitializeComponent();

            var lib = ArchsimLib.LibraryDefaults.getHardCodedDefaultLib();

            writeOpaqueMaterialCSV(@"C:\Users\Timur\Desktop\OpaqueMaterials.csv", lib.OpaqueMaterials.ToList());
            List<OpaqueMaterial> inOMat = readOpaqueMaterialCSV(@"C:\Users\Timur\Desktop\OpaqueMaterialsIN.csv");

            writeGlazingMaterialCSV(@"C:\Users\Timur\Desktop\GlazingMaterials.csv", lib.GlazingMaterials.ToList());
            List<GlazingMaterial> inGMat = readGlazingMaterialCSV(@"C:\Users\Timur\Desktop\GlazingMaterialsIN.csv");

            writeLibCSV<ZoneVentilation>(@"C:\Users\Timur\Desktop\ZoneVentilations.csv", lib.ZoneVentilations.ToList());
            List<ZoneVentilation> inZoneVentilation = readLibCSV<ZoneVentilation>(@"C:\Users\Timur\Desktop\ZoneVentilations.csv");

            writeOpaqueConstructionsCSV(@"C:\Users\Timur\Desktop\OpaqueConstructions.csv", lib.OpaqueConstructions.ToList());
            List<OpaqueConstruction> inOpaqueConstructions = readOpaqueConstructionsCSV(@"C:\Users\Timur\Desktop\OpaqueConstructions.csv", inOMat);




        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = fbd.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                   
                     TextBX.Text = fbd.SelectedPath;   

                    string[] files = Directory.GetFiles(fbd.SelectedPath);

                    System.Windows.Forms.MessageBox.Show("Files found: " + files.Length.ToString(), "Message");

                 

                }
            }
        }
    }
}
