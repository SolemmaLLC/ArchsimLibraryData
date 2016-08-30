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
using System.IO;
using System.Globalization;

namespace Excel2JSON
{
    internal static class Parse
    {
        internal static List<T> Objects<T>(IWorkbook wb , string sheetName )
        {
            IFormulaEvaluator formulaEvaluator = WorkbookFactory.CreateFormulaEvaluator(wb);
            ISheet sh = wb.GetSheet(sheetName);
            if (sh == null) return null;


            List<T> Objects = new List<T>();


            var header = sh.GetRow(0);

            for (int i = 1; i < sh.LastRowNum + 1; i++)
            {
                var row = sh.GetRow(i);
                if (row == null) continue;


                StringBuilder sbCSharp;
                StringBuilder sbJSON;
                sbCSharp = new StringBuilder();
                sbJSON = new StringBuilder();
                sbCSharp.Append("{ ");
                sbJSON.Append("{ ");
                //ParseObject(row, header, out sbJSON, out sbCSharp);

                for (int j = 0; j < row.Cells.Count; j++)
                {
                    var cell = row.GetCell(j);
                    var head = header.GetCell(j);

                    if (cell == null || head == null) continue;

                    if (cell.CellType == CellType.Blank) continue;

                    
                    string txt = GetFormattedValue(cell,  formulaEvaluator);
                    txt = Formating.RemoveSpecialCharactersLeaveSpaces(txt.Trim());
                    
                    sbCSharp.Append(header.Cells[j].StringCellValue.Trim() + " = \"" + txt + "\"");
                    sbJSON.Append("\"" + header.Cells[j].StringCellValue.Trim() + "\"" + " : \"" + txt + "\"");

                    if (j != row.Cells.Count - 1)
                    {
                        sbCSharp.Append(", ");
                        sbJSON.Append(", ");
                    }

                }
                sbCSharp.Append(" }");
                sbJSON.Append(" }");


                T mat;
                try
                {
                    mat = Serialization.Deserialize<T>(sbJSON.ToString());
                    Objects.Add(mat);
                }
                catch
                {
                    Debug.WriteLine(sh.SheetName + " Row " + i + " " + sbJSON + "  is not valid");
                }
            }

            return Objects;
        }
       



        //internal static List<T> ObjectsOld<T>(XSSFSheet sh)
        //{
        //    if (sh == null) return null;

        //    List<T> Objects = new List<T>();


        //    var header = sh.GetRow(0);

        //    for (int i = 1; i < sh.LastRowNum + 1; i++)
        //    {
        //        var row = sh.GetRow(i);
        //        if (row == null) continue;


        //        StringBuilder sbCSharp;
        //        StringBuilder sbJSON;

        //        ParseObject(row, header, out sbJSON, out sbCSharp);




        //        T mat;
        //        try
        //        {
        //            mat = Serialization.Deserialize<T>(sbJSON.ToString());
        //            Objects.Add(mat);
        //        }
        //        catch
        //        {
        //            Debug.WriteLine(sh.SheetName + " Row " + i + " " + sbJSON + "  is not valid");
        //        }
        //    }

        //    return Objects;
        //}
        //private static void ParseObject(IRow row, IRow header, out StringBuilder sbJSON, out StringBuilder sbCSharp)
        //{
        //    sbCSharp = new StringBuilder();
        //    sbJSON = new StringBuilder();
        //    sbCSharp.Append("{ ");
        //    sbJSON.Append("{ ");




        //    for (int j = 0; j < row.Cells.Count; j++)
        //    {
        //        var cell = row.GetCell(j);
        //        var head = header.GetCell(j);

        //        if (cell == null || head == null) continue;

        //        if (cell.CellType == CellType.Blank) continue;


        //        if (cell.CellType == CellType.Numeric)
        //        {
        //            sbCSharp.Append(header.Cells[j].StringCellValue.Trim() + " = " + cell.NumericCellValue);
        //            sbJSON.Append("\"" + header.Cells[j].StringCellValue.Trim() + "\"" + " : " + cell.NumericCellValue);
        //        }



        //        if (cell.CellType == CellType.Formula)
        //        {
        //            //numeric formula
        //            try
        //            {
        //                sbCSharp.Append(header.Cells[j].StringCellValue.Trim() + " = " + cell.NumericCellValue);
        //                sbJSON.Append("\"" + header.Cells[j].StringCellValue.Trim() + "\"" + " : " + cell.NumericCellValue);
        //            }
        //            //string formula
        //            catch
        //            {
        //                sbCSharp.Append(header.Cells[j].StringCellValue.Trim() + " = " + cell.StringCellValue);
        //                sbJSON.Append("\"" + header.Cells[j].StringCellValue.Trim() + "\"" + " : " + cell.StringCellValue);
        //            }
        //        }



