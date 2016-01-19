using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace ArchsimLib
{
    /// <summary>
    /// Library that holds all materials, constructions and schedules during runtime.
    /// </summary>     
    public class Library
    {

        #region Add Low Level Objects

        public OpaqueConstruction Add(OpaqueConstruction obj)
        {
            if (obj == null) return null;

            if (!OpaqueConstructions.Any(i => i.Name == obj.Name))
            {

                //try
                //{
                //    foreach (var o in obj.Layers.Select(a => a.Material)) this.Add(o);
                //}
                //catch (Exception ex) { }

                OpaqueConstructions.Add(obj);

                return obj;
            }
            else
            {
                var oc = OpaqueConstructions.Single(o => o.Name == obj.Name);

                //try
                //{
                //    if (oc != null)
                //    {
                //        foreach (var o in obj.Layers.Select(a => a.Material)) this.Add(o);
                //    }
                //}
                //catch (Exception ex) { }

                CopyObjectData(obj, oc, "", BindingFlags.Public | BindingFlags.Instance);
                
                return oc;
            }
        }
        public GlazingConstruction Add(GlazingConstruction obj)
        {
            if (obj == null) return null;

            if (!GlazingConstructions.Any(i => i.Name == obj.Name))
            {

                //try
                //{

                //    foreach (var o in obj.Layers.Select(a => a.GlassMaterial)) this.Add(o);
                //}
                //catch (Exception ex) { }

                GlazingConstructions.Add(obj);

              

                return obj;
            }
            else
            {
                var oc = GlazingConstructions.Single(o => o.Name == obj.Name);

                //try
                //{
                //    if (oc != null)
                //    {
                //        foreach (var o in obj.Layers.Select(a => a.GlassMaterial)) this.Add(o);
                //    }
                //}
                //catch (Exception ex) { }

                CopyObjectData(obj, oc, "", BindingFlags.Public | BindingFlags.Instance);

                //foreach (var l in obj.Layers.Select(o => o.GlassMaterial)) Add(l);

                return oc;
            }
        }
        public OpaqueMaterial Add(OpaqueMaterial obj)
        {
            if (obj == null) return null;

            if (!OpaqueMaterials.Any(i => i.Name == obj.Name))
            {
                OpaqueMaterials.Add(obj);
                return obj;
            }
            else
            {
                var oc = OpaqueMaterials.Single(o => o.Name == obj.Name);
                CopyObjectData(obj, oc, "", BindingFlags.Public | BindingFlags.Instance);
                return oc;
            }
        }
        public GlazingMaterial Add(GlazingMaterial obj)
        {
            if (obj == null) return null;

            if (!GlazingMaterials.Any(i => i.Name == obj.Name))
            {
                GlazingMaterials.Add(obj);
                return obj;
            }
            else
            {
                var oc = GlazingMaterials.Single(o => o.Name == obj.Name);
                CopyObjectData(obj, oc, "", BindingFlags.Public | BindingFlags.Instance);
                return oc;
            }
        }
        public GlazingConstructionSimple Add(GlazingConstructionSimple obj)
        {

            if (obj == null) return null;

            if (!GlazingConstructionsSimple.Any(i => i.Name == obj.Name))
            {
                GlazingConstructionsSimple.Add(obj);
                return obj;
            }
            else
            {
                var oc = GlazingConstructionsSimple.Single(o => o.Name == obj.Name);
                CopyObjectData(obj, oc, "", BindingFlags.Public | BindingFlags.Instance);
                return oc;

            }
        }
        public GasMaterial Add(GasMaterial obj)
        {
            if (obj == null) return null;

            if (!GasMaterials.Any(i => i.Name == obj.Name))
            {
                GasMaterials.Add(obj);
                return obj;
            }
            else
            {
                var oc = GasMaterials.Single(o => o.Name == obj.Name);
                CopyObjectData(obj, oc, "", BindingFlags.Public | BindingFlags.Instance);
                return oc;
            }
        }
        public DaySchedule Add(DaySchedule obj)
        {
            if (obj == null) return null;

            if (!DaySchedules.Any(i => i.Name == obj.Name))
            {
                DaySchedules.Add(obj);
                return obj;
            }
            else
            {
                var oc = DaySchedules.Single(o => o.Name == obj.Name);
                CopyObjectData(obj, oc, "", BindingFlags.Public | BindingFlags.Instance);
                return oc;
            }
        }
        public WeekSchedule Add(WeekSchedule obj)
        {
            if (obj == null) return null;

            if (!WeekSchedules.Any(i => i.Name == obj.Name))
            {
                //try
                //{
                //    foreach (var o in obj.Days.Select(a => a)) this.Add(o);
                //}
                //catch (Exception ex) { }

                WeekSchedules.Add(obj);

                //foreach (var dsched in obj.Days) Add(dsched);

                return obj;
            }
            else
            {
                var oc = WeekSchedules.Single(o => o.Name == obj.Name);

                //try
                //{
                //    if (oc != null)
                //    {
                //        foreach (var o in obj.Days.Select(a => a)) this.Add(o);
                //    }
                //}
                //catch (Exception ex) { }

                CopyObjectData(obj, oc, "", BindingFlags.Public | BindingFlags.Instance);

                //foreach (var dsched in obj.Days) Add(dsched);

                return oc;
            }
        }
        public YearSchedule Add(YearSchedule obj)
        {
            if (obj == null) return null;

            if (!YearSchedules.Any(i => i.Name == obj.Name))
            {
                //try
                //{
                //    foreach (var o in obj.WeekSchedules.Select(a => a)) this.Add(o);
                //}
                //catch (Exception ex) { }


                YearSchedules.Add(obj);

                 //foreach (var ws in obj.WeekSchedules) Add(ws);

                return obj;
            }
            else
            {
                var oc = YearSchedules.Single(o => o.Name == obj.Name);
               
                //try
                //{
                //    if (oc != null)
                //    {
                //        foreach (var o in obj.WeekSchedules.Select(a => a)) this.Add(o);
                //    }
                //}
                //catch (Exception ex) { }

                CopyObjectData(obj, oc, "", BindingFlags.Public | BindingFlags.Instance);

                //foreach (var ws in obj.WeekSchedules) Add(ws);

                return oc;
            }
        }
        public ScheduleArray Add(ScheduleArray obj)
        {
            if (obj == null) return null;

            if (!ArraySchedules.Any(i => i.Name == obj.Name))
            {
                ArraySchedules.Add(obj);
                return obj;
            }
            else
            {
                var oc = ArraySchedules.Single(o => o.Name == obj.Name);
                CopyObjectData(obj, oc, "", BindingFlags.Public | BindingFlags.Instance);
                return oc;
            }
        }

        #endregion

        #region Add High Level Objects

        public ZoneLoad Add(ZoneLoad obj)
        {
            if (obj == null) return null;

            if (!ZoneLoads.Any(i => i.Name == obj.Name))
            {
                ZoneLoads.Add(obj);
                return obj;
            }
            else
            {
                var oc = ZoneLoads.Single(o => o.Name == obj.Name);
                CopyObjectData(obj, oc, "", BindingFlags.Public | BindingFlags.Instance);
                return oc;
            }

        }

        public ZoneVentilation Add(ZoneVentilation obj)
        {
            if (obj == null) return null;

            if (!ZoneVentilations.Any(i => i.Name == obj.Name))
            {
                ZoneVentilations.Add(obj);
                return obj;
            }
            else
            {
                var oc = ZoneVentilations.Single(o => o.Name == obj.Name);
                CopyObjectData(obj, oc, "", BindingFlags.Public | BindingFlags.Instance);
                return oc;
            }

        }


        public ZoneConstruction Add(ZoneConstruction obj)
        {
            if (obj == null) return null;

            if (!ZoneConstructions.Any(i => i.Name == obj.Name))
            {
                ZoneConstructions.Add(obj);
                return obj;
            }
            else
            {
                var oc = ZoneConstructions.Single(o => o.Name == obj.Name);
                CopyObjectData(obj, oc, "", BindingFlags.Public | BindingFlags.Instance);
                return oc;
            }

        }

        public ZoneConditioning Add(ZoneConditioning obj)
        {
            if (obj == null) return null;

            if (!ZoneConditionings.Any(i => i.Name == obj.Name))
            {
                ZoneConditionings.Add(obj);
                return obj;
            }
            else
            {
                var oc = ZoneConditionings.Single(o => o.Name == obj.Name);
                CopyObjectData(obj, oc, "", BindingFlags.Public | BindingFlags.Instance);
                return oc;
            }

        }


        public DomHotWater Add(DomHotWater obj)
        {
            if (obj == null) return null;

            if (!DomHotWaters.Any(i => i.Name == obj.Name))
            {
                DomHotWaters.Add(obj);
                return obj;
            }
            else
            {
                var oc = DomHotWaters.Single(o => o.Name == obj.Name);
                CopyObjectData(obj, oc, "", BindingFlags.Public | BindingFlags.Instance);
                return oc;
            }

        }


        public ZoneDefinition Add(ZoneDefinition obj) {
            if (obj == null) return null;

            if (!ZoneDefinitions.Any(i => i.Name == obj.Name))
            {
                ZoneDefinitions.Add(obj);
                return obj;
            }
            else
            {
                var oc = ZoneDefinitions.Single(o => o.Name == obj.Name);
                CopyObjectData(obj, oc, "", BindingFlags.Public | BindingFlags.Instance);
                return oc;
            }

        }
        #endregion

        public T getElementByName<T>(string name)
        {
            // materials and constructions

            if (typeof(T) == typeof(OpaqueConstruction))
            {
                return (T)Convert.ChangeType(OpaqueConstructions.Single(o => o.Name == name), typeof(T));
            }
            else if (typeof(T) == typeof(GlazingConstruction))
            {
                return (T)Convert.ChangeType(GlazingConstructions.Single(o => o.Name == name), typeof(T));
            }
            else if (typeof(T) == typeof(OpaqueMaterial))
            {
                return (T)Convert.ChangeType(OpaqueMaterials.Single(o => o.Name == name), typeof(T));
            }
            else if (typeof(T) == typeof(GlazingMaterial))
            {
                return (T)Convert.ChangeType(GlazingMaterials.Single(o => o.Name == name), typeof(T));
            }
            else if (typeof(T) == typeof(GasMaterial))
            {
                return (T)Convert.ChangeType(GasMaterials.Single(o => o.Name == name), typeof(T));
            }
            else if (typeof(T) == typeof(GlazingConstructionSimple))
            {
                return (T)Convert.ChangeType(GlazingConstructionsSimple.Single(o => o.Name == name), typeof(T));
            }

            // schedules

            else if (typeof(T) == typeof(DaySchedule))
            {
                return (T)Convert.ChangeType(DaySchedules.Single(o => o.Name == name), typeof(T));
            }
            else if (typeof(T) == typeof(WeekSchedule))
            {
                return (T)Convert.ChangeType(WeekSchedules.Single(o => o.Name == name), typeof(T));
            }
            else if (typeof(T) == typeof(YearSchedule))
            {
                return (T)Convert.ChangeType(YearSchedules.Single(o => o.Name == name), typeof(T));
            }

            else if (typeof(T) == typeof(ScheduleArray))
            {
                return (T)Convert.ChangeType(ArraySchedules.Single(o => o.Name == name), typeof(T));
            }

            // zone def

            else if (typeof(T) == typeof(ZoneLoad))
            {
                return (T)Convert.ChangeType(ZoneLoads.Single(o => o.Name == name), typeof(T));
            }
            else if (typeof(T) == typeof(ZoneVentilation))
            {
                return (T)Convert.ChangeType(ZoneVentilations.Single(o => o.Name == name), typeof(T));
            }
            else if (typeof(T) == typeof(ZoneConstruction))
            {
                return (T)Convert.ChangeType(ZoneConstructions.Single(o => o.Name == name), typeof(T));
            }
            else if (typeof(T) == typeof(ZoneConditioning))
            {
                return (T)Convert.ChangeType(ZoneConditionings.Single(o => o.Name == name), typeof(T));
            }
            else if (typeof(T) == typeof(DomHotWater))
            {
                return (T)Convert.ChangeType(DomHotWaters.Single(o => o.Name == name), typeof(T));
            }
            else if (typeof(T) == typeof(ZoneDefinition))
            {
                return (T)Convert.ChangeType(ZoneDefinitions.Single(o => o.Name == name), typeof(T));
            }
       

            // dont know what this is???

            else return (T)Convert.ChangeType(null, typeof(T));

        }

        public string Version;
        public DateTime TimeStamp;

        //low level objects
        public IList<OpaqueMaterial> OpaqueMaterials;
        public IList<GlazingMaterial> GlazingMaterials;
        public IList<GasMaterial> GasMaterials;
        public IList<OpaqueConstruction> OpaqueConstructions;
        public IList<GlazingConstruction> GlazingConstructions;
        public IList<GlazingConstructionSimple> GlazingConstructionsSimple;
        public IList<DaySchedule> DaySchedules;
        public IList<WeekSchedule> WeekSchedules;
        public IList<YearSchedule> YearSchedules;
        public IList<ScheduleArray> ArraySchedules;
        
        //zone definitions
        public IList<ZoneLoad> ZoneLoads;
        public IList<ZoneVentilation> ZoneVentilations;
        public IList<ZoneConstruction> ZoneConstructions;
        public IList<ZoneConditioning> ZoneConditionings;
        public IList<DomHotWater> DomHotWaters;
        public IList<ZoneDefinition> ZoneDefinitions;




        public Library()
        {
            Version = Utilities.AssemblyVersion;
            TimeStamp = DateTime.Now;

            OpaqueMaterials = new List<OpaqueMaterial>();
            GlazingMaterials = new List<GlazingMaterial>();
            GasMaterials = new List<GasMaterial>();
            OpaqueConstructions = new List<OpaqueConstruction>();
            GlazingConstructions = new List<GlazingConstruction>();
            GlazingConstructionsSimple = new List<GlazingConstructionSimple>();
            DaySchedules = new List<DaySchedule>();
            WeekSchedules = new List<WeekSchedule>();
            YearSchedules = new List<YearSchedule>();
            ArraySchedules = new List<ScheduleArray>();

            ZoneLoads = new List<ZoneLoad>();
            ZoneVentilations = new List<ZoneVentilation>();
            ZoneConstructions = new List<ZoneConstruction>();
            ZoneConditionings = new List<ZoneConditioning>();
            DomHotWaters = new List<DomHotWater>();
            ZoneDefinitions = new List<ZoneDefinition>();
    }

  
        public static Library fromJSON(string json)
        {
            return Serialization.Deserialize<Library>(json);
        }

        public string toJSON()
        {
            return Serialization.Serialize<Library>(this);
        }

        public void Import(string path)
        {
            
            try
            {
                
                if (File.Exists(path))
                {
                    Library ImportedLibrary = null;
                    string errPath = Path.GetDirectoryName(path);
                    string json = File.ReadAllText(path);
                    ImportedLibrary = Library.fromJSON(json);

                    string s = "";
                    if (ImportedLibrary.Correct(out s))
                    {
                        File.WriteAllText(errPath + "/ImportErrors.txt", s);
                    }

                    Import(ImportedLibrary);

                    Debug.WriteLine("Library loaded from " + path);
                }
                
            }
            catch (Exception ex) { Debug.WriteLine("eplusIDF.Library import error " + ex.Message); }
            
          
        }

        public void Import(Library ImportedLibrary)
        {
            TimeStamp = ImportedLibrary.TimeStamp;
            Version = ImportedLibrary.Version;

            OpaqueMaterials = ImportedLibrary.OpaqueMaterials;
            GlazingMaterials = ImportedLibrary.GlazingMaterials;
            GasMaterials = ImportedLibrary.GasMaterials;
            OpaqueConstructions = ImportedLibrary.OpaqueConstructions;
            GlazingConstructions = ImportedLibrary.GlazingConstructions;
            GlazingConstructionsSimple = ImportedLibrary.GlazingConstructionsSimple;
            DaySchedules = ImportedLibrary.DaySchedules;
            WeekSchedules = ImportedLibrary.WeekSchedules;
            YearSchedules = ImportedLibrary.YearSchedules;
            ArraySchedules = ImportedLibrary.ArraySchedules;


            ZoneLoads = ImportedLibrary.ZoneLoads;
            ZoneVentilations = ImportedLibrary.ZoneVentilations;
            ZoneConstructions = ImportedLibrary.ZoneConstructions;
            ZoneConditionings = ImportedLibrary.ZoneConditionings;
            DomHotWaters = ImportedLibrary.DomHotWaters;


            ZoneDefinitions = ImportedLibrary.ZoneDefinitions;

        }



        /// <summary>
        /// Copies the data of one object to another. The target object 'pulls' properties of the first. 
        /// This any matching properties are written to the target.
        /// 
        /// The object copy is a shallow copy only. Any nested types will be copied as 
        /// whole values rather than individual property assignments (ie. via assignment)
        /// </summary>
        /// <param name="source">The source object to copy from</param>
        /// <param name="target">The object to copy to</param>
        /// <param name="excludedProperties">A comma delimited list of properties that should not be copied</param>
        /// <param name="memberAccess">Reflection binding access</param>
        public static void CopyObjectData(object source, object target, string excludedProperties, BindingFlags memberAccess)
        {
            string[] excluded = null;
            if (!string.IsNullOrEmpty(excludedProperties))
                excluded = excludedProperties.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            MemberInfo[] miT = target.GetType().GetMembers(memberAccess);
            foreach (MemberInfo Field in miT)
            {
                string name = Field.Name;

                // Skip over any property exceptions
                if (!string.IsNullOrEmpty(excludedProperties) &&
                    excluded.Contains(name))
                    continue;

                if (Field.MemberType == MemberTypes.Field)
                {
                    FieldInfo SourceField = source.GetType().GetField(name);
                    if (SourceField == null)
                        continue;

                    object SourceValue = SourceField.GetValue(source);
                    ((FieldInfo)Field).SetValue(target, SourceValue);
                }
                else if (Field.MemberType == MemberTypes.Property)
                {
                    PropertyInfo piTarget = Field as PropertyInfo;
                    PropertyInfo SourceField = source.GetType().GetProperty(name, memberAccess);
                    if (SourceField == null)
                        continue;

                    if (piTarget.CanWrite && SourceField.CanRead)
                    {
                        object SourceValue = SourceField.GetValue(source, null);
                        piTarget.SetValue(target, SourceValue, null);
                    }
                }
            }
        }

  public void Clear()
        {

            try
            {
                TimeStamp = DateTime.Now;
                OpaqueMaterials.Clear();
                GlazingMaterials.Clear();
                GasMaterials.Clear();
                OpaqueConstructions.Clear();
                GlazingConstructions.Clear();
                GlazingConstructionsSimple.Clear();
                DaySchedules.Clear();
                WeekSchedules.Clear();
                YearSchedules.Clear();
                ArraySchedules.Clear();

                ZoneLoads.Clear();
                ZoneVentilations.Clear();
                ZoneConstructions.Clear();
                ZoneConditionings.Clear();
                DomHotWaters.Clear();
                ZoneDefinitions.Clear();
            }
            catch { }
        }



        #region CheckForInvalidObjs


        private void reportInvalidOpaqueMaterials(ref string errorReport)
        {
            //Dictionary<string, string> invalidNames = new Dictionary<string, string>();
            try
            {
                foreach (OpaqueMaterial op in this.OpaqueMaterials)
                {

                    // find strange names
                    string cleanName = HelperFunctions.RemoveSpecialCharacters(op.Name);
                    if (op.Name != cleanName)
                    {
                        errorReport += op.Name + " --> " + cleanName + " invalid characters have been removed" + "\r\n";
                        op.Name = cleanName;
                    }

                    // autocorrect values out of range // extract as method
                    if (op.Correct())
                    {
                        errorReport += "Material " + op.Name + " contained invalid entries that have been auto corrected \r\n";
                    }
                }
            }
            catch (Exception ex)
            {
                errorReport += ("reportInvalidOpaqueMaterials failed: " + ex.Message);// + " " + ex.InnerException.Message); }

            }
        }
        private void reportInvalidGlazingMaterials(ref string errorReport)
        {

            try
            {
                foreach (GlazingMaterial op in this.GlazingMaterials)
                {

                    // find strange names
                    string cleanName = HelperFunctions.RemoveSpecialCharacters(op.Name);
                    if (op.Name != cleanName)
                    {
                        errorReport += op.Name + " --> " + cleanName + " invalid characters have been removed" + "\r\n";
                        op.Name = cleanName;
                    }

                    // autocorrect values out of range // extract as method
                    if (op.Correct())
                    {
                        errorReport += "Material " + op.Name + " contained invalid entries that have been auto corrected \r\n";
                    }
                }
            }
            catch (Exception ex)
            {
                errorReport += ("reportInvalidGlazingMaterials failed: " + ex.Message);// + " " + ex.InnerException.Message); }
            }
        }
        private void reportInvalidOCons(ref string errorReport)
        {


            foreach (OpaqueConstruction op in this.OpaqueConstructions)
            {
                string cleanName = HelperFunctions.RemoveSpecialCharacters(op.Name);
                if (cleanName != op.Name)
                {

                    //op.Name = HelperFunctions.RemoveSpecialCharacters(op.Name);
                    errorReport += "Construction  " + op.Name + " name contained invalid characters and has been auto corrected to " + cleanName + "\r\n";
                    op.Name = cleanName;
                }

                foreach (var ol in op.Layers)
                {

                    ol.Material.Name = HelperFunctions.RemoveSpecialCharacters(ol.Material.Name);// fix as in materials
                    if (ol.Correct())
                    {
                        errorReport += "Layer in  " + op.Name + " contained invalid thickness \r\n";
                    }
                }
            }
        }
        private void reportInvalidGCons(ref string errorReport)
        {


            foreach (GlazingConstruction op in this.GlazingConstructions)
            {
                string cleanName = HelperFunctions.RemoveSpecialCharacters(op.Name);
                if (cleanName != op.Name)
                {

                    //op.Name = HelperFunctions.RemoveSpecialCharacters(op.Name);
                    errorReport += "Construction  " + op.Name + " name contained invalid characters and has been auto corrected to " + cleanName + "\r\n";
                    op.Name = cleanName;
                }

                foreach (var ol in op.Layers)
                {

                    ol.SetMaterialName(HelperFunctions.RemoveSpecialCharacters(ol.GetMaterialName()));// fix as in materials
                    if (ol.Correct())
                    {
                        errorReport += "Layer in  " + op.Name + " contained invalid thickness \r\n";
                    }
                }
            }
        }
        private void reportInvalidDaySchedules(ref string errorReport)
        {
            Dictionary<string, string> invalidNames = new Dictionary<string, string>();
            try
            {
                foreach (DaySchedule op in this.DaySchedules)
                {


                    // find strange names
                    string cleanName = HelperFunctions.RemoveSpecialCharacters(op.Name);
                    if (op.Name != cleanName)
                    {

                        errorReport += op.Name + " --> " + cleanName + " invalid characters have been removed" + "\r\n";
                        op.Name = cleanName;
                    }

                    // autocorrect values out of range // extract as method
                    if (op.Correct())
                    {
                        errorReport += "Schedule " + op.Name + " contained invalid entries that have been auto corrected \r\n";
                    }
                }
            }
            catch (Exception ex)
            {
                errorReport += ("reportInvalidDaySchedules failed: " + ex.Message);// + " " + ex.InnerException.Message); }
            }

        }
        private void reportInvalidWeekSchedules(ref string errorReport)
        {
            try
            {
                foreach (WeekSchedule op in this.WeekSchedules)
                {

                    // find strange names
                    string cleanName = HelperFunctions.RemoveSpecialCharacters(op.Name);
                    if (op.Name != cleanName)
                    {
                        errorReport += op.Name + " --> " + cleanName + " invalid characters have been removed" + "\r\n";
                        op.Name = cleanName;
                    }

                    // autocorrect values out of range // extract as method
                    if (op.Correct())
                    {
                        errorReport += "Schedule " + op.Name + " contained invalid entries that have been auto corrected \r\n";
                    }
                }
            }
            catch (Exception ex)
            {
                errorReport += ("reportInvalidWeekSchedules failed: " + ex.Message);// + " " + ex.InnerException.Message); }
            }
        }
        private void reportInvalidYearSchedules(ref string errorReport)
        {

            try
            {
                foreach (YearSchedule op in this.YearSchedules)
                {

                    // find strange names
                    string cleanName = HelperFunctions.RemoveSpecialCharacters(op.Name);
                    if (op.Name != cleanName)
                    {

                        errorReport += op.Name + " --> " + cleanName + " invalid characters have been removed" + "\r\n";
                        op.Name = cleanName;
                    }

                    // autocorrect values out of range // extract as method
                    if (op.Correct())
                    {
                        errorReport += "Schedule " + op.Name + " contained invalid entries that have been auto corrected \r\n";
                    }
                }
            }
            catch (Exception ex)
            {
                errorReport += ("reportInvalidYearSchedules failed: " + ex.Message);// + " " + ex.InnerException.Message); }
            }

        }

        public bool Correct(out string err)
        {

            string a = "";


            reportInvalidOpaqueMaterials(ref a);
            reportInvalidGlazingMaterials(ref a);
            reportInvalidOCons(ref a);
            reportInvalidGCons(ref a);
            reportInvalidDaySchedules(ref a);
            reportInvalidWeekSchedules(ref a);
            reportInvalidYearSchedules(ref a);



            err = "======= Error report =======\r\n\r\n" +
                a + "\r\n=======  Report End  =======";

            if (a == "") return false;
            else return true;

        }

#endregion
        
      

























    }


}


