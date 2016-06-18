using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArchsimLib;
using Excel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Excel2JSON
{
    internal static class Parse
    {



        internal static List<T> ObjectsNew<T>(XSSFSheet sh)
        {
            if (sh == null) return null;

            List<T> Objects = new List<T>();


            var header = sh.GetRow(0);



            int i = 1; // skip first row
            while (sh.GetRow(i) != null)
            {

                var row = sh.GetRow(i);
                i++;

                StringBuilder sbCSharp;
                StringBuilder sbJSON;

                ParseClassNew(row, header, out sbJSON, out sbCSharp);




                T mat;
                try
                {
                    mat = Serialization.Deserialize<T>(sbJSON.ToString());
                    Objects.Add(mat);
                }
                catch
                {
                    Debug.WriteLine(sbJSON + "  is not valid");
                }





            }

            return Objects;
        }

        private static void ParseClassNew(IRow row, IRow header, out StringBuilder sbJSON, out StringBuilder sbCSharp)
        {
            sbCSharp = new StringBuilder();
            sbJSON = new StringBuilder();
            sbCSharp.Append("{ ");
            sbJSON.Append("{ ");




            for (int j = 0; j < row.Cells.Count; j++)
            {
                var cell = row.GetCell(j);
                var head = header.GetCell(j);

                if (cell == null || head == null) continue;




                if (cell.CellType == CellType.Numeric)
                {
                    sbCSharp.Append(header.Cells[j].StringCellValue.Trim() + " = " + cell.NumericCellValue);
                    sbJSON.Append("\"" + header.Cells[j].StringCellValue.Trim() + "\"" + " : " + cell.NumericCellValue);
                }



                if (cell.CellType == CellType.Formula)
                {
                    sbCSharp.Append(header.Cells[j].StringCellValue.Trim() + " = " + cell.NumericCellValue);
                    sbJSON.Append("\"" + header.Cells[j].StringCellValue.Trim() + "\"" + " : " + cell.NumericCellValue);
                }



                else if (cell.CellType == CellType.Boolean)
                {
                    sbCSharp.Append(header.Cells[j].StringCellValue.Trim() + " = " + cell.BooleanCellValue.ToString());
                    sbJSON.Append("\"" + header.Cells[j].StringCellValue.Trim() + "\"" + " : " + cell.BooleanCellValue.ToString());
                }
           




                else if (cell.CellType == CellType.String)
                {
                    bool truefalse;

                      // is it a boolean?
                 if (bool.TryParse(cell.StringCellValue, out truefalse))
                    {
                        sbCSharp.Append(header.Cells[j].StringCellValue.Trim() + " = " + cell.StringCellValue.Trim());
                        sbJSON.Append("\"" + header.Cells[j].StringCellValue.Trim() + "\"" + " : " + cell.StringCellValue.Trim());
                    }

                    else
                    {

                        string txt = "";
                        if (!String.IsNullOrWhiteSpace(cell.StringCellValue))
                        {
                            txt = cell.StringCellValue.Trim();
                        }
                        sbCSharp.Append(header.Cells[j].StringCellValue.Trim() + " = \"" + txt + "\"");
                        sbJSON.Append("\"" + header.Cells[j].StringCellValue.Trim() + "\"" + " : \"" + txt + "\"");

                    }
                }






                if (j != row.Cells.Count - 1)
                {
                    sbCSharp.Append(", ");
                    sbJSON.Append(", ");
                }

            }
            sbCSharp.Append(" }");
            sbJSON.Append(" }");

          //  Debug.WriteLine(sbCSharp.ToString());
          //  Debug.WriteLine(sbJSON.ToString());
        }








        internal static List<T> Objects<T>(worksheet worksheet)
        {
            List<T> Objects = new List<T>();

            var header = worksheet.Rows[0];

            for (int i = 0; i < worksheet.Rows.Length; i++)
            {

                if (i == 0)
                {
                    continue;
                }

                var row = worksheet.Rows[i];

                StringBuilder sbCSharp;
                StringBuilder sbJSON;

                ParseClass(row, header, out sbJSON, out sbCSharp);

                T mat;
                try
                {
                    mat = Serialization.Deserialize<T>(sbJSON.ToString());
                    Objects.Add(mat);
                }
                catch
                {
                    Debug.WriteLine(sbJSON + "  is not valid");
                }

            }
            return Objects;
        }

        private static void ParseClass(Row row, Row header, out StringBuilder sbJSON, out StringBuilder sbCSharp)
        {
            sbCSharp = new StringBuilder();
            sbJSON = new StringBuilder();
            sbCSharp.Append("{ ");
            sbJSON.Append("{ ");

            for (int j = 0; j < row.Cells.Length; j++)
            {
                var cell = row.Cells[j];
                var head = header.Cells[j];
                if (cell == null || head == null) continue;

                bool truefalse;
                double number;

                // is it a number?
                if (double.TryParse(cell.Text, out number))
                {
                    sbCSharp.Append(header.Cells[j].Text.Trim() + " = " + number);
                    sbJSON.Append("\"" + header.Cells[j].Text.Trim() + "\"" + " : " + number);
                }
                // is it a boolean?
                else if (bool.TryParse(cell.Text, out truefalse))
                {
                    sbCSharp.Append(header.Cells[j].Text.Trim() + " = " + cell.Text.Trim());
                    sbJSON.Append("\"" + header.Cells[j].Text.Trim() + "\"" + " : " + cell.Text.Trim());
                }
                // or is it a string?
                else
                {
                    string txt = "";
                    if (!String.IsNullOrWhiteSpace(cell.Text))
                    {
                        txt = cell.Text.Trim();
                    }
                    sbCSharp.Append(header.Cells[j].Text.Trim() + " = \"" + txt + "\"");
                    sbJSON.Append("\"" + header.Cells[j].Text.Trim() + "\"" + " : \"" + txt + "\"");
                }
                if (j != row.Cells.Length - 1)
                {
                    sbCSharp.Append(", ");
                    sbJSON.Append(", ");
                }
            }
            sbCSharp.Append(" }");
            sbJSON.Append(" }");

            Debug.WriteLine(sbCSharp.ToString());
            Debug.WriteLine(sbJSON.ToString());
        }


        private static void ParseClass_Old(Row row, Row header, out StringBuilder sbJSON, out StringBuilder sbCSharp)
        {
            sbCSharp = new StringBuilder();
            sbJSON = new StringBuilder();
            sbCSharp.Append("{ ");
            sbJSON.Append("{ ");

            for (int j = 0; j < row.Cells.Length; j++)
            {
                var cell = row.Cells[j];
                var head = header.Cells[j];
                if (cell == null || head == null) continue;

                if (cell.IsAmount)
                {
                    sbCSharp.Append(header.Cells[j].Text.Trim() + " = " + cell.Amount);
                    sbJSON.Append("\"" + header.Cells[j].Text.Trim() + "\"" + " : " + cell.Amount);
                }
                else
                {
                    string txt = "";
                    if (!String.IsNullOrWhiteSpace(cell.Text))
                    {
                        txt = cell.Text.Trim();
                    }
                    sbCSharp.Append(header.Cells[j].Text.Trim() + " = \"" + txt + "\"");
                    sbJSON.Append("\"" + header.Cells[j].Text.Trim() + "\"" + " : \"" + txt + "\"");
                }
                if (j != row.Cells.Length - 1)
                {
                    sbCSharp.Append(", ");
                    sbJSON.Append(", ");
                }
            }
            sbCSharp.Append(" }");
            sbJSON.Append(" }");

            Debug.WriteLine(sbCSharp.ToString());
            Debug.WriteLine(sbJSON.ToString());
        }
    }
}
