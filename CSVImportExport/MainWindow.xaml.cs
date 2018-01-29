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

        public void writeYearCSV(string fp, List<YearSchedule> records)
        {
            using (var sw = new StreamWriter(fp))
            {
                var csv = new CsvWriter(sw);
                csv.WriteHeader(typeof(YearSchedule));
                csv.WriteField("Week Schedules Count");
                csv.WriteField("Week Schedules");
        
                csv.NextRecord();

                foreach (var record in records)
                {
                    //Write entire current record
                    csv.WriteRecord(record);
                    csv.WriteRecord(record.WeekSchedules.Count);
                    foreach (var w in record.WeekSchedules)
                    {
                        csv.WriteField(w.From);
                        csv.WriteField(w.To);
                        foreach (var d in w.Days)
                        {
                            csv.WriteField(d.Name);
                        }
                    }

                    csv.NextRecord();
                }
            }
        }
        public List<YearSchedule> readYearCSV(string fp, List<DaySchedule> days)
        {
            var records = new List<YearSchedule>();
            using (var sr = new StreamReader(fp))
            {
                var csv = new CsvReader(sr);
                while (csv.Read())
                {
                    try
                    {
                        bool foundAllSchedules = true;
                        YearSchedule record = csv.GetRecord<YearSchedule>();
                        int weekCnt = csv.GetField<int>("Week Schedules Count");
                        int weeksStartAt = csv.GetFieldIndex("Week Schedules Count") + 1;

                        for (int i = weeksStartAt; i < weeksStartAt + weekCnt*9; i+=9)
                        {
                            var weekSched = new WeekSchedule();

                            weekSched.From = csv.GetField<DateTime>(i);
                            weekSched.To = csv.GetField<DateTime>(i+1);
                            weekSched.Days = new DaySchedule[7];


                            for (int j = 0; j < 7; j++) {
                                string weekDay= csv.GetField<string>(2 + i+j) ;

                                if (days.Any(x => x.Name == weekDay))
                                {
                                    weekSched.Days[i] = days.First(x => x.Name == weekDay);
                                }
                                else {
                                    foundAllSchedules = false;
                                }

                            }
                            record.WeekSchedules.Add(weekSched);
                        }
                       
                        if (foundAllSchedules) records.Add(record);
                        else { System.Windows.MessageBox.Show(record.Name + " contains day schedules that are not found in library"); }
                    }
                    catch (Exception ex) { Debug.WriteLine(ex.Message); }
                }
            }
            return records;
        }

        public void writeDayCSV(string fp, List<DaySchedule> records)
        {
            using (var sw = new StreamWriter(fp))
            {
                // var records = lib.OpaqueMaterials;
                var csv = new CsvWriter(sw);
                //csv.WriteRecords(records);
                csv.WriteHeader(typeof(DaySchedule));
                csv.WriteField("1");
                csv.WriteField("2");
                csv.WriteField("3");
                csv.WriteField("4");
                csv.WriteField("5");
                csv.WriteField("6");
                csv.WriteField("7");
                csv.WriteField("8");
                csv.WriteField("9");
                csv.WriteField("10");
                csv.WriteField("11");
                csv.WriteField("12");
                csv.WriteField("13");
                csv.WriteField("14");
                csv.WriteField("15");
                csv.WriteField("16");
                csv.WriteField("17");
                csv.WriteField("18");
                csv.WriteField("19");
                csv.WriteField("20");
                csv.WriteField("21");
                csv.WriteField("22");
                csv.WriteField("23");
                csv.WriteField("24");

                csv.NextRecord();

                foreach (var record in records)
                {
                    //Write entire current record
                    csv.WriteRecord(record);
  
                    foreach (var h in record.Values)
                    {
                        csv.WriteField(h);
                    }


                    csv.NextRecord();
                }
            }
        }
        public List<DaySchedule> readDayCSV(string fp)
        {
            var records = new List<DaySchedule>();
            using (var sr = new StreamReader(fp))
            {
                var csv = new CsvReader(sr);
                while (csv.Read())
                {
                    try
                    {
                        DaySchedule record = csv.GetRecord<DaySchedule>();
                
                        int layerStartAt = 5;
                        for (int i = layerStartAt; i < layerStartAt + 24; i++)
                        {
                            var h = csv.GetField<double>(i);
                            record.Values.Add(h);
                        }

                        records.Add(record);
                                          }
                    catch (Exception ex) { Debug.WriteLine(ex.Message); }
                }
            }
            return records;
        }

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

            

            writeDayCSV(@"C:\Users\Timur\Desktop\DaySchedules.csv", lib.DaySchedules.ToList());
            List<DaySchedule> inDaySchedules = readDayCSV(@"C:\Users\Timur\Desktop\DaySchedules.csv");

            writeYearCSV(@"C:\Users\Timur\Desktop\YearSchedules.csv", lib.YearSchedules.ToList());
            List<YearSchedule> inYearSchedules = readYearCSV(@"C:\Users\Timur\Desktop\YearSchedules.csv", inDaySchedules);

            writeOpaqueMaterialCSV(@"C:\Users\Timur\Desktop\OpaqueMaterials.csv", lib.OpaqueMaterials.ToList());
            List<OpaqueMaterial> inOMat = readOpaqueMaterialCSV(@"C:\Users\Timur\Desktop\OpaqueMaterials.csv");

            writeLibCSV<GlazingMaterial>(@"C:\Users\Timur\Desktop\GlazingMaterials.csv", lib.GlazingMaterials.ToList());
            List<GlazingMaterial> inGMat = readLibCSV<GlazingMaterial>(@"C:\Users\Timur\Desktop\GlazingMaterials.csv");

            writeLibCSV<ZoneVentilation>(@"C:\Users\Timur\Desktop\ZoneVentilations.csv", lib.ZoneVentilations.ToList());
            List<ZoneVentilation> inZoneVentilation = readLibCSV<ZoneVentilation>(@"C:\Users\Timur\Desktop\ZoneVentilations.csv");

            writeOpaqueConstructionsCSV(@"C:\Users\Timur\Desktop\OpaqueConstructions.csv", lib.OpaqueConstructions.ToList());
            List<OpaqueConstruction> inOpaqueConstructions = readOpaqueConstructionsCSV(@"C:\Users\Timur\Desktop\OpaqueConstructions.csv", inOMat);

            writeLibCSV<ZoneConditioning>(@"C:\Users\Timur\Desktop\ZoneConditioning.csv", lib.ZoneConditionings.ToList());
            List<ZoneConditioning> inZoneConditioning = readLibCSV<ZoneConditioning>(@"C:\Users\Timur\Desktop\ZoneConditioning.csv");


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
