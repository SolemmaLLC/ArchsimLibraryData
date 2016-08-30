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




            var lib = ParseLib.Excel2Lib(file);

            string JsonPath = System.IO.Path.GetDirectoryName(file);
            string JsonName = System.IO.Path.GetFileNameWithoutExtension(file) + ".json" ;
            string JsonFile = System.IO.Path.Combine(JsonPath, JsonName);


            Logger.WriteLine("Finished... writing JSON library to "+ JsonFile);

            loggerBox.Text = Logger.log.ToString();

           File.WriteAllText(JsonFile, lib.toJSON());

        }

       
    }
}