        //        else if (cell.CellType == CellType.Boolean)
        //        {
        //            sbCSharp.Append(header.Cells[j].StringCellValue.Trim() + " = " + cell.BooleanCellValue.ToString().ToLower());
        //            sbJSON.Append("\"" + header.Cells[j].StringCellValue.Trim() + "\"" + " : " + cell.BooleanCellValue.ToString().ToLower());
        //        }





        //        else if (cell.CellType == CellType.String)
        //        {
        //            bool truefalse;

        //            // is it a boolean?
        //            if (bool.TryParse(cell.StringCellValue, out truefalse))
        //            {
        //                sbCSharp.Append(header.Cells[j].StringCellValue.Trim() + " = " + cell.StringCellValue.Trim());
        //                sbJSON.Append("\"" + header.Cells[j].StringCellValue.Trim() + "\"" + " : " + cell.StringCellValue.Trim());
        //            }

        //            else
        //            {
        //                string txt = "";
        //                if (!String.IsNullOrWhiteSpace(cell.StringCellValue))
        //                {
        //                    txt = Formating.RemoveSpecialCharactersLeaveSpaces(cell.StringCellValue.Trim());
        //                }
        //                sbCSharp.Append(header.Cells[j].StringCellValue.Trim() + " = \"" + txt + "\"");
        //                sbJSON.Append("\"" + header.Cells[j].StringCellValue.Trim() + "\"" + " : \"" + txt + "\"");

        //            }
        //        }






        //        if (j != row.Cells.Count - 1)
        //        {
        //            sbCSharp.Append(", ");
        //            sbJSON.Append(", ");
        //        }

        //    }
        //    sbCSharp.Append(" }");
        //    sbJSON.Append(" }");

        //    //  Debug.WriteLine(sbCSharp.ToString());
        //    //  Debug.WriteLine(sbJSON.ToString());
        //}




        internal static List<OpaqueConstruction> Constructions(IWorkbook wb, string sheetName, ref Library lib)
        {
            IFormulaEvaluator formulaEvaluator = WorkbookFactory.CreateFormulaEvaluator(wb);
            ISheet sh = wb.GetSheet(sheetName);
            if (sh == null) return null;

            List<OpaqueConstruction> Objects = new List<OpaqueConstruction>();

            var header = sh.GetRow(0);

            for (int i = 1; i < sh.LastRowNum + 1; i++)
            {
                var row = sh.GetRow(i);
                if (row == null) continue;

                try
                {
                    OpaqueConstruction mat;
                    mat = ParseConstruction(row, header, ref lib);
                    if (mat != null) { Objects.Add(mat); }
                }
                catch (Exception e)
                {
                    Debug.WriteLine("ERROR: " + sh.SheetName + " Row " + i + "  is not valid " + e.Message);
                    Logger.WriteLine("ERROR: " + sh.SheetName + " Row " + i + "  is not valid " + e.Message);
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
                string headVal = "";

                if (head != null)
                {
                    if (head.CellType == CellType.String) headVal = head.StringCellValue.Trim().ToLower();
                }



                if (cell == null) continue;

                if (cell.CellType == CellType.Blank) continue;


                if (headVal == "name")
                {
                    name = cell.StringCellValue.Trim();
                }
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
                        cnames.Add(Formating.RemoveSpecialCharactersLeaveSpaces(cell.StringCellValue.Trim()));
                    }

                    else if (cell.CellType == CellType.Formula)
                    {
                        if (cell.CachedFormulaResultType == CellType.Numeric)
                        {
                            cthick.Add(cell.NumericCellValue);
                        }
                        else if (cell.CachedFormulaResultType == CellType.String)
                        {
                            cnames.Add(Formating.RemoveSpecialCharactersLeaveSpaces(cell.StringCellValue.Trim()));
                        }
                    }
                }

            }

            if (String.IsNullOrWhiteSpace(name)) return null;

            var c = OpaqueConstruction.QuickConstruction(name, type, cnames.ToArray(), cthick.ToArray(), category, source, ref lib);

            return c;

        }

