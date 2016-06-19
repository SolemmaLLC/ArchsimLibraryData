using System;
using System.CodeDom;
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



        internal static List<T> Objects<T>(XSSFSheet sh)
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

                ParseObject(row, header, out sbJSON, out sbCSharp);




                T mat;
                try
                {
                    mat = Serialization.Deserialize<T>(sbJSON.ToString());
                    Objects.Add(mat);
                }
                catch
                {
                    Debug.WriteLine(sh.SheetName + " Row " + i + " "+ sbJSON + "  is not valid");
                }





            }

            return Objects;
        }



        private static void ParseObject(IRow row, IRow header, out StringBuilder sbJSON, out StringBuilder sbCSharp)
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






        internal static List<OpaqueConstruction> Constructions(XSSFSheet sh, ref Library lib)
        {
            if (sh == null) return null;

            List<OpaqueConstruction> Objects = new List<OpaqueConstruction>();


            var header = sh.GetRow(0);



            int i = 1; // skip first row
            while (sh.GetRow(i) != null)
            {

                var row = sh.GetRow(i);
                i++;


                try
                {
                    OpaqueConstruction mat;
                    mat = ParseConstruction(row, header, ref lib);
                    Objects.Add(mat);
                }
                catch(Exception e)
                {
                    Debug.WriteLine(sh.SheetName + " Row " + i + "  is not valid " + e.Message);
                }

            }

            return Objects;
        }

        private static OpaqueConstruction ParseConstruction(IRow row, IRow header, ref Library lib)
        {


            string name = "";
            //string comment = "";
            string source = "";
            string category = "";
            ConstructionTypes type = ConstructionTypes.Facade;

            List<string> cnames = new List<string>();
            List<double> cthick = new List<double>();

            for (int j = 0; j < row.Cells.Count; j++)
            {
                var cell = row.GetCell(j);
                var head = header.GetCell(j);
                string headVal ="";
                    
                 if(head!=null) headVal   = head.StringCellValue.Trim().ToLower();


                if (cell != null)
                {

                    
                    if (headVal == "name")
                    {
                        name = cell.StringCellValue.Trim();
                    }
                    //else if (headVal == "comment")
                    //{
                    //    comment = cell.StringCellValue.Trim();
                    //}
                    else if (headVal == "source")
                    {
                        source = cell.StringCellValue.Trim();
                    }
                    else if (headVal == "category")
                    {
                        category = cell.StringCellValue.Trim();
                    }
                    else if (headVal == "type")
                    {
                        ConstructionTypes ct = ConstructionTypes.Facade;
                        if (ConstructionTypes.TryParse(cell.StringCellValue, out ct))
                        {
                            type = ct;
                        }
                    }

                    else
                    {

                        if (cell.CellType == CellType.Numeric)
                        {
                        
                            cthick.Add(cell.NumericCellValue);
                        }

                        else if (cell.CellType == CellType.String)
                        {
                            cnames.Add(cell.StringCellValue.Trim());
                        }

                        if ( cell.CellType == CellType.Formula)
                        {

                            try
                            {
                                cthick.Add(cell.NumericCellValue);
                            }
                            catch
                            {
                                
                            }
                            try
                            {
                                cnames.Add(cell.StringCellValue.Trim());
                            }
                            catch
                            {
                               
                            }
                        }

                    }
                }

            }


            var c = OpaqueConstruction.QuickConstruction(name, type, cnames.ToArray(), cthick.ToArray(), category, source, ref lib);

            return c;

        }




    }
}
