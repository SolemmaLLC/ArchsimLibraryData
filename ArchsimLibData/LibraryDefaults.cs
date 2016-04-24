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

            GlazingMaterial defaultGMat = new GlazingMaterial() {
           Name = "defaultGlazingMat",
                Type=  "Uncoated",
                Conductivity = 0.9,
                Density= 2500,
                SolarTransmittance =0.68,
                SolarReflectanceFront =0.09,
                SolarReflectanceBack=0.10,
                VisibleTransmittance=0.81,
                VisibleReflectanceFront=0.11,
                VisibleReflectanceBack=0.12,
                IRTransmittance=0.00,
                IREmissivityFront=0.84,
                IREmissivityBack=0.20
           };
            Library.GlazingMaterials.Add( defaultGMat);



            Layer<OpaqueMaterial> defaultLay = new Layer<OpaqueMaterial>(0.25, defaultMat);
            OpaqueConstruction defaultConstruction = new OpaqueConstruction();
            defaultConstruction.Layers.Add(defaultLay);
            defaultConstruction.Name = "defaultConstruction";
            defaultConstruction.Type = ConstructionTypes.Facade;// "Facade";
            Library.OpaqueConstructions.Add( defaultConstruction);


            Layer<WindowMaterialBase> defaultGLay = new Layer<WindowMaterialBase>(0.006, defaultGMat);
            GlazingConstruction defaultGlazing = new GlazingConstruction();
            defaultGlazing.Layers.Add(defaultGLay);
            defaultGlazing.Name = "defaultGlazing";
            defaultGlazing.Type = GlazingConstructionTypes.Single;// "Single";
            Library.GlazingConstructions.Add( defaultGlazing);


            //AIRWALL
            GlazingMaterial AirWallMat = new GlazingMaterial()
            {
                Name = "100TRANS",
                Type = "uncoated",
                Conductivity = 5,
                Density = 0.0001,
                SolarTransmittance = 0.99,
                SolarReflectanceFront = 0.005,
                SolarReflectanceBack = 0.005,
                VisibleTransmittance = 0.99,
                VisibleReflectanceFront = 0.005,
                VisibleReflectanceBack = 0.005,
                IRTransmittance = 0.99,
                IREmissivityFront = 0.005,
                IREmissivityBack = 0.005
            };
            Library.GlazingMaterials.Add(AirWallMat);
            Layer<WindowMaterialBase> airwallLayer = new Layer<WindowMaterialBase>(0.003, AirWallMat);
            GlazingConstruction airWall = new GlazingConstruction();
            airWall.Layers.Add(airwallLayer);
            airWall.Name = "Airwall";
            airWall.Type = GlazingConstructionTypes.Other;// "Other";
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


            //Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Pine wood", Type = @"Wood", Conductivity = 0.13, Density = 520, SpecificHeat = 1600, ThermalEmittance = 0.9, SolarAbsorptance = 0.7, VisibleAbsorptance = 0.7, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net]" });
            //Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Douglas fir", Type = @"Wood", Conductivity = 0.12, Density = 530, SpecificHeat = 1600, ThermalEmittance = 0.9, SolarAbsorptance = 0.7, VisibleAbsorptance = 0.7, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net]" });
            //Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Oak", Type = @"Wood", Conductivity = 0.18, Density = 690, SpecificHeat = 2400, ThermalEmittance = 0.9, SolarAbsorptance = 0.7, VisibleAbsorptance = 0.7, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net]" });
            //Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Spruce", Type = @"Wood", Conductivity = 0.13, Density = 450, SpecificHeat = 1600, ThermalEmittance = 0.9, SolarAbsorptance = 0.7, VisibleAbsorptance = 0.7, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net]" });
            //Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Larch", Type = @"Wood", Conductivity = 0.13, Density = 460, SpecificHeat = 1600, ThermalEmittance = 0.9, SolarAbsorptance = 0.7, VisibleAbsorptance = 0.7, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net]" });
            //Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Oriented strand board", Type = @"Wood", Conductivity = 0.13, Density = 650, SpecificHeat = 1700, ThermalEmittance = 0.9, SolarAbsorptance = 0.7, VisibleAbsorptance = 0.7, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net]" });
            //Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Medium density fiberboard", Type = @"Wood", Conductivity = 0.09, Density = 500, SpecificHeat = 1700, ThermalEmittance = 0.9, SolarAbsorptance = 0.7, VisibleAbsorptance = 0.7, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net]" });
            //Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Chipboard", Type = @"Wood", Conductivity = 0.14, Density = 650, SpecificHeat = 1800, ThermalEmittance = 0.9, SolarAbsorptance = 0.7, VisibleAbsorptance = 0.7, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net]" });
            //Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Concrete", Type = @"Concrete", Conductivity = 2, Density = 2400, SpecificHeat = 950, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"U-Wert.net" });
            //Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Concrete reinforced {1%}", Type = @"Concrete", Conductivity = 2.3, Density = 2300, SpecificHeat = 880, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"U-Wert.net" });
            //Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Concrete reinforced {2%}", Type = @"Concrete", Conductivity = 2.5, Density = 2400, SpecificHeat = 880, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"U-Wert.net" });
            //Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Asphalt", Type = @"Screed", Conductivity = 0.7, Density = 2350, SpecificHeat = 920, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"U-Wert.net" });
            //Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Lightweight concrete", Type = @"Concrete", Conductivity = 1.3, Density = 1800, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"U-Wert.net" });
            //Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Cement screed", Type = @"Screed", Conductivity = 1.4, Density = 2000, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"U-Wert.net" });
            //Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Basalt", Type = @"Masonry", Conductivity = 3.5, Density = 2850, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"U-Wert.net" });
            //Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Granite", Type = @"Masonry", Conductivity = 2.8, Density = 2600, SpecificHeat = 790, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"U-Wert.net" });
            //Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Lime stone", Type = @"Masonry", Conductivity = 1.4, Density = 2000, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"U-Wert.net" });
            //Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Adobe 1500kg/m3", Type = @"Masonry", Conductivity = 0.66, Density = 1500, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"U-Wert.net" });
            //Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Sand stone", Type = @"Masonry", Conductivity = 2.3, Density = 2600, SpecificHeat = 710, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"U-Wert.net" });
            //Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Clinker brick 1400 kg/m3", Type = @"Masonry", Conductivity = 0.58, Density = 1400, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"U-Wert.net" });
            //Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Clinker brick 1600 kg/m3", Type = @"Masonry", Conductivity = 0.68, Density = 1600, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"U-Wert.net" });
            //Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Clinker brick 1800 kg/m3", Type = @"Masonry", Conductivity = 0.81, Density = 1800, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"U-Wert.net" });
            //Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Clinker brick 2000 kg/m3", Type = @"Masonry", Conductivity = 0.96, Density = 2000, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"U-Wert.net" });
            //Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Aerated concrete 350kg/m3", Type = @"Masonry", Conductivity = 0.09, Density = 350, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"U-Wert.net" });
            //Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Aerated concrete 500kg/m3", Type = @"Masonry", Conductivity = 0.12, Density = 500, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"U-Wert.net" });
            //Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Ytong block W PP 1.6-0.30", Type = @"Masonry", Conductivity = 0.08, Density = 300, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"U-Wert.net" });
            //Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Poroton Plan-T10", Type = @"Masonry", Conductivity = 0.1, Density = 650, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"U-Wert.net" });
            //Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Rigid foam  EPS 035", Type = @"Insulation", Conductivity = 0.035, Density = 30, SpecificHeat = 1500, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net]" });
            //Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Rigid foam PUR {no coating}", Type = @"Insulation", Conductivity = 0.03, Density = 30, SpecificHeat = 1400, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net]" });
            //Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Rigid foam PUR {alu coating}", Type = @"Insulation", Conductivity = 0.025, Density = 30, SpecificHeat = 1400, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net]" });
            //Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Rigid foam PUR {fleece coating}", Type = @"Insulation", Conductivity = 0.028, Density = 30, SpecificHeat = 1400, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net]" });
            //Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Wood fiber insulating board", Type = @"Insulation", Conductivity = 0.042, Density = 160, SpecificHeat = 2100, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net]" });
            //Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Styrofoam", Type = @"Insulation", Conductivity = 0.04, Density = 20, SpecificHeat = 1500, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net]" });
            //Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Vacuum insulation panel Variotec", Type = @"Insulation", Conductivity = 0.007, Density = 205, SpecificHeat = 900, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net]" });






            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Pine wood", Type = @"Wood", Conductivity = 0.13, Density = 520, SpecificHeat = 1600, ThermalEmittance = 0.9, SolarAbsorptance = 0.7, VisibleAbsorptance = 0.7, EmbodiedEnergy = 10, EmbodiedCarbon = 0.71, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net][LCA  ICE (0.31fos + 0.41bio-embodied cabon emissions)]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Douglas fir", Type = @"Wood", Conductivity = 0.12, Density = 530, SpecificHeat = 1600, ThermalEmittance = 0.9, SolarAbsorptance = 0.7, VisibleAbsorptance = 0.7, EmbodiedEnergy = 10, EmbodiedCarbon = 0.71, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net][LCA  ICE (0.31fos + 0.41bio-embodied cabon emissions)]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Oak", Type = @"Wood", Conductivity = 0.18, Density = 690, SpecificHeat = 2400, ThermalEmittance = 0.9, SolarAbsorptance = 0.7, VisibleAbsorptance = 0.7, EmbodiedEnergy = 10, EmbodiedCarbon = 0.71, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net][LCA  ICE (0.31fos + 0.41bio-embodied cabon emissions)]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Spruce", Type = @"Wood", Conductivity = 0.13, Density = 450, SpecificHeat = 1600, ThermalEmittance = 0.9, SolarAbsorptance = 0.7, VisibleAbsorptance = 0.7, EmbodiedEnergy = 10, EmbodiedCarbon = 0.71, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net][LCA  ICE (0.31fos + 0.41bio-embodied cabon emissions)]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Larch", Type = @"Wood", Conductivity = 0.13, Density = 460, SpecificHeat = 1600, ThermalEmittance = 0.9, SolarAbsorptance = 0.7, VisibleAbsorptance = 0.7, EmbodiedEnergy = 10, EmbodiedCarbon = 0.71, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net][LCA  ICE (0.31fos + 0.41bio-embodied cabon emissions)]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Oriented strand board", Type = @"Wood", Conductivity = 0.13, Density = 650, SpecificHeat = 1700, ThermalEmittance = 0.9, SolarAbsorptance = 0.7, VisibleAbsorptance = 0.7, EmbodiedEnergy = 15, EmbodiedCarbon = 0.96, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net][LCA  ICE (0.45fos + 0.54 bio- embodied carbon emissions)]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Medium density fiberboard", Type = @"Wood", Conductivity = 0.09, Density = 500, SpecificHeat = 1700, ThermalEmittance = 0.9, SolarAbsorptance = 0.7, VisibleAbsorptance = 0.7, EmbodiedEnergy = 11, EmbodiedCarbon = 0.72, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net][LCA  ICE (0.39fos + 0.35bio- embodied carbon emissions)]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Chipboard", Type = @"Wood", Conductivity = 0.14, Density = 650, SpecificHeat = 1800, ThermalEmittance = 0.9, SolarAbsorptance = 0.7, VisibleAbsorptance = 0.7, EmbodiedEnergy = 10, EmbodiedCarbon = 0.71, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net][LCA  ICE (0.39fos + 0.35bio- embodied carbon emissions)]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"General Concrete", Type = @"Concrete", Conductivity = 2, Density = 2400, SpecificHeat = 950, ThermalEmittance = 0.9, SolarAbsorptance = 0.7, VisibleAbsorptance = 0.7, EmbodiedEnergy = 0.75, EmbodiedCarbon = 0.1, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net][LCA  ICE, General Concrete data]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Concrete reinforced 20-30 MPa", Type = @"Concrete", Conductivity = 2, Density = 2400, SpecificHeat = 950, ThermalEmittance = 0.9, SolarAbsorptance = 0.7, VisibleAbsorptance = 0.7, EmbodiedEnergy = 0.75, EmbodiedCarbon = 0.1, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] General concrete data used for density through visible absorptance. Data averaged for embodied enegy, embodied carbon and embodied carbon emissions." });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Concrete reinforced 30-50 MPa", Type = @"Concrete", Conductivity = 2, Density = 2400, SpecificHeat = 950, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0.74, EmbodiedCarbon = 0.099, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net]General concrete data used for density through visible absorptance. Data averaged for embodied enegy, embodied carbon and embodied carbon emissions." });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Asphalt low binder content", Type = @"Screed", Conductivity = 0.75, Density = 2350, SpecificHeat = 920, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 3.39, EmbodiedCarbon = 0.191, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] General asphalt data used for density through visible absorptance, averaged data for embodied energy through embodied carbon emissions" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Asphalt high binder content", Type = @"Screed", Conductivity = 0.75, Density = 2350, SpecificHeat = 920, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 4.46, EmbodiedCarbon = 0.216, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] General asphalt data used for density through visible absorptance, averaged data for embodied energy through embodied carbon emissions" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Lightweight concrete", Type = @"Concrete", Conductivity = 1.3, Density = 1800, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0.78, EmbodiedCarbon = 0.106, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE Concrete 20/25 Mpa]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Cement screed", Type = @"Screed", Conductivity = 1.4, Density = 2000, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 1.33, EmbodiedCarbon = 0.221, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [ LCA ICE, Mortar (1:3 cement:sand mix) ]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Basalt", Type = @"Masonry", Conductivity = 3.5, Density = 2850, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 1.26, EmbodiedCarbon = 0.073, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE General Stone]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Granite", Type = @"Masonry", Conductivity = 2.8, Density = 2600, SpecificHeat = 790, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 11, EmbodiedCarbon = 0.64, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE Granite]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Lime stone", Type = @"Masonry", Conductivity = 1.4, Density = 2000, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 1.5, EmbodiedCarbon = 0.087, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE, Limestone]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Adobe 1500kg_m3", Type = @"Masonry", Conductivity = 0.66, Density = 1500, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 3, EmbodiedCarbon = 0.23, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA ICE,  General Clay data]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Sand stone", Type = @"Masonry", Conductivity = 2.3, Density = 2600, SpecificHeat = 710, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 1, EmbodiedCarbon = 0.058, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE Sandstone]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Clinker brick 1400kg_m3", Type = @"Masonry", Conductivity = 0.58, Density = 1400, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 3, EmbodiedCarbon = 0.23, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE General (Common Brick)]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Clinker brick 1600kg_m3", Type = @"Masonry", Conductivity = 0.68, Density = 1600, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 3, EmbodiedCarbon = 0.23, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net]  [LCA, ICE General (Common Brick)]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Clinker brick 1800kg_m3", Type = @"Masonry", Conductivity = 0.81, Density = 1800, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 3, EmbodiedCarbon = 0.23, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net]  [LCA, ICE General (Common Brick)]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Clinker brick 2000kg_m3", Type = @"Masonry", Conductivity = 0.96, Density = 2000, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 3, EmbodiedCarbon = 0.23, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net]  [LCA, ICE General (Common Brick)]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Aerated concrete 350kg_m3", Type = @"Masonry", Conductivity = 0.09, Density = 350, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 3.5, EmbodiedCarbon = 0.3075, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE Autoclaved Aerated Blocks (ACC's)]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Aerated concrete 500kg_m3", Type = @"Masonry", Conductivity = 0.12, Density = 500, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 3.5, EmbodiedCarbon = 0.3075, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE Autoclaved Aerated Blocks (ACC's)]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Ytong block W PP 1.6-0.30", Type = @"Masonry", Conductivity = 0.08, Density = 300, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 3.5, EmbodiedCarbon = 0.31, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE Autoclaved Aerated Blocks (ACC's)]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Poroton Plan-T10", Type = @"Masonry", Conductivity = 0.1, Density = 650, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 3, EmbodiedCarbon = 0.23, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE General Common Brick]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Rigid foam  EPS 035", Type = @"Insulation", Conductivity = 0.035, Density = 30, SpecificHeat = 1500, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 101.5, EmbodiedCarbon = 3.48, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE Polyurethane Rigid Foam]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Rigid foam PUR no coating", Type = @"Insulation", Conductivity = 0.03, Density = 30, SpecificHeat = 1400, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 101.5, EmbodiedCarbon = 3.48, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE Polyurethane Rigid Foam]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Rigid foam PUR alu coating", Type = @"Insulation", Conductivity = 0.025, Density = 30, SpecificHeat = 1400, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 101.5, EmbodiedCarbon = 3.48, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE Polyurethane Rigid Foam]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Rigid foam PUR fleece coating", Type = @"Insulation", Conductivity = 0.028, Density = 30, SpecificHeat = 1400, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 101.5, EmbodiedCarbon = 3.48, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE Polyurethane Rigid Foam]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Wood fiber insulating board", Type = @"Insulation", Conductivity = 0.042, Density = 160, SpecificHeat = 2100, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 45, EmbodiedCarbon = 1.86, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE General Insulation]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Styrofoam", Type = @"Insulation", Conductivity = 0.04, Density = 20, SpecificHeat = 1500, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 101.5, EmbodiedCarbon = 3.48, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] No data found" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Vacuum insulation panel Variotec", Type = @"Insulation", Conductivity = 0.007, Density = 205, SpecificHeat = 900, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 45, EmbodiedCarbon = 1.86, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE, General Insulation]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Wood wool board 15mm", Type = @"Wood", Conductivity = 0.09, Density = 570, SpecificHeat = 2100, ThermalEmittance = 0.9, SolarAbsorptance = 0.7, VisibleAbsorptance = 0.7, EmbodiedEnergy = 20, EmbodiedCarbon = 0.98, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE, Woodwool (Board)]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Wood wool board 25mm", Type = @"Wood", Conductivity = 0.09, Density = 460, SpecificHeat = 2100, ThermalEmittance = 0.9, SolarAbsorptance = 0.7, VisibleAbsorptance = 0.7, EmbodiedEnergy = 20, EmbodiedCarbon = 0.98, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE, Woodwool (Board)]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Wood wool board 35mm", Type = @"Wood", Conductivity = 0.09, Density = 415, SpecificHeat = 2100, ThermalEmittance = 0.9, SolarAbsorptance = 0.7, VisibleAbsorptance = 0.7, EmbodiedEnergy = 20, EmbodiedCarbon = 0.98, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net][LCA, ICE, Woodwool (Board)]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Wood wool board 50mm", Type = @"Wood", Conductivity = 0.09, Density = 390, SpecificHeat = 2100, ThermalEmittance = 0.9, SolarAbsorptance = 0.7, VisibleAbsorptance = 0.7, EmbodiedEnergy = 20, EmbodiedCarbon = 0.98, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE, Woodwool (Board)]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Light adobe NF 700", Type = @"Masonry", Conductivity = 0.21, Density = 700, SpecificHeat = 1200, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 3, EmbodiedCarbon = 0.23, Cost = 0, Comment = @"U-Wert.net [LCA, ICE General (Simple Clay Baked Products)" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Light adobe NF 1200", Type = @"Masonry", Conductivity = 0.47, Density = 1200, SpecificHeat = 1200, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 3, EmbodiedCarbon = 0.23, Cost = 0, Comment = @"U-Wert.net [LCA, ICE General (Simple Clay Baked Products)" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Light adobe NF 1800", Type = @"Masonry", Conductivity = 0.91, Density = 1800, SpecificHeat = 1200, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 3, EmbodiedCarbon = 0.23, Cost = 0, Comment = @"U-Wert.net [LCA, ICE General (Simple Clay Baked Products)" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Shale", Type = @"Masonry", Conductivity = 2.2, Density = 2400, SpecificHeat = 760, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0.03, EmbodiedCarbon = 0.002, Cost = 0, Comment = @"U-Wert.net [LCA, ICE Shale]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Clinker brick", Type = @"Masonry", Conductivity = 0.96, Density = 2000, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 3, EmbodiedCarbon = 0.23, Cost = 0, Comment = @"U-Wert.net [LCA, ICE General Common Bricks]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Ytong block W PP 2-0.35", Type = @"Masonry", Conductivity = 0.09, Density = 350, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 3.5, EmbodiedCarbon = 0.31, Cost = 0, Comment = @"U-Wert.net [LCA, ICE Autoclaved Aerated Blocks (AAC's)]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Ytong block W PP 2-0.40", Type = @"Masonry", Conductivity = 0.1, Density = 400, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 3.5, EmbodiedCarbon = 0.31, Cost = 0, Comment = @"U-Wert.net [LCA, ICE Autoclaved Aerated Blocks (AAC's)]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Ytong block W PP 4-0.50", Type = @"Masonry", Conductivity = 0.12, Density = 500, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 3.5, EmbodiedCarbon = 0.31, Cost = 0, Comment = @"U-Wert.net [LCA, ICE Autoclaved Aerated Blocks (AAC's)]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Ytong block W PP 4-0.55", Type = @"Masonry", Conductivity = 0.14, Density = 550, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 3.5, EmbodiedCarbon = 0.31, Cost = 0, Comment = @"U-Wert.net [LCA, ICE Autoclaved Aerated Blocks (AAC's)]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Ytong block W PP 4-0.60", Type = @"Masonry", Conductivity = 0.16, Density = 600, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 3.5, EmbodiedCarbon = 0.31, Cost = 0, Comment = @"U-Wert.net [LCA, ICE Autoclaved Aerated Blocks (AAC's)]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Ytong block W PP 6-0.65", Type = @"Masonry", Conductivity = 0.18, Density = 650, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 3.5, EmbodiedCarbon = 0.31, Cost = 0, Comment = @"U-Wert.net [LCA, ICE Autoclaved Aerated Blocks (AAC's)]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Poroton T12", Type = @"Masonry", Conductivity = 0.12, Density = 650, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 3, EmbodiedCarbon = 0.23, Cost = 0, Comment = @"U-Wert.net [LCA, ICE General Common Bricks]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Cork", Type = @"Insulation", Conductivity = 0.05, Density = 160, SpecificHeat = 1800, ThermalEmittance = 0.9, SolarAbsorptance = 0.78, VisibleAbsorptance = 0.78, EmbodiedEnergy = 4, EmbodiedCarbon = 0.19, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net][ThermalEmittance SolarAbsorptance VisibleAbsorptance: DesignBuilderv3] [LCA, ICE Cork]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Ytong Multipor DAA ds", Type = @"Insulation", Conductivity = 0.047, Density = 115, SpecificHeat = 1300, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 45, EmbodiedCarbon = 1.86, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE General Insulation]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Ytong Multipor WI WTR DI", Type = @"Insulation", Conductivity = 0.042, Density = 90, SpecificHeat = 1300, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 45, EmbodiedCarbon = 1.86, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE General Insulation]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Ytong Multipor WAP", Type = @"Insulation", Conductivity = 0.045, Density = 110, SpecificHeat = 1300, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 45, EmbodiedCarbon = 1.86, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE General Insulation]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Foam glass", Type = @"Insulation", Conductivity = 0.056, Density = 130, SpecificHeat = 750, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 27, EmbodiedCarbon = 0, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE, Celular Glass]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Reed", Type = @"Insulation", Conductivity = 0.065, Density = 225, SpecificHeat = 1200, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0.24, EmbodiedCarbon = 0.01, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net][ LCA, ICE, Straw]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Vacuum insulation panel Vacupor  NT-B2-S", Type = @"Insulation", Conductivity = 0.007, Density = 190, SpecificHeat = 1050, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 45, EmbodiedCarbon = 1.86, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE General Insulation]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Stainless Steel", Type = @"Metal", Conductivity = 45, Density = 7800, SpecificHeat = 480, ThermalEmittance = 0.1, SolarAbsorptance = 0.4, VisibleAbsorptance = 0.4, EmbodiedEnergy = 56.7, EmbodiedCarbon = 6.15, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [ LCA ICE, Stainless Steel]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Steel", Type = @"Metal", Conductivity = 45, Density = 7800, SpecificHeat = 480, ThermalEmittance = 0.1, SolarAbsorptance = 0.4, VisibleAbsorptance = 0.4, EmbodiedEnergy = 20.1, EmbodiedCarbon = 1.46, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [ LCA ICE, General Steel ]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Rammed Earth", Type = @"Masonry", Conductivity = 0.75, Density = 1730, SpecificHeat = 880, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @" [lambda  rho c: U-Wert.net] [LCA, ICE Mud] [LCA, ICE, Single Clay Brick]" });










            #endregion

            //List of premade basic glazing materials/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            double[] a3 = { 1 };
            GlazingMaterial gm1 = new GlazingMaterial()
            {
                Name = "Generic Clear Glass 6mm",
                Type = "Uncoated",
                Conductivity = 0.9,
                Density = 2500,
                EmbodiedEnergy = 15,
                EmbodiedCarbon = 0.85,
                Cost = 0.0,
                Comment = "",
                SolarTransmittance = 0.68,
                SolarReflectanceFront = 0.09,
                SolarReflectanceBack = 0.10,
                VisibleTransmittance = 0.81,
                VisibleReflectanceFront = 0.11,
                VisibleReflectanceBack = 0.12,
                IRTransmittance = 0.00,
                IREmissivityFront = 0.84,
                IREmissivityBack = 0.20
            };
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
            oc1.Type = ConstructionTypes.Facade;// "Facade";

            Library.OpaqueConstructions.Add( oc1);

            //Basic roof

            Layer<OpaqueMaterial> ol3 = new Layer<OpaqueMaterial>(0.20, defaultMat);
            Layer<OpaqueMaterial> ol4 = new Layer<OpaqueMaterial>(0.12, mo2);

            OpaqueConstruction oc2 = new OpaqueConstruction();

            oc2.Layers.Add(ol1);
            oc2.Layers.Add(ol2);
            oc2.Name = "Heavy Roof";
            oc2.Type = ConstructionTypes.Roof;// "Roof";

            Library.OpaqueConstructions.Add(oc2);

            //Basic floor

            Layer<OpaqueMaterial> ol5 = new Layer<OpaqueMaterial>(0.20, defaultMat);

            OpaqueConstruction oc3 = new OpaqueConstruction();

            oc3.Layers.Add(ol1);
            oc3.Name = "Heavy Floor";
            oc3.Type = ConstructionTypes.InteriorFloor;// "Floor";

            Library.OpaqueConstructions.Add( oc3);


            //List of premade basic glazing constructions/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //Basic double glazing

            Layer<WindowMaterialBase> go1 = new Layer<WindowMaterialBase>(0.006, gm1);
            Layer<WindowMaterialBase> go2 = new Layer<WindowMaterialBase>(0.0013, Library.GasMaterials.Single(o=>o.Name == "ARGON"));

            GlazingConstruction gc1 = new GlazingConstruction();

            gc1.Layers.Add(go1);
            gc1.Layers.Add(go2);
            gc1.Layers.Add(go1);
            gc1.Name = "DblClear Air 6_13_6";
            gc1.Type = GlazingConstructionTypes.Double;// "Double";

            Library.GlazingConstructions.Add( gc1);




            #region schedules

            //--------------------------------------------------------------------------------schedules


            QuickScheduleAdd("occBedroom", new double[] { 1, 1, 1, 1, 1, 1, 0.8, 0.6, 0.4, 0.4, 0.4, 0.6, 0.8, 0.6, 0.4, 0.4, 0.6, 0.8, 0.8, 0.8, 0.8, 1, 1, 1 },  "Residential" , "SIA Merkblatt 2024", ref Library);
            QuickScheduleAdd("equipBedroom", new double[] { 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.5, 1, 0.5, 0.5, 0.5, 1, 1, 0.5, 0.5, 0.5, 1, 1, 1, 1, 0.5, 0.5, 0.5, 0.1 }, "Residential" , "SIA Merkblatt 2024", ref Library);
            QuickScheduleAdd("lightsBedroom", new double[] { 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0 }, "Residential", "based on SIA Merkblatt 2024 Occ Schedule", ref Library);


            QuickScheduleAdd("occKitchen", new double[] { 0, 0, 0, 0, 0, 0, 0.4, 0.8, 0.4, 0, 0, 0.4, 0.8, 0.4, 0, 0, 0.4, 1, 0.6, 0.4, 0, 0, 0, 0 }, "Residential" , "SIA Merkblatt 2024", ref Library);
            QuickScheduleAdd("equipmentKitchen", new double[] { 0.2, 0.2, 0.2, 0.2, 0.2, 0.2, 0.4, 0.8, 0.4, 0.2, 0.2, 0.4, 1, 0.4, 0.2, 0.2, 0.4, 1, 0.4, 0.2, 0.2, 0.2, 0.2, 0.2 }, "Residential",  "SIA Merkblatt 2024", ref Library);
            QuickScheduleAdd("lightsKitchen", new double[] { 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0 }, "Residential", "based on SIA Merkblatt 2024 Occ Schedule", ref Library);


            QuickScheduleAdd("occOffice", new double[] { 0, 0, 0, 0, 0, 0, 0, 0.2, 0.4, 0.6, 0.8, 0.8, 0.4, 0.6, 0.8, 0.8, 0.4, 0.2, 0, 0, 0, 0, 0, 0 }, new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, "Office","SIA Merkblatt 2024", ref Library);
            QuickScheduleAdd("equipmentOffice", new double[] { 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.2, 0.4, 0.6, 0.8, 0.8, 0.4, 0.6, 0.8, 0.8, 0.4, 0.2, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1 }, new double[] { 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1 } , "Office", "SIA Merkblatt 2024", ref Library);
            QuickScheduleAdd("lightsOffice", new double[] { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0 }, new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, "Office", "based on SIA Merkblatt 2024 Occ Schedule", ref Library);


            QuickScheduleAdd("occMeetingRoom", new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.6, 1, 0.4, 0, 0, 0.6, 1, 0.4, 0, 0, 0, 0, 0, 0, 0 }, new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, "Office",  "SIA Merkblatt 2024", ref Library);
            QuickScheduleAdd("equipmentMeetingRoom", new double[] { 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.2, 0.4, 0.6, 0.8, 0.8, 0.4, 0.6, 0.8, 0.8, 0.4, 0.2, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1 }, new double[] { 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1 }, "Office", "SIA Merkblatt 2024", ref Library);
            QuickScheduleAdd("lightsMeetingRoom", new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 }, new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, "Office", "based on SIA Merkblatt 2024 Occ Schedule", ref Library);


            QuickScheduleAdd("occLibrary", new double[] { 0, 0, 0, 0, 0, 0, 0, 0.2, 0.6, 1, 1, 0.2, 0.2, 1, 1, 0.6, 0.2, 0, 0, 0, 0, 0, 0, 0 }, "Education", "SIA Merkblatt 2024", ref Library);
            QuickScheduleAdd("equipmentLibrary", new double[] { 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.2, 0.6, 1, 1, 0.2, 0.2, 1, 1, 0.6, 0.4, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1 }, "Education", "SIA Merkblatt 2024", ref Library);
            QuickScheduleAdd("lightsLibrary", new double[] { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 }, "Education", "based on SIA Merkblatt 2024 Occ Schedule", ref Library);


            QuickScheduleAdd("occLectureHall", new double[] { 0, 0, 0, 0, 0, 0, 0, 0.2, 0.6, 1, 1, 0.2, 0.2, 1, 1, 0.6, 0.2, 0, 0, 0, 0, 0, 0, 0 }, new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, "Education", "SIA Merkblatt 2024", ref Library);
            QuickScheduleAdd("equipmentLectureHall", new double[] { 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.2, 0.6, 1, 1, 0.2, 0.2, 1, 1, 0.6, 0.4, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1 }, new double[] { 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1 }, "Education", "SIA Merkblatt 2024", ref Library);
            QuickScheduleAdd("lightsLectureHall", new double[] { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 }, new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, "Education", "based on SIA Merkblatt 2024 Occ Schedule", ref Library);


            QuickScheduleAdd("occSuperMarket", new double[] { 0, 0, 0, 0, 0, 0, 0, 0.2, 0.4, 0.4, 0.4, 0.6, 0.6, 0.6, 0.4, 0.4, 0.6, 0.8, 0.6, 0, 0, 0, 0, 0 }, "Commercial", "SIA Merkblatt 2024", ref Library);
            QuickScheduleAdd("equipmentSuperMarket", new double[] { 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0.1, 0.1, 0.1, 0.1, 0.1 }, "Commercial", "SIA Merkblatt 2024", ref Library);
            QuickScheduleAdd("lightsSuperMarket", new double[] { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0 }, "Commercial", "based on SIA Merkblatt 2024 Occ Schedule", ref Library);


            QuickScheduleAdd("occShopping", new double[] { 0, 0, 0, 0, 0, 0, 0, 0.2, 0.4, 0.4, 0.4, 0.6, 0.6, 0.6, 0.4, 0.4, 0.6, 0.8, 0.6, 0, 0, 0, 0, 0 }, "Commercial", "SIA Merkblatt 2024", ref Library);
            QuickScheduleAdd("equipmentShopping", new double[] { 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0.1, 0.1, 0.1, 0.1, 0.1 }, "Commercial", "SIA Merkblatt 2024", ref Library);
            QuickScheduleAdd("lightsShopping", new double[] { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0 }, "Commercial", "based on SIA Merkblatt 2024 Occ Schedule", ref Library);





            #endregion



            Library.ZoneLoads.Add(new ZoneLoad() { Name = "BedroomLoads", PeopleDensity = 1.0 / 40.0, IlluminanceTarget = 200, LightingPowerDensity = 9.5, EquipmentPowerDensity = 2, OccupancySchedule = "occBedroom", EquipmentAvailibilitySchedule = "equipBedroom", LightsAvailibilitySchedule = "lightsBedroom", Category = "Residential" ,DataSource = "SIA Merkblatt 2024" });
            Library.ZoneLoads.Add(new ZoneLoad() { Name = "KitchenLoads", PeopleDensity = 1.0 / 5.0, IlluminanceTarget = 500, LightingPowerDensity = 17, EquipmentPowerDensity = 40, OccupancySchedule = "occKitchen", EquipmentAvailibilitySchedule = "equipKitchen", LightsAvailibilitySchedule = "lightsKitchen", Category = "Residential" ,DataSource = "SIA Merkblatt 2024" });
            Library.ZoneLoads.Add(new ZoneLoad() { Name = "SingleOfficeLoads", PeopleDensity = 1.0 / 14.0, IlluminanceTarget = 500, LightingPowerDensity = 16, EquipmentPowerDensity = 7, OccupancySchedule = "occOffice", EquipmentAvailibilitySchedule = "equipOffice", LightsAvailibilitySchedule = "lightsOffice", Category = "Office", DataSource = "SIA Merkblatt 2024" });
            Library.ZoneLoads.Add(new ZoneLoad() { Name = "MeetingRoomLoads", PeopleDensity = 1.0 / 3.0, IlluminanceTarget = 500, LightingPowerDensity = 16, EquipmentPowerDensity = 2, OccupancySchedule = "occMeetingRoom", EquipmentAvailibilitySchedule = "equipMeetingRoom", LightsAvailibilitySchedule = "lightsMeetingRoom", Category = "Office", DataSource = "SIA Merkblatt 2024" });

            Library.ZoneConditionings.Add(new ZoneConditioning() { Name = "BedroomHeatingCoolingMechVent", HeatingSetpoint = 19, CoolingSetpoint = 26,  MechVentIsOn = true, MinFreshAirPerson = 2.5, MinFreshAirArea = 0.3, Category = "Residential" });
            Library.ZoneConditionings.Add(new ZoneConditioning() { Name = "KitchenHeatingCoolingMechVent", HeatingSetpoint = 20, CoolingSetpoint = 26, MechVentIsOn = true, MinFreshAirPerson = 3.9, MinFreshAirArea = 0.9, Category = "Residential" });
            Library.ZoneConditionings.Add(new ZoneConditioning() { Name = "OfficeHeatingCoolingMechVent", HeatingSetpoint = 20, CoolingSetpoint = 26, MechVentIsOn = true, MinFreshAirPerson = 2.5, MinFreshAirArea = 0.3, Category = "Office" });
            Library.ZoneConditionings.Add(new ZoneConditioning() { Name = "MeetingRoomHeatingCoolingMechVent", HeatingSetpoint = 20, CoolingSetpoint = 26, MechVentIsOn = true, MinFreshAirPerson = 3.8, MinFreshAirArea = 0.3, Category = "Office" });
            Library.ZoneConditionings.Add(new ZoneConditioning() { Name = "LectureHallHeatingCoolingMechVent", HeatingSetpoint = 20, CoolingSetpoint = 26, MechVentIsOn = true, MinFreshAirPerson = 3.8, MinFreshAirArea = 0.3, Category = "Education" });

            Library.ZoneVentilations.Add(new ZoneVentilation() { Name = "PoorAirTightness", InfiltrationIsOn = true,  InfiltrationAch = 1.2, InfiltrationModel = InfiltrationModel.Wind, Category = "Infiltration" });
            Library.ZoneVentilations.Add(new ZoneVentilation() { Name = "ModerateAirTightness", InfiltrationIsOn = true, InfiltrationAch = 0.5, InfiltrationModel = InfiltrationModel.Wind, Category = "Infiltration" });
            Library.ZoneVentilations.Add(new ZoneVentilation() { Name = "GoodAirTightness", InfiltrationIsOn = true, InfiltrationAch = 0.15, InfiltrationModel = InfiltrationModel.Wind, Category = "Infiltration" });



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


            #region ZoneDefTemplates


            


            Library.ZoneDefinitions.Add(new ZoneDefinition());



            #endregion


            ////--------------------------------------------------------------------------------serialization test


            //string xml = SerializeDeserialize.Serialize(Library);

            //SerializeDeserialize.Deserialize(xml, typeof(ArchsimLib.Lib));

            ////--------------------------------------------------------------------------------out

            return Library;
        }


        private static void QuickScheduleAdd(string Name ,double[] dayArray, string category, string dataSource ,ref Library Library) {


            int[] MonthFrom = { 1 };
            int[] DayFrom = { 1 };
            int[] MonthTo = { 12 };
            int[] DayTo = { 31 };

            DaySchedule someDaySchedule = new DaySchedule(Name, "Fraction", dayArray.ToList());
            someDaySchedule.DataSource = dataSource;
            someDaySchedule.Category = category;
            Library.DaySchedules.Add(someDaySchedule);
            DaySchedule[] daySchedulesArray = { someDaySchedule, someDaySchedule, someDaySchedule, someDaySchedule, someDaySchedule, someDaySchedule, someDaySchedule };
            WeekSchedule someWeekSchedule = new WeekSchedule(Name, daySchedulesArray, "Fraction");
            someWeekSchedule.DataSource = dataSource;
            someWeekSchedule.Category = category;
            Library.WeekSchedules.Add(someWeekSchedule);
            WeekSchedule[] weekSchedulesArray = { someWeekSchedule };
            YearSchedule someYearSchedule = new YearSchedule(Name, "Fraction", weekSchedulesArray.ToList(), MonthFrom.ToList(), DayFrom.ToList(), MonthTo.ToList(), DayTo.ToList());
            someYearSchedule.DataSource = dataSource;
            someYearSchedule.Category = category;
            Library.YearSchedules.Add(someYearSchedule);

        }

        private static void QuickScheduleAdd(string Name, double[] dayArray, double[] weArray, string category, string dataSource, ref Library Library)
        {


            int[] MonthFrom = { 1 };
            int[] DayFrom = { 1 };
            int[] MonthTo = { 12 };
            int[] DayTo = { 31 };

            DaySchedule someDaySchedule = new DaySchedule(Name, "Fraction", dayArray.ToList());
            someDaySchedule.DataSource = dataSource;
            someDaySchedule.Category = category;
            Library.DaySchedules.Add(someDaySchedule);
            DaySchedule weSchedule = new DaySchedule(Name+"WeekEnd", "Fraction", dayArray.ToList());
            weSchedule.DataSource = dataSource;
            weSchedule.Category = category;
            Library.DaySchedules.Add(someDaySchedule);
            DaySchedule[] daySchedulesArray = { someDaySchedule, someDaySchedule, someDaySchedule, someDaySchedule, someDaySchedule, weSchedule, weSchedule };
            WeekSchedule someWeekSchedule = new WeekSchedule(Name, daySchedulesArray, "Fraction");
            someWeekSchedule.DataSource = dataSource;
            someWeekSchedule.Category = category;
            Library.WeekSchedules.Add(someWeekSchedule);
            WeekSchedule[] weekSchedulesArray = { someWeekSchedule };
            YearSchedule someYearSchedule = new YearSchedule(Name, "Fraction", weekSchedulesArray.ToList(), MonthFrom.ToList(), DayFrom.ToList(), MonthTo.ToList(), DayTo.ToList());
            someYearSchedule.DataSource = dataSource;
            someYearSchedule.Category = category;
            Library.YearSchedules.Add(someYearSchedule);

        }

    }
}
