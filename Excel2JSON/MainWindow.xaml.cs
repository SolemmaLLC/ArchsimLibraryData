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




            foreach (var worksheet in Workbook.Worksheets(@"C:\temp\160603_ExcelLibraryEditor.xlsx"))
            {

                foreach (var row in worksheet.Rows)
                {


                    foreach (var cell in row.Cells)
                    {



                        Debug.WriteLine(cell.Text +" "+ cell.Amount);



                    }

                }


            }




        }
    }
}
