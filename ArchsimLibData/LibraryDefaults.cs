using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace ArchsimLib
{
    /// <summary>
    /// 
    /// </summary>
    /// </exclude>
    public static class LibraryDefaults
    {
        public static Library loadDefualtLibrary()
        {
            Library Library = null;
           
            try
            {
                if (File.Exists(Utilities.AssemblyDirectory + @"\" + DefaultLibrary.FileName))
                    {
                        string serializedData = File.ReadAllText(Utilities.AssemblyDirectory + @"\" + DefaultLibrary.FileName);
                        Library = Library.fromJSON(serializedData);
                        // RhinoApp.WriteLine("Loading defualt library from " + Utilities.AssemblyDirectory + @"\" + DefaultLibrary.FileName);
                    }

            }
            catch (Exception ex) {
              //  Rhino.RhinoApp.WriteLine("Default library loading error " + ex.Message);
            }


            try {
                    if(Library == null)
                    {
                      //  RhinoApp.WriteLine("No default library found at " + Utilities.AssemblyDirectory + @"\" + DefaultLibrary.FileName + " Resetting default library.");
                        Library = LibraryDefaults.getHardCodedDefaultLib();
                        writeDefaultLibrary(Library);
                    }
            
            }
            catch (Exception ex) {
                //RhinoApp.WriteLine("Default library resetting error " + ex.Message);
            }


            return Library;
        }

        //wipes library and restores it with hard coded libray
        public static Library resetLibrary() {
            Library l = getHardCodedDefaultLib();

            writeDefaultLibrary(l);

            return l;
        }

        private static void writeDefaultLibrary(Library Library)
        {
            string rootPath = Utilities.AssemblyDirectory;
            try
            {
                    string newLib = Library.toJSON();
                    File.WriteAllText(rootPath + @"\" + DefaultLibrary.FileName, newLib);
                    //Rhino.RhinoApp.WriteLine("Library written to " + rootPath  + @"\" + DefaultLibrary.FileName);
            }
            catch { Debug.WriteLine("Library writing error"); }
        }


        // hardcoded default library
        private static Library getHardCodedDefaultLib()
        {
            Library Library = new Library();

            #region DEFAULTS - MUST BE IN LIBRARY

            OpaqueMaterial defaultMat = new OpaqueMaterial() {
            Name ="defaultMat",
            Type ="Concrete",
                Conductivity= 2.30,
                Density =2400,
                SpecificHeat=840,
                ThermalEmittance=0.9,
                SolarAbsorptance= 0.7,
                VisibleAbsorptance=0.7
            };
            Library.OpaqueMaterials.Add(defaultMat);

            GlazingMaterial defaultGMat = new GlazingMaterial(
            "defaultGlazingMat",
            "Uncoated",
            0.9,
            2500,
            0.68,
            0.09,
            0.10,
            0.81,
            0.11,
            0.12,
            0.00,
            0.84,
            0.20
            );
            Library.GlazingMaterials.Add( defaultGMat);



            Layer<OpaqueMaterial> defaultLay = new Layer<OpaqueMaterial>(0.25, defaultMat);
            OpaqueConstruction defaultConstruction = new OpaqueConstruction();
            defaultConstruction.Layers.Add(defaultLay);
            defaultConstruction.Name = "defaultConstruction";
            defaultConstruction.Type = "Facade";
            Library.OpaqueConstructions.Add( defaultConstruction);


            Layer<WindowMaterialBase> defaultGLay = new Layer<WindowMaterialBase>(0.006, defaultGMat);
            GlazingConstruction defaultGlazing = new GlazingConstruction();
            defaultGlazing.Layers.Add(defaultGLay);
            defaultGlazing.Name = "defaultGlazing";
            defaultGlazing.Type = "Single";
            Library.GlazingConstructions.Add( defaultGlazing);


            //AIRWALL
            GlazingMaterial AirWallMat = new GlazingMaterial("100TRANS", "uncoated", 5, 0.0001, 0.99, 0.005, 0.005, 0.99, 0.005, 0.005, 0.99, 0.005, 0.005);
            Library.GlazingMaterials.Add(AirWallMat);
            Layer<WindowMaterialBase> airwallLayer = new Layer<WindowMaterialBase>(0.003, AirWallMat);
            GlazingConstruction airWall = new GlazingConstruction();
            airWall.Layers.Add(airwallLayer);
            airWall.Name = "Airwall";
            airWall.Type = "Other";
            Library.GlazingConstructions.Add( airWall);




            //---------------------------------------------------------------------------------gases

            // add all possible gas materials to GasMaterials
           // string[] gases = { "AIR", "ARGON", "KRYPTON", "XENON", "SF6" };
            Library.GasMaterials.Clear();
            foreach (var s in Enum.GetValues(typeof(GasTypes)).Cast<GasTypes>())
            {
                Library.GasMaterials.Add(new GasMaterial(s));
            }



            int[] MonthFrom = { 1 };
            int[] DayFrom = { 1 };
            int[] MonthTo = { 12 };
            int[] DayTo = { 31 };



            #region AllOn

            double[] hourlyAllOnArr = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            DaySchedule hourlyAllOn = new DaySchedule("hourlyAllOn", "Fraction", hourlyAllOnArr.ToList());
            Library.DaySchedules.Add( hourlyAllOn);
            DaySchedule[] weekAllOnArr = { hourlyAllOn, hourlyAllOn, hourlyAllOn, hourlyAllOn, hourlyAllOn, hourlyAllOn, hourlyAllOn };
            WeekSchedule weekAllOn = new WeekSchedule("weekAllOn", weekAllOnArr, "Fraction");
            Library.WeekSchedules.Add( weekAllOn);
            WeekSchedule[] snarr = { weekAllOn };
            YearSchedule AllOnYear = new YearSchedule("AllOn", "Fraction", snarr.ToList(), MonthFrom.ToList(), DayFrom.ToList(), MonthTo.ToList(), DayTo.ToList());
            Library.YearSchedules.Add(AllOnYear);

            #endregion










            #endregion







            double[] a2 = { 1 };
            OpaqueMaterial mo2 = new OpaqueMaterial() {
                Name = "XPS Board",
                Type = "Insulation",
                Conductivity = 0.034,
                Density = 35,
                SpecificHeat = 1400,
                ThermalEmittance = 0.9,
                SolarAbsorptance = 0.6,
                VisibleAbsorptance = 0.6,
                EmbodiedEnergy = 87.4,
                EmbodiedCarbon = 2.8,
                Cost = 0.0,
                Comment = ""
            };

            Library.OpaqueMaterials.Add( mo2);

            #region U-WERT.NET OpaqueMaterials


            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Pine wood", Type = @"Wood", Conductivity = 0.13, Density = 520, SpecificHeat = 1600, ThermalEmittance = 0.9, SolarAbsorptance = 0.7, VisibleAbsorptance = 0.7, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Douglas fir", Type = @"Wood", Conductivity = 0.12, Density = 530, SpecificHeat = 1600, ThermalEmittance = 0.9, SolarAbsorptance = 0.7, VisibleAbsorptance = 0.7, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Oak", Type = @"Wood", Conductivity = 0.18, Density = 690, SpecificHeat = 2400, ThermalEmittance = 0.9, SolarAbsorptance = 0.7, VisibleAbsorptance = 0.7, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Spruce", Type = @"Wood", Conductivity = 0.13, Density = 450, SpecificHeat = 1600, ThermalEmittance = 0.9, SolarAbsorptance = 0.7, VisibleAbsorptance = 0.7, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Larch", Type = @"Wood", Conductivity = 0.13, Density = 460, SpecificHeat = 1600, ThermalEmittance = 0.9, SolarAbsorptance = 0.7, VisibleAbsorptance = 0.7, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Oriented strand board", Type = @"Wood", Conductivity = 0.13, Density = 650, SpecificHeat = 1700, ThermalEmittance = 0.9, SolarAbsorptance = 0.7, VisibleAbsorptance = 0.7, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Medium density fiberboard", Type = @"Wood", Conductivity = 0.09, Density = 500, SpecificHeat = 1700, ThermalEmittance = 0.9, SolarAbsorptance = 0.7, VisibleAbsorptance = 0.7, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Chipboard", Type = @"Wood", Conductivity = 0.14, Density = 650, SpecificHeat = 1800, ThermalEmittance = 0.9, SolarAbsorptance = 0.7, VisibleAbsorptance = 0.7, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Concrete", Type = @"Concrete", Conductivity = 2, Density = 2400, SpecificHeat = 950, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"U-Wert.net" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Concrete reinforced {1%}", Type = @"Concrete", Conductivity = 2.3, Density = 2300, SpecificHeat = 880, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"U-Wert.net" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Concrete reinforced {2%}", Type = @"Concrete", Conductivity = 2.5, Density = 2400, SpecificHeat = 880, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"U-Wert.net" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Asphalt", Type = @"Screed", Conductivity = 0.7, Density = 2350, SpecificHeat = 920, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"U-Wert.net" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Lightweight concrete", Type = @"Concrete", Conductivity = 1.3, Density = 1800, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"U-Wert.net" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Cement screed", Type = @"Screed", Conductivity = 1.4, Density = 2000, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"U-Wert.net" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Basalt", Type = @"Masonry", Conductivity = 3.5, Density = 2850, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"U-Wert.net" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Granite", Type = @"Masonry", Conductivity = 2.8, Density = 2600, SpecificHeat = 790, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"U-Wert.net" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Lime stone", Type = @"Masonry", Conductivity = 1.4, Density = 2000, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"U-Wert.net" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Adobe 1500kg/m3", Type = @"Masonry", Conductivity = 0.66, Density = 1500, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"U-Wert.net" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Sand stone", Type = @"Masonry", Conductivity = 2.3, Density = 2600, SpecificHeat = 710, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"U-Wert.net" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Clinker brick 1400 kg/m3", Type = @"Masonry", Conductivity = 0.58, Density = 1400, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"U-Wert.net" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Clinker brick 1600 kg/m3", Type = @"Masonry", Conductivity = 0.68, Density = 1600, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"U-Wert.net" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Clinker brick 1800 kg/m3", Type = @"Masonry", Conductivity = 0.81, Density = 1800, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"U-Wert.net" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Clinker brick 2000 kg/m3", Type = @"Masonry", Conductivity = 0.96, Density = 2000, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"U-Wert.net" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Aerated concrete 350kg/m3", Type = @"Masonry", Conductivity = 0.09, Density = 350, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"U-Wert.net" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Aerated concrete 500kg/m3", Type = @"Masonry", Conductivity = 0.12, Density = 500, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"U-Wert.net" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Ytong block W PP 1.6-0.30", Type = @"Masonry", Conductivity = 0.08, Density = 300, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"U-Wert.net" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Poroton Plan-T10", Type = @"Masonry", Conductivity = 0.1, Density = 650, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"U-Wert.net" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Rigid foam  EPS 035", Type = @"Insulation", Conductivity = 0.035, Density = 30, SpecificHeat = 1500, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Rigid foam PUR {no coating}", Type = @"Insulation", Conductivity = 0.03, Density = 30, SpecificHeat = 1400, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Rigid foam PUR {alu coating}", Type = @"Insulation", Conductivity = 0.025, Density = 30, SpecificHeat = 1400, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Rigid foam PUR {fleece coating}", Type = @"Insulation", Conductivity = 0.028, Density = 30, SpecificHeat = 1400, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Wood fiber insulating board", Type = @"Insulation", Conductivity = 0.042, Density = 160, SpecificHeat = 2100, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Styrofoam", Type = @"Insulation", Conductivity = 0.04, Density = 20, SpecificHeat = 1500, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Vacuum insulation panel Variotec", Type = @"Insulation", Conductivity = 0.007, Density = 205, SpecificHeat = 900, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net]" });





            #endregion

            //List of premade basic glazing materials/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            double[] a3 = { 1 };
            ArchsimLib.GlazingMaterial gm1 = new ArchsimLib.GlazingMaterial(
            "Generic Clear Glass 6mm",
            "Uncoated",
            0.9,
            2500,
            15,
            0.85,
            0.0,
            "",
            0.68,
            0.09,
            0.10,
            0.81,
            0.11,
            0.12,
            0.00,
            0.84,
            0.20
            );
            Library.GlazingMaterials.Add( gm1);



            //List of premade basic gas materials/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //GasMaterial g1 = new GasMaterial("ARGON", "ARGON");

            //Library.GasMaterials.Add( g1);

            //List of premade basic opaque constructions/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //Basic wall

            Layer<OpaqueMaterial> ol1 = new Layer<OpaqueMaterial>(0.20, defaultMat);
            Layer<OpaqueMaterial> ol2 = new Layer<OpaqueMaterial>(0.06, mo2);

            OpaqueConstruction oc1 = new OpaqueConstruction();

            oc1.Layers.Add(ol1);
            oc1.Layers.Add(ol2);
            oc1.Name = "Heavy Wall";
            oc1.Type = "Facade";

            Library.OpaqueConstructions.Add( oc1);

            //Basic roof

            Layer<OpaqueMaterial> ol3 = new Layer<OpaqueMaterial>(0.20, defaultMat);
            Layer<OpaqueMaterial> ol4 = new Layer<OpaqueMaterial>(0.12, mo2);

            OpaqueConstruction oc2 = new OpaqueConstruction();

            oc2.Layers.Add(ol1);
            oc2.Layers.Add(ol2);
            oc2.Name = "Heavy Roof";
            oc2.Type = "Roof";

            Library.OpaqueConstructions.Add(oc2);

            //Basic floor

            Layer<OpaqueMaterial> ol5 = new Layer<OpaqueMaterial>(0.20, defaultMat);

            OpaqueConstruction oc3 = new OpaqueConstruction();

            oc3.Layers.Add(ol1);
            oc3.Name = "Heavy Floor";
            oc3.Type = "Floor";

            Library.OpaqueConstructions.Add( oc3);


            //List of premade basic glazing constructions/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //Basic double glazing

            Layer<WindowMaterialBase> go1 = new Layer<WindowMaterialBase>(0.006, gm1);
            Layer<WindowMaterialBase> go2 = new Layer<WindowMaterialBase>(0.0013, Library.GasMaterials.Single(o=>o.Name == "ARGON"));

            GlazingConstruction gc1 = new GlazingConstruction();

            gc1.Layers.Add(go1);
            gc1.Layers.Add(go2);
            gc1.Layers.Add(go1);
            gc1.Name = "DblClear Air 6+13+6";
            gc1.Type = "Double";

            Library.GlazingConstructions.Add( gc1);




            #region schedules

            //--------------------------------------------------------------------------------schedules

            double[] occBedroomArr = { 1, 1, 1, 1, 1, 1, 0.8, 0.6, 0.4, 0.4, 0.4, 0.6, 0.8, 0.6, 0.4, 0.4, 0.6, 0.8, 0.8, 0.8, 0.8, 1, 1, 1 };
            DaySchedule occBedroom = new DaySchedule("occBedroom", "Fraction", occBedroomArr.ToList());
            occBedroom.DataSource = "SIA Merkblatt 2024";
            Library.DaySchedules.Add(occBedroom);
            DaySchedule[] occBedroomWeekArr = { occBedroom, occBedroom, occBedroom, occBedroom, occBedroom, occBedroom, occBedroom };
            WeekSchedule occBedroomWeek = new WeekSchedule("occBedroom", occBedroomWeekArr, "Fraction");
            occBedroomWeek.DataSource = occBedroom.DataSource;
            Library.WeekSchedules.Add(occBedroomWeek);
            WeekSchedule[] occBedroomYrArr = { occBedroomWeek };
            YearSchedule occBedroomYr = new YearSchedule("occBedroom", "Fraction", occBedroomYrArr.ToList(), MonthFrom.ToList(), DayFrom.ToList(), MonthTo.ToList(), DayTo.ToList());
            occBedroomYr.DataSource = occBedroom.DataSource;
            Library.YearSchedules.Add(occBedroomYr);

            double[] equipBedroomArr = { 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.5, 1, 0.5 , 0.5, 0.5, 1, 1, 0.5, 0.5, 0.5, 1, 1, 1, 1, 0.5, 0.5, 0.5, 0.1 };
            DaySchedule equipBedroom = new DaySchedule("equipBedroom", "Fraction", equipBedroomArr.ToList());
            equipBedroom.DataSource = "SIA Merkblatt 2024";
            Library.DaySchedules.Add(equipBedroom);
            DaySchedule[] equipBedroomWeekArr = { equipBedroom, equipBedroom, equipBedroom, equipBedroom, equipBedroom, equipBedroom, equipBedroom };
            WeekSchedule equipBedroomWeek = new WeekSchedule("equipBedroom", equipBedroomWeekArr, "Fraction");
            equipBedroomWeek.DataSource = equipBedroom.DataSource;
            Library.WeekSchedules.Add(equipBedroomWeek);
            WeekSchedule[] equipBedroomYrArr = { equipBedroomWeek };
            YearSchedule equipBedroomYr = new YearSchedule("equipBedroom", "Fraction", equipBedroomYrArr.ToList(), MonthFrom.ToList(), DayFrom.ToList(), MonthTo.ToList(), DayTo.ToList());
            equipBedroomYr.DataSource = equipBedroom.DataSource;
            Library.YearSchedules.Add(equipBedroomYr);






            double[] occKitchenArr = { 0, 0, 0, 0, 0, 0, 0.4, 0.8, 0.4, 0, 0, 0.4, 0.8, 0.4, 0, 0, 0.4, 1, 0.6, 0.4, 0, 0, 0, 0 };
            DaySchedule occKitchen = new DaySchedule("occKitchen", "Fraction", occKitchenArr.ToList());
            occKitchen.DataSource = "SIA Merkblatt 2024";
            Library.DaySchedules.Add(occKitchen);
            DaySchedule[] occKitchenWeekArr = { occKitchen, occKitchen, occKitchen, occKitchen, occKitchen, occKitchen, occKitchen };
            WeekSchedule occKitchenWeek = new WeekSchedule("occKitchen", occKitchenWeekArr, "Fraction");
            occKitchenWeek.DataSource = occKitchen.DataSource;
            Library.WeekSchedules.Add(occKitchenWeek);
            WeekSchedule[] occKitchenYrArr = { occKitchenWeek };
            YearSchedule occKitchenYr = new YearSchedule("occKitchen", "Fraction", occKitchenYrArr.ToList(), MonthFrom.ToList(), DayFrom.ToList(), MonthTo.ToList(), DayTo.ToList());
            occKitchenYr.DataSource = occKitchen.DataSource;
            Library.YearSchedules.Add(occKitchenYr);

            double[] equipmentKitchenArr = { 0.2, 0.2, 0.2, 0.2, 0.2, 0.2, 0.4, 0.8, 0.4, 0.2, 0.2, 0.4, 1, 0.4, 0.2, 0.2, 0.4, 1, 0.4, 0.2, 0.2, 0.2, 0.2, 0.2 };
            DaySchedule equipmentKitchen = new DaySchedule("equipmentKitchen", "Fraction", equipmentKitchenArr.ToList());
            equipmentKitchen.DataSource = "SIA Merkblatt 2024";
            Library.DaySchedules.Add(equipmentKitchen);
            DaySchedule[] equipmentKitchenWeekArr = { equipmentKitchen, equipmentKitchen, equipmentKitchen, equipmentKitchen, equipmentKitchen, equipmentKitchen, equipmentKitchen };
            WeekSchedule equipmentKitchenWeek = new WeekSchedule("equipmentKitchen", equipmentKitchenWeekArr, "Fraction");
            equipmentKitchenWeek.DataSource = equipmentKitchen.DataSource;
            Library.WeekSchedules.Add(equipmentKitchenWeek);
            WeekSchedule[] equipmentKitchenYrArr = { equipmentKitchenWeek };
            YearSchedule equipmentKitchenYr = new YearSchedule("equipmentKitchen", "Fraction", equipmentKitchenYrArr.ToList(), MonthFrom.ToList(), DayFrom.ToList(), MonthTo.ToList(), DayTo.ToList());
            equipmentKitchenYr.DataSource = equipmentKitchen.DataSource;
            Library.YearSchedules.Add(equipmentKitchenYr);







            double[] occOfficeArr = { 0, 0, 0, 0, 0, 0, 0, 0.2, 0.4, 0.6, 0.8, 0.8, 0.4, 0.6, 0.8, 0.8, 0.4, 0.2, 0, 0, 0, 0, 0, 0 };
            DaySchedule occOffice = new DaySchedule("occOffice", "Fraction", occOfficeArr.ToList());
            occOffice.DataSource = "SIA Merkblatt 2024";
            Library.DaySchedules.Add(occOffice);
            double[] occOfficeArrWE = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            DaySchedule occOfficeWE = new DaySchedule("occOffice", "Fraction", occOfficeArr.ToList());
            occOfficeWE.DataSource = "SIA Merkblatt 2024";
            Library.DaySchedules.Add(occOfficeWE);
            DaySchedule[] occOfficeWeekArr = { occOffice, occOffice, occOffice, occOffice, occOffice, occOfficeWE, occOfficeWE };
            WeekSchedule occOfficeWeek = new WeekSchedule("occOffice", occOfficeWeekArr, "Fraction");
            occOfficeWeek.DataSource = occOffice.DataSource;
            Library.WeekSchedules.Add(occOfficeWeek);
            WeekSchedule[] occOfficeYrArr = { occOfficeWeek };
            YearSchedule occOfficeYr = new YearSchedule("occOffice", "Fraction", occOfficeYrArr.ToList(), MonthFrom.ToList(), DayFrom.ToList(), MonthTo.ToList(), DayTo.ToList());
            occOfficeYr.DataSource = occOffice.DataSource;
            Library.YearSchedules.Add(occOfficeYr);

            double[] equipmentOfficeArr = { 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.2, 0.4, 0.6, 0.8, 0.8, 0.4, 0.6, 0.8, 0.8, 0.4, 0.2, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1 };
            DaySchedule equipmentOffice = new DaySchedule("equipmentOffice", "Fraction", equipmentOfficeArr.ToList());
            equipmentOffice.DataSource = "SIA Merkblatt 2024";
            Library.DaySchedules.Add(equipmentOffice);
            double[] equipmentOfficeArrWE = { 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1 };
            DaySchedule equipmentOfficeWE = new DaySchedule("equipmentOffice", "Fraction", equipmentOfficeArr.ToList());
            equipmentOfficeWE.DataSource = "SIA Merkblatt 2024";
            Library.DaySchedules.Add(equipmentOfficeWE);
            DaySchedule[] equipmentOfficeWeekArr = { equipmentOffice, equipmentOffice, equipmentOffice, equipmentOffice, equipmentOffice, equipmentOfficeWE, equipmentOfficeWE };
            WeekSchedule equipmentOfficeWeek = new WeekSchedule("equipmentOffice", equipmentOfficeWeekArr, "Fraction");
            equipmentOfficeWeek.DataSource = equipmentOffice.DataSource;
            Library.WeekSchedules.Add(equipmentOfficeWeek);
            WeekSchedule[] equipmentOfficeYrArr = { equipmentOfficeWeek };
            YearSchedule equipmentOfficeYr = new YearSchedule("equipmentOffice", "Fraction", equipmentOfficeYrArr.ToList(), MonthFrom.ToList(), DayFrom.ToList(), MonthTo.ToList(), DayTo.ToList());
            equipmentOfficeYr.DataSource = equipmentOffice.DataSource;
            Library.YearSchedules.Add(equipmentOfficeYr);





            double[] occMeetingRoomArr = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.6, 1, 0.4, 0, 0, 0.6, 1, 0.4, 0, 0, 0, 0, 0, 0, 0 };
            DaySchedule occMeetingRoom = new DaySchedule("occMeetingRoom", "Fraction", occMeetingRoomArr.ToList());
            occMeetingRoom.DataSource = "SIA Merkblatt 2024";
            Library.DaySchedules.Add(occMeetingRoom);
            double[] occMeetingRoomArrWE = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            DaySchedule occMeetingRoomWE = new DaySchedule("occMeetingRoom", "Fraction", occMeetingRoomArr.ToList());
            occMeetingRoomWE.DataSource = "SIA Merkblatt 2024";
            Library.DaySchedules.Add(occMeetingRoomWE);
            DaySchedule[] occMeetingRoomWeekArr = { occMeetingRoom, occMeetingRoom, occMeetingRoom, occMeetingRoom, occMeetingRoom, occMeetingRoomWE, occMeetingRoomWE };
            WeekSchedule occMeetingRoomWeek = new WeekSchedule("occMeetingRoom", occMeetingRoomWeekArr, "Fraction");
            occMeetingRoomWeek.DataSource = occMeetingRoom.DataSource;
            Library.WeekSchedules.Add(occMeetingRoomWeek);
            WeekSchedule[] occMeetingRoomYrArr = { occMeetingRoomWeek };
            YearSchedule occMeetingRoomYr = new YearSchedule("occMeetingRoom", "Fraction", occMeetingRoomYrArr.ToList(), MonthFrom.ToList(), DayFrom.ToList(), MonthTo.ToList(), DayTo.ToList());
            occMeetingRoomYr.DataSource = occMeetingRoom.DataSource;
            Library.YearSchedules.Add(occMeetingRoomYr);


            double[] equipmentMeetingRoomArr = { 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.6, 1, 0.4, 0.1, 0.1, 0.6, 1, 0.4, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1 };
            DaySchedule equipmentMeetingRoom = new DaySchedule("equipmentMeetingRoom", "Fraction", equipmentMeetingRoomArr.ToList());
            equipmentMeetingRoom.DataSource = "SIA Merkblatt 2024";
            Library.DaySchedules.Add(equipmentMeetingRoom);
            double[] equipmentMeetingRoomArrWE = { 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1 };
            DaySchedule equipmentMeetingRoomWE = new DaySchedule("equipmentMeetingRoom", "Fraction", equipmentMeetingRoomArr.ToList());
            equipmentMeetingRoomWE.DataSource = "SIA Merkblatt 2024";
            Library.DaySchedules.Add(equipmentMeetingRoomWE);
            DaySchedule[] equipmentMeetingRoomWeekArr = { equipmentMeetingRoom, equipmentMeetingRoom, equipmentMeetingRoom, equipmentMeetingRoom, equipmentMeetingRoom, equipmentMeetingRoomWE, equipmentMeetingRoomWE };
            WeekSchedule equipmentMeetingRoomWeek = new WeekSchedule("equipmentMeetingRoom", equipmentMeetingRoomWeekArr, "Fraction");
            equipmentMeetingRoomWeek.DataSource = equipmentMeetingRoom.DataSource;
            Library.WeekSchedules.Add(equipmentMeetingRoomWeek);
            WeekSchedule[] equipmentMeetingRoomYrArr = { equipmentMeetingRoomWeek };
            YearSchedule equipmentMeetingRoomYr = new YearSchedule("equipmentMeetingRoom", "Fraction", equipmentMeetingRoomYrArr.ToList(), MonthFrom.ToList(), DayFrom.ToList(), MonthTo.ToList(), DayTo.ToList());
            equipmentMeetingRoomYr.DataSource = equipmentMeetingRoom.DataSource;
            Library.YearSchedules.Add(equipmentMeetingRoomYr);







            double[] occLibraryArr = { 0, 0, 0, 0, 0, 0, 0, 0.2, 0.6, 1, 1, 0.2, 0.2, 1, 1, 0.6, 0.2, 0, 0, 0, 0, 0, 0, 0 };
            DaySchedule occLibrary = new DaySchedule("occLibrary", "Fraction", occLibraryArr.ToList());
            occLibrary.DataSource = "SIA Merkblatt 2024";
            Library.DaySchedules.Add(occLibrary);
            DaySchedule[] occLibraryWeekArr = { occLibrary, occLibrary, occLibrary, occLibrary, occLibrary, occLibrary, occLibrary };
            WeekSchedule occLibraryWeek = new WeekSchedule("occLibrary", occLibraryWeekArr, "Fraction");
            occLibraryWeek.DataSource = occLibrary.DataSource;
            Library.WeekSchedules.Add(occLibraryWeek);
            WeekSchedule[] occLibraryYrArr = { occLibraryWeek };
            YearSchedule occLibraryYr = new YearSchedule("occLibrary", "Fraction", occLibraryYrArr.ToList(), MonthFrom.ToList(), DayFrom.ToList(), MonthTo.ToList(), DayTo.ToList());
            occLibraryYr.DataSource = occLibrary.DataSource;
            Library.YearSchedules.Add(occLibraryYr);

            double[] equipmentLibraryArr = { 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.2, 0.6, 1, 1, 0.2, 0.2, 1, 1, 0.6, 0.4, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1 };
            DaySchedule equipmentLibrary = new DaySchedule("equipmentLibrary", "Fraction", equipmentLibraryArr.ToList());
            equipmentLibrary.DataSource = "SIA Merkblatt 2024";
            Library.DaySchedules.Add(equipmentLibrary);
            DaySchedule[] equipmentLibraryWeekArr = { equipmentLibrary, equipmentLibrary, equipmentLibrary, equipmentLibrary, equipmentLibrary, equipmentLibrary, equipmentLibrary };
            WeekSchedule equipmentLibraryWeek = new WeekSchedule("equipmentLibrary", equipmentLibraryWeekArr, "Fraction");
            equipmentLibraryWeek.DataSource = equipmentLibrary.DataSource;
            Library.WeekSchedules.Add(equipmentLibraryWeek);
            WeekSchedule[] equipmentLibraryYrArr = { equipmentLibraryWeek };
            YearSchedule equipmentLibraryYr = new YearSchedule("equipmentLibrary", "Fraction", equipmentLibraryYrArr.ToList(), MonthFrom.ToList(), DayFrom.ToList(), MonthTo.ToList(), DayTo.ToList());
            equipmentLibraryYr.DataSource = equipmentLibrary.DataSource;
            Library.YearSchedules.Add(equipmentLibraryYr);






            double[] occLectureHallArr = { 0, 0, 0, 0, 0, 0, 0, 0.2, 0.6, 1, 1, 0.2, 0.2, 1, 1, 0.6, 0.2, 0, 0, 0, 0, 0, 0, 0 };
            DaySchedule occLectureHall = new DaySchedule("occLectureHall", "Fraction", occLectureHallArr.ToList());
            occLectureHall.DataSource = "SIA Merkblatt 2024";
            Library.DaySchedules.Add(occLectureHall);
            double[] occLectureHallArrWE = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            DaySchedule occLectureHallWE = new DaySchedule("occLectureHall", "Fraction", occLectureHallArr.ToList());
            occLectureHallWE.DataSource = "SIA Merkblatt 2024";
            Library.DaySchedules.Add(occLectureHallWE);
            DaySchedule[] occLectureHallWeekArr = { occLectureHall, occLectureHall, occLectureHall, occLectureHall, occLectureHall, occLectureHallWE, occLectureHallWE };
            WeekSchedule occLectureHallWeek = new WeekSchedule("occLectureHall", occLectureHallWeekArr, "Fraction");
            occLectureHallWeek.DataSource = occLectureHall.DataSource;
            Library.WeekSchedules.Add(occLectureHallWeek);
            WeekSchedule[] occLectureHallYrArr = { occLectureHallWeek };
            YearSchedule occLectureHallYr = new YearSchedule("occLectureHall", "Fraction", occLectureHallYrArr.ToList(), MonthFrom.ToList(), DayFrom.ToList(), MonthTo.ToList(), DayTo.ToList());
            occLectureHallYr.DataSource = occLectureHall.DataSource;
            Library.YearSchedules.Add(occLectureHallYr);

            double[] equipmentLectureHallArr = { 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.2, 0.6, 1, 1, 0.2, 0.2, 1, 1, 0.6, 0.4, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1 };
            DaySchedule equipmentLectureHall = new DaySchedule("equipmentLectureHall", "Fraction", equipmentLectureHallArr.ToList());
            equipmentLectureHall.DataSource = "SIA Merkblatt 2024";
            Library.DaySchedules.Add(equipmentLectureHall);
            double[] equipmentLectureHallArrWE = { 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 1, 1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1 };
            DaySchedule equipmentLectureHallWE = new DaySchedule("equipmentLectureHall", "Fraction", equipmentLectureHallArr.ToList());
            equipmentLectureHallWE.DataSource = "SIA Merkblatt 2024";
            Library.DaySchedules.Add(equipmentLectureHallWE);
            DaySchedule[] equipmentLectureHallWeekArr = { equipmentLectureHall, equipmentLectureHall, equipmentLectureHall, equipmentLectureHall, equipmentLectureHall, equipmentLectureHallWE, equipmentLectureHallWE };
            WeekSchedule equipmentLectureHallWeek = new WeekSchedule("equipmentLectureHall", equipmentLectureHallWeekArr, "Fraction");
            equipmentLectureHallWeek.DataSource = equipmentLectureHall.DataSource;
            Library.WeekSchedules.Add(equipmentLectureHallWeek);
            WeekSchedule[] equipmentLectureHallYrArr = { equipmentLectureHallWeek };
            YearSchedule equipmentLectureHallYr = new YearSchedule("equipmentLectureHall", "Fraction", equipmentLectureHallYrArr.ToList(), MonthFrom.ToList(), DayFrom.ToList(), MonthTo.ToList(), DayTo.ToList());
            equipmentLectureHallYr.DataSource = equipmentLectureHall.DataSource;
            Library.YearSchedules.Add(equipmentLectureHallYr);









            double[] occSuperMarketArr = { 0, 0, 0, 0, 0, 0, 0, 0.2, 0.4, 0.4, 0.4, 0.6, 0.6, 0.6, 0.4, 0.4, 0.6, 0.8, 0.6, 0, 0, 0, 0, 0 };
            DaySchedule occSuperMarket = new DaySchedule("occSuperMarket", "Fraction", occSuperMarketArr.ToList());
            occSuperMarket.DataSource = "SIA Merkblatt 2024";
            Library.DaySchedules.Add(occSuperMarket);
            DaySchedule[] occSuperMarketWeekArr = { occSuperMarket, occSuperMarket, occSuperMarket, occSuperMarket, occSuperMarket, occSuperMarket, occSuperMarket };
            WeekSchedule occSuperMarketWeek = new WeekSchedule("occSuperMarket", occSuperMarketWeekArr, "Fraction");
            occSuperMarketWeek.DataSource = occSuperMarket.DataSource;
            Library.WeekSchedules.Add(occSuperMarketWeek);
            WeekSchedule[] occSuperMarketYrArr = { occSuperMarketWeek };
            YearSchedule occSuperMarketYr = new YearSchedule("occSuperMarket", "Fraction", occSuperMarketYrArr.ToList(), MonthFrom.ToList(), DayFrom.ToList(), MonthTo.ToList(), DayTo.ToList());
            occSuperMarketYr.DataSource = occSuperMarket.DataSource;
            Library.YearSchedules.Add(occSuperMarketYr);

            double[] equipmentSuperMarketArr = { 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0.1, 0.1, 0.1, 0.1, 0.1 };
            DaySchedule equipmentSuperMarket = new DaySchedule("equipmentSuperMarket", "Fraction", equipmentSuperMarketArr.ToList());
            equipmentSuperMarket.DataSource = "SIA Merkblatt 2024";
            Library.DaySchedules.Add(equipmentSuperMarket);
            DaySchedule[] equipmentSuperMarketWeekArr = { equipmentSuperMarket, equipmentSuperMarket, equipmentSuperMarket, equipmentSuperMarket, equipmentSuperMarket, equipmentSuperMarket, equipmentSuperMarket };
            WeekSchedule equipmentSuperMarketWeek = new WeekSchedule("equipmentSuperMarket", equipmentSuperMarketWeekArr, "Fraction");
            equipmentSuperMarketWeek.DataSource = equipmentSuperMarket.DataSource;
            Library.WeekSchedules.Add(equipmentSuperMarketWeek);
            WeekSchedule[] equipmentSuperMarketYrArr = { equipmentSuperMarketWeek };
            YearSchedule equipmentSuperMarketYr = new YearSchedule("equipmentSuperMarket", "Fraction", equipmentSuperMarketYrArr.ToList(), MonthFrom.ToList(), DayFrom.ToList(), MonthTo.ToList(), DayTo.ToList());
            equipmentSuperMarketYr.DataSource = equipmentSuperMarket.DataSource;
            Library.YearSchedules.Add(equipmentSuperMarketYr);






            double[] occShoppingArr = { 0, 0, 0, 0, 0, 0, 0, 0.2, 0.4, 0.4, 0.4, 0.6, 0.6, 0.6, 0.4, 0.4, 0.6, 0.8, 0.6, 0, 0, 0, 0, 0 };
            DaySchedule occShopping = new DaySchedule("occShopping", "Fraction", occShoppingArr.ToList());
            occShopping.DataSource = "SIA Merkblatt 2024";
            Library.DaySchedules.Add(occShopping);
            DaySchedule[] occShoppingWeekArr = { occShopping, occShopping, occShopping, occShopping, occShopping, occShopping, occShopping };
            WeekSchedule occShoppingWeek = new WeekSchedule("occShopping", occShoppingWeekArr, "Fraction");
            occShoppingWeek.DataSource = occShopping.DataSource;
            Library.WeekSchedules.Add(occShoppingWeek);
            WeekSchedule[] occShoppingYrArr = { occShoppingWeek };
            YearSchedule occShoppingYr = new YearSchedule("occShopping", "Fraction", occShoppingYrArr.ToList(), MonthFrom.ToList(), DayFrom.ToList(), MonthTo.ToList(), DayTo.ToList());
            occShoppingYr.DataSource = occShopping.DataSource;
            Library.YearSchedules.Add(occShoppingYr);

            double[] equipmentShoppingArr = { 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0.1, 0.1, 0.1, 0.1, 0.1 };
            DaySchedule equipmentShopping = new DaySchedule("equipmentShopping", "Fraction", equipmentShoppingArr.ToList());
            equipmentShopping.DataSource = "SIA Merkblatt 2024";
            Library.DaySchedules.Add(equipmentShopping);
            DaySchedule[] equipmentShoppingWeekArr = { equipmentShopping, equipmentShopping, equipmentShopping, equipmentShopping, equipmentShopping, equipmentShopping, equipmentShopping };
            WeekSchedule equipmentShoppingWeek = new WeekSchedule("equipmentShopping", equipmentShoppingWeekArr, "Fraction");
            equipmentShoppingWeek.DataSource = equipmentShopping.DataSource;
            Library.WeekSchedules.Add(equipmentShoppingWeek);
            WeekSchedule[] equipmentShoppingYrArr = { equipmentShoppingWeek };
            YearSchedule equipmentShoppingYr = new YearSchedule("equipmentShopping", "Fraction", equipmentShoppingYrArr.ToList(), MonthFrom.ToList(), DayFrom.ToList(), MonthTo.ToList(), DayTo.ToList());
            equipmentShoppingYr.DataSource = equipmentShopping.DataSource;
            Library.YearSchedules.Add(equipmentShoppingYr);



            #endregion




            //double[] ones241 = {  0, 0, 0, 0.5, 0.8, 1, 1, 1, 1, 1, 1, 1, 1, 0.8, 0.5, 0, 0, 0, 0, 0, 0, 0,1,1 };
            //DaySchedule AllOnDay1 = new DaySchedule("Profile", "Fraction", ones241.ToList());
            //Library.DaySchedules.Add(AllOnDay1.Name, AllOnDay1);

            //double[] ones242 = { 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22 };
            //DaySchedule AllOnDay2 = new DaySchedule("SetPoint", "Temperature", ones242.ToList());
            //Library.DaySchedules.Add(AllOnDay2.Name, AllOnDay2);



            #region SetBackScheduleOffice

            //double[] hourlySetBackScheduleOfficeArrWE = { 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12 };
            //DaySchedule hourlySetBackScheduleOfficeWE = new DaySchedule("hourlySetBackScheduleOfficeWE", "Temperature", hourlySetBackScheduleOfficeArrWE.ToList());
            //Library.DaySchedules.Add(hourlySetBackScheduleOfficeWE.Name, hourlySetBackScheduleOfficeWE);


            //double[] hourlySetBackScheduleOfficeArr = { 12, 12, 12, 12, 12, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 22, 12, 12, 12, 12, 12, 12 };
            //DaySchedule hourlySetBackScheduleOffice = new DaySchedule("hourlySetBackScheduleOffice", "Temperature", hourlySetBackScheduleOfficeArr.ToList());
            //Library.DaySchedules.Add(hourlySetBackScheduleOffice.Name, hourlySetBackScheduleOffice);

            //string[] weekSetBackScheduleOfficeArr = { "hourlySetBackScheduleOffice", "hourlySetBackScheduleOffice", "hourlySetBackScheduleOffice", "hourlySetBackScheduleOffice", "hourlySetBackScheduleOffice", "hourlySetBackScheduleOfficeWE", "hourlySetBackScheduleOfficeWE" };
            //WeekSchedule weekSetBackScheduleOffice = new WeekSchedule("weekSetBackScheduleOffice", weekSetBackScheduleOfficeArr, "Temperature");
            //Library.WeekSchedules.Add(weekSetBackScheduleOffice.Name, weekSetBackScheduleOffice);

            //string[] yearSetBackScheduleOfficeArr = { "weekSetBackScheduleOffice" };
            //int[] yearSetBackScheduleOfficeMonthFrom = { 1 };
            //int[] yearSetBackScheduleOfficeDayFrom = { 1 };
            //int[] yearSetBackScheduleOfficeMonthTo = { 12 };
            //int[] yearSetBackScheduleOfficeDayTo = { 31 };
            //YearSchedule yearSetBackScheduleOffice = new YearSchedule("SetBackScheduleOffice", "Temperature", yearSetBackScheduleOfficeArr.ToList(), yearSetBackScheduleOfficeMonthFrom.ToList(), yearSetBackScheduleOfficeDayFrom.ToList(), yearSetBackScheduleOfficeMonthTo.ToList(), yearSetBackScheduleOfficeDayTo.ToList());
            //Library.YearSchedules.Add(yearSetBackScheduleOffice.Name, yearSetBackScheduleOffice);



            #endregion

            #region ResidentialOccOcc

            double[] hourlyResidentialOccWEArr = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5, 1, 1, 1, 1, 1, 1 };
            DaySchedule hourlyResidentialOccWE = new DaySchedule("hourlyResidentialOccWE", "Fraction", hourlyResidentialOccWEArr.ToList());
            Library.DaySchedules.Add( hourlyResidentialOccWE);

            double[] hourlyResidentialOccArr = { 1, 1, 1, 1, 1, 1, 1, 0.5, 0.25, 0, 0, 0, 0, 0, 0, 0, 0, 0.25, 0.5, 1, 1, 1, 1, 1 };
            DaySchedule hourlyResidentialOcc = new DaySchedule("hourlyResidentialOcc", "Fraction", hourlyResidentialOccArr.ToList());
            Library.DaySchedules.Add( hourlyResidentialOcc);

            DaySchedule[] weekResidentialOccArr = { hourlyResidentialOcc, hourlyResidentialOcc, hourlyResidentialOcc, hourlyResidentialOcc, hourlyResidentialOcc, hourlyResidentialOccWE, hourlyResidentialOccWE };
            WeekSchedule weekResidentialOcc = new WeekSchedule("weekResidentialOcc", weekResidentialOccArr, "Fraction");
            Library.WeekSchedules.Add( weekResidentialOcc);


            WeekSchedule[] yearResidentialOccArr = { weekResidentialOcc };
            int[] yearResidentialOccMonthFrom = { 1 };
            int[] yearResidentialOccDayFrom = { 1 };
            int[] yearResidentialOccMonthTo = { 12 };
            int[] yearResidentialOccDayTo = { 31 };
            YearSchedule ResidentialOcc = new YearSchedule("ResidentialOcc", "Fraction", yearResidentialOccArr.ToList(), yearResidentialOccMonthFrom.ToList(), yearResidentialOccDayFrom.ToList(), yearResidentialOccMonthTo.ToList(), yearResidentialOccDayTo.ToList());
            Library.YearSchedules.Add(ResidentialOcc);
            #endregion

            #region OpenOfficeOcc

            double[] hourlyOpenOfficeOccWEArr = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            DaySchedule hourlyOpenOfficeOccWE = new DaySchedule("hourlyOpenOfficeOccWE", "Fraction", hourlyOpenOfficeOccWEArr.ToList());
            Library.DaySchedules.Add(hourlyOpenOfficeOccWE);

            double[] hourlyOpenOfficeOccArr = { 0, 0, 0, 0, 0, 0, 0.25, 0.5, 1, 1, 1, 0.75, 0.75, 0.75, 1, 1, 1, 0.5, 0.25, 0, 0, 0, 0, 0 };
            DaySchedule hourlyOpenOfficeOcc = new DaySchedule("hourlyOpenOfficeOcc", "Fraction", hourlyOpenOfficeOccArr.ToList());
            Library.DaySchedules.Add( hourlyOpenOfficeOcc);

            DaySchedule[] weekOpenOfficeOccArr = { hourlyOpenOfficeOcc, hourlyOpenOfficeOcc, hourlyOpenOfficeOcc, hourlyOpenOfficeOcc, hourlyOpenOfficeOcc, hourlyOpenOfficeOcc, hourlyOpenOfficeOcc };
            WeekSchedule weekOpenOfficeOcc = new WeekSchedule("weekOpenOfficeOcc", weekOpenOfficeOccArr, "Fraction");
            Library.WeekSchedules.Add( weekOpenOfficeOcc);


            WeekSchedule[] yearOpenOfficeOccArr = { weekOpenOfficeOcc };
            int[] yearOpenOfficeOccMonthFrom = { 1 };
            int[] yearOpenOfficeOccDayFrom = { 1 };
            int[] yearOpenOfficeOccMonthTo = { 12 };
            int[] yearOpenOfficeOccDayTo = { 31 };
            YearSchedule OpenOfficeOcc = new YearSchedule("OpenOfficeOcc", "Fraction", yearOpenOfficeOccArr.ToList(), yearOpenOfficeOccMonthFrom.ToList(), yearOpenOfficeOccDayFrom.ToList(), yearOpenOfficeOccMonthTo.ToList(), yearOpenOfficeOccDayTo.ToList());
            Library.YearSchedules.Add( OpenOfficeOcc);
            #endregion


            #region SimpleGlass

            Library.GlazingConstructionsSimple.Add(new GlazingConstructionSimple("SinglePaneClr", "Single pane", "Standard clear", 0.913, 5.894, 0.905));
            Library.GlazingConstructionsSimple.Add(new GlazingConstructionSimple("DoublePaneClr", "Double pane", "Standard clear", 0.812, 2.720, 0.764));
            Library.GlazingConstructionsSimple.Add(new GlazingConstructionSimple("DoublePaneLoEe2", "Double pane", "Low emissivity coating on layer e2", 0.444, 1.493, 0.373));
            Library.GlazingConstructionsSimple.Add(new GlazingConstructionSimple("DoublePaneLoEe3", "Double pane", "Low emissivity coating on layer e3", 0.769, 1.507, 0.649));
            Library.GlazingConstructionsSimple.Add(new GlazingConstructionSimple("TriplePaneLoE", "Triple pane", "Low emissivity coating on layer e2 and e5", 0.661, 0.785, 0.764));


            #endregion


            #region ZoneLoadTemplates





            #endregion


            ////--------------------------------------------------------------------------------serialization test


            //string xml = SerializeDeserialize.Serialize(Library);

            //SerializeDeserialize.Deserialize(xml, typeof(ArchsimLib.Lib));

            ////--------------------------------------------------------------------------------out

            return Library;
        }
    }
}
