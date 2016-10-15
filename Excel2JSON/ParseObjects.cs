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
    internal class Parse
    {
        IWorkbook wb;
        IFormulaEvaluator formulaEvaluator;
        DataFormatter dataFormatter;
        Library lib;

        internal Parse(IWorkbook _wb, ref Library _lib) {
            wb = _wb;
            lib = _lib;
            formulaEvaluator = WorkbookFactory.CreateFormulaEvaluator(wb);
            dataFormatter = new DataFormatter(CultureInfo.InvariantCulture);
        }

        internal List<T> Objects<T>(string sheetName)
        {
    
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

                    
                    string txt = GetFormattedValue(cell);
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


                Debug.WriteLine(sbCSharp.ToString());


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
       


        internal List<OpaqueConstruction> Constructions(string sheetName)
        {
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
                    mat = ParseConstruction(row, header);
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
        private  OpaqueConstruction ParseConstruction(IRow row, IRow header)
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
                    headVal = GetFormattedValue(head).ToLower();
                }



                if (cell == null) continue;

                if (cell.CellType == CellType.Blank) continue;


                if (headVal == "name")
                {
                    name = GetFormattedValue(cell);
                }
                else if (headVal == "source")
                {
                    source = GetFormattedValue(cell);
                }
                else if (headVal == "category")
                {
                    category = GetFormattedValue(cell);
                }
                else if (headVal == "type")
                {
                    ConstructionTypes ct = ConstructionTypes.Facade;
                    if (ConstructionTypes.TryParse(GetFormattedValue(cell), out ct))
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
                        cnames.Add(Formating.RemoveSpecialCharactersLeaveSpaces(GetFormattedValue(cell)));
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

        internal List<ZoneDefinition> Zone(string sheetName)
        {
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
                    mat = ParseZone(row, header);
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
        private ZoneDefinition ParseZone(IRow row, IRow header)
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
                    headVal = GetFormattedValue(head).ToLower();
                }


                if (cell == null) continue;

                if (cell.CellType == CellType.Blank) continue;


                if (headVal == "name")
                {
                    name = GetFormattedValue(cell);
                    c.Name = name;
                }
                else if (headVal == "zoneload")
                {
                    string lookup = GetFormattedValue(cell);
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
                    string lookup = GetFormattedValue(cell);
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
                    string lookup = GetFormattedValue(cell);
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
                    string lookup = GetFormattedValue(cell);
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
                    string lookup = GetFormattedValue(cell);
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

        internal  List<YearSchedule> Schedule( string sheetName)
        {
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
                    mat = ParseSchedule(row, header);
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
        private  YearSchedule ParseSchedule(IRow row, IRow header)
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

                    headVal = GetFormattedValue(head).ToLower();
                }



                if (cell == null) continue;

                if (cell.CellType == CellType.Blank) continue;


                if (headVal == "name")
                {
                    name = GetFormattedValue(cell);
                }
                else if (headVal == "source")
                {
                    source = GetFormattedValue(cell);
                }
                else if (headVal == "category")
                {
                    category = GetFormattedValue(cell);
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

        internal  List<ScheduleArray> ArraySchedule( string sheetName)
        {
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

                    headVal = GetFormattedValue(head).ToLower();
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
















        private  string GetFormattedValue(ICell cell)
        {

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