        internal static List<ZoneDefinition> Zone(IWorkbook wb, string sheetName, ref Library lib)
        {
            IFormulaEvaluator formulaEvaluator = WorkbookFactory.CreateFormulaEvaluator(wb);
            ISheet sh = wb.GetSheet(sheetName);
            if (sh == null) return null;

            List<ZoneDefinition> Objects = new List<ZoneDefinition>();


            var header = sh.GetRow(0);



            for (int i = 1; i < sh.LastRowNum + 1; i++)
            {
                var row = sh.GetRow(i);
                if (row == null) continue;


                try
                {
                    ZoneDefinition mat;
                    mat = ParseZone(row, header, ref lib);
                    Objects.Add(mat);
                }
                catch (Exception e)
                {
                    Debug.WriteLine("ERROR: " + sh.SheetName + " Row " + i + "  is not valid " + e.Message);
                    Logger.WriteLine("ERROR: " + sh.SheetName + " Row " + i + "  is not valid " + e.Message);
                }

            }

            return Objects;
        }
        private static ZoneDefinition ParseZone(IRow row, IRow header, ref Library lib)
        {


            string name = "";
            var c = new ZoneDefinition();

            for (int j = 0; j < row.Cells.Count; j++)
            {

                var cell = row.GetCell(j);
                var head = header.GetCell(j);
                string headVal = "";

                if (head != null)
                {
                    if (head.CellType == CellType.String) headVal = head.StringCellValue.Trim().ToLower();
                }


                if (cell == null) continue;

                if (cell.CellType == CellType.Blank) continue;


                if (headVal == "name")
                {
                    name = cell.StringCellValue.Trim();
                    c.Name = name;
                }
                else if (headVal == "zoneload")
                {
                    string lookup = cell.StringCellValue.Trim();
                    try
                    {
                        var setting = lib.ZoneLoads.First(x => x.Name == lookup);
                        if (setting != null)
                        {
                            c.Loads = setting;
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("ERROR: " + lookup + "  does not exsist in Library " + e.Message);
                        Logger.WriteLine("ERROR: " + lookup + "  does not exsist in Library " + e.Message);
                    }
                }
                else if (headVal == "zoneconditioning")
                {
                    string lookup = cell.StringCellValue.Trim();
                    try
                    {
                        var setting = lib.ZoneConditionings.First(x => x.Name == lookup);
                        if (setting != null)
                        {
                            c.Conditioning = setting;
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("ERROR: " + lookup + "  does not exsist in Library " + e.Message);
                        Logger.WriteLine("ERROR: " + lookup + "  does not exsist in Library " + e.Message);
                    }
                }
                else if (headVal == "ventilation")
                {
                    string lookup = cell.StringCellValue.Trim();
                    try
                    {
                        var setting = lib.ZoneVentilations.First(x => x.Name == lookup);
                        if (setting != null)
                        {
                            c.Ventilation = setting;
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("ERROR: " + lookup + "  does not exsist in Library " + e.Message);
                        Logger.WriteLine("ERROR: " + lookup + "  does not exsist in Library " + e.Message);
                    }
                }
                else if (headVal == "domhotwater")
                {
                    string lookup = cell.StringCellValue.Trim();
                    try
                    {
                        var setting = lib.DomHotWaters.First(x => x.Name == lookup);
                        if (setting != null)
                        {
                            c.DomHotWater = setting;
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("ERROR: " + lookup + "  does not exsist in Library " + e.Message);
                        Logger.WriteLine("ERROR: " + lookup + "  does not exsist in Library " + e.Message);
                    }
                }
                else if (headVal == "zoneconstruction")
                {
                    string lookup = cell.StringCellValue.Trim();
                    try
                    {
                        var setting = lib.ZoneConstructions.First(x => x.Name == lookup);
                        if (setting != null)
                        {
                            c.Materials = setting;
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("ERROR: " + lookup + "  does not exsist in Library " + e.Message);
                        Logger.WriteLine("ERROR: " + lookup + "  does not exsist in Library " + e.Message);
                    }
                }





            }






            return c;

        }

        internal static List<YearSchedule> Schedule(IWorkbook wb, string sheetName, ref Library lib)
        {
            IFormulaEvaluator formulaEvaluator = WorkbookFactory.CreateFormulaEvaluator(wb);
            ISheet sh = wb.GetSheet(sheetName);
            if (sh == null) return null;

            List<YearSchedule> Objects = new List<YearSchedule>();


            var header = sh.GetRow(0);



            for (int i = 1; i < sh.LastRowNum + 1; i++)
            {
                var row = sh.GetRow(i);
                if (row == null) continue;


                try
                {
                    YearSchedule mat;
                    mat = ParseSchedule(row, header, ref lib);
                    if (mat != null) { Objects.Add(mat); }


                }
                catch (Exception e)
                {
                    Debug.WriteLine("ERROR: " + sh.SheetName + " Row " + i + "  is not valid " + e.Message);
                    Logger.WriteLine("ERROR: " + sh.SheetName + " Row " + i + "  is not valid " + e.Message);
                }

            }

            return Objects;
        }
        private static YearSchedule ParseSchedule(IRow row, IRow header, ref Library lib)
        {



            string name = "";
            //string comment = "";
            string source = "";
            string category = "";

            List<double> values = new List<double>();



            for (int j = 0; j < row.Cells.Count; j++)
            {
                var cell = row.GetCell(j);
                var head = header.GetCell(j);
                string headVal = "";

                if (head != null)
                {

                    if (head.CellType == CellType.String) headVal = head.StringCellValue.Trim().ToLower();
                }



                if (cell == null) continue;

                if (cell.CellType == CellType.Blank) continue;


                if (headVal == "name")
                {
                    name = cell.StringCellValue.Trim();
                }
                else if (headVal == "source")
                {
                    source = cell.StringCellValue.Trim();
                }
                else if (headVal == "category")
                {
                    category = cell.StringCellValue.Trim();
                }


                else
                {

                    if (cell.CellType == CellType.Numeric)
                    {
                        values.Add(cell.NumericCellValue);
                    }

                    else if (cell.CellType == CellType.Formula)
                    {
                        if (cell.CachedFormulaResultType == CellType.Numeric)
                        {
                            values.Add(cell.NumericCellValue);
                        }

                    }
                }

            }

            var first = values.Take(24);
            var second = values.Skip(24).Take(24);


            var sched = YearSchedule.QuickSchedule(name, first.ToArray(), second.ToArray(), category, source, ref lib);

            return sched;

        }

        internal static List<ScheduleArray> ArraySchedule(IWorkbook wb, string sheetName, ref Library lib)
        {
            IFormulaEvaluator formulaEvaluator = WorkbookFactory.CreateFormulaEvaluator(wb);
            ISheet sh = wb.GetSheet(sheetName);
            if (sh == null) return null;

            List<ScheduleArray> Objects = new List<ScheduleArray>();


            var header = sh.GetRow(0);
            for (int j = 0; j < header.Cells.Count; j++)
            {
                var head = header.GetCell(j);
                string headVal = "";

                if (head != null)
                {

                    if (head.CellType == CellType.String) headVal = head.StringCellValue.Trim().ToLower();
                }


                var sched = new ScheduleArray();
                sched.Values = new double[8760];
                sched.Name = headVal;
                Objects.Add(sched);
            }


            for (int i = 1; i < sh.LastRowNum + 1; i++)
            {
                var row = sh.GetRow(i);
                if (row == null) continue;

                for (int j = 0; j < row.Cells.Count; j++)
                {
                    var cell = row.GetCell(j);

                    if (cell == null) continue;



                    if (cell.CellType == CellType.Numeric)
                    {
                        Objects[j].Values[i-1] = cell.NumericCellValue;
                    }

                    else if (cell.CellType == CellType.Formula)
                    {
                        if (cell.CachedFormulaResultType == CellType.Numeric)
                        {
                            Objects[j].Values[i-1] = cell.NumericCellValue;
                        }
                    }


                }



            }

            return Objects;
        }
















        private static string GetFormattedValue(ICell cell, IFormulaEvaluator formulaEvaluator)
        {
            DataFormatter dataFormatter = new DataFormatter(CultureInfo.InvariantCulture);
            string returnValue = string.Empty;
            if (cell != null)
            {
                try
                {
                    // Get evaluated and formatted cell value
                    returnValue = dataFormatter.FormatCellValue(cell, formulaEvaluator);
                }
                catch
                {
                    // When failed in evaluating the formula, use stored values instead...
                    // and set cell value for reference from formulae in other cells...
                    if (cell.CellType == CellType.Formula)
                    {
                        switch (cell.CachedFormulaResultType)
                        {
                            case CellType.String:
                                returnValue = cell.StringCellValue;
                                cell.SetCellValue(cell.StringCellValue);
                                break;
                            case CellType.Numeric:
                                returnValue = dataFormatter.FormatRawCellContents
                                (cell.NumericCellValue, 0, cell.CellStyle.GetDataFormatString());
                                cell.SetCellValue(cell.NumericCellValue);
                                break;
                            case CellType.Boolean:
                                returnValue = cell.BooleanCellValue.ToString().ToLower();
                                cell.SetCellValue(cell.BooleanCellValue);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            return (returnValue ?? string.Empty).Trim();
        }
    }

}
