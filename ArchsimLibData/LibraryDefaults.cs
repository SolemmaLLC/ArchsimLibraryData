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
                        Debug.WriteLine("Loading defualt library from " + Utilities.AssemblyDirectory + @"\" + DefaultLibrary.FileName);
                    }

            }
            catch (Exception ex) {
             Debug.WriteLine("Default library loading error " + ex.Message);
            }


            try {
                    if(Library == null)
                    {
                      Debug.WriteLine("No default library found at " + Utilities.AssemblyDirectory + @"\" + DefaultLibrary.FileName + " Resetting default library.");
                        Library = LibraryDefaults.getHardCodedDefaultLib();
                        writeDefaultLibrary(Library);
                    }
            
            }
            catch (Exception ex) {
                Debug.WriteLine("Default library resetting error " + ex.Message);
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
                Debug.WriteLine("Library written to " + rootPath  + @"\" + DefaultLibrary.FileName);
            }
            catch { Debug.WriteLine("Library writing error"); }
        }


        // hardcoded default library
        public static Library getHardCodedDefaultLib()
        {
            Library Library = new Library();

            #region DEFAULTS - MUST BE IN LIBRARY

            OpaqueMaterial defaultMat = new OpaqueMaterial() {
            Name ="defaultMat",
            Category ="Concrete",
                Conductivity= 2.30,
                Density =2400,
                SpecificHeat=840,
                ThermalEmittance=0.9,
                SolarAbsorptance= 0.7,
                VisibleAbsorptance=0.7
            };
            Library.Add(defaultMat);

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
            Library.Add( defaultGMat);



            Layer<OpaqueMaterial> defaultLay = new Layer<OpaqueMaterial>(0.25, defaultMat);
            OpaqueConstruction defaultConstruction = new OpaqueConstruction();
            defaultConstruction.Layers.Add(defaultLay);
            defaultConstruction.Name = "defaultConstruction";
            defaultConstruction.Type = ConstructionTypes.Facade;// "Facade";
            Library.Add( defaultConstruction);


            Layer<WindowMaterialBase> defaultGLay = new Layer<WindowMaterialBase>(0.006, defaultGMat);
            GlazingConstruction defaultGlazing = new GlazingConstruction();
            defaultGlazing.Layers.Add(defaultGLay);
            defaultGlazing.Name = "defaultGlazing";
            defaultGlazing.Type = GlazingConstructionTypes.Single;// "Single";
            Library.Add( defaultGlazing);


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
            Library.Add(AirWallMat);
            Layer<WindowMaterialBase> airwallLayer = new Layer<WindowMaterialBase>(0.003, AirWallMat);
            GlazingConstruction airWall = new GlazingConstruction();
            airWall.Layers.Add(airwallLayer);
            airWall.Name = "Airwall";
            airWall.Type = GlazingConstructionTypes.Other;// "Other";
            Library.Add( airWall);




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




            Library.Add(new ZoneLoad() { Name = "NoLoads", PeopleIsOn = false, EquipmentIsOn = false , LightsIsOn = false, PeopleDensity = 0, IlluminanceTarget = 0, LightingPowerDensity = 0, EquipmentPowerDensity = 0, OccupancySchedule = "AllOn", EquipmentAvailibilitySchedule = "AllOn", LightsAvailibilitySchedule = "AllOn", Category = "Default", DataSource = "Default" });
            Library.Add(new ZoneConditioning() { Name = "NoConditioning", HeatIsOn=false, CoolIsOn =false, MechVentIsOn = false, HeatingSetpoint = 19, CoolingSetpoint = 26,  MinFreshAirPerson = 0, MinFreshAirArea = 0, Category = "Default" });
            Library.Add(new ZoneVentilation() { Name = "NoVentilationInfiltration", InfiltrationIsOn = false, InfiltrationAch = 0.15, InfiltrationModel = InfiltrationModel.Wind, Category = "Infiltration" });
            Library.Add(new DomHotWater() { Name = "NoDHW", IsOn = false, FlowRatePerFloorArea = 0, WaterSchedule = "AllOn", WaterTemperatureInlet = 10, WaterSupplyTemperature = 60 });
            Library.Add(new ZoneConstruction() { Name = "Default", RoofConstruction = "defaultConstruction", FacadeConstruction = "defaultConstruction", SlabConstruction = "defaultConstruction", GroundConstruction = "defaultConstruction", PartitionConstruction = "defaultConstruction" });


            #endregion

            #endregion











            #region  OpaqueMaterialsWithEEIndicators


            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Pine wood", Category = @"Timber", Conductivity = 0.13, Density = 520, SpecificHeat = 1600, ThermalEmittance = 0.9, SolarAbsorptance = 0.7, VisibleAbsorptance = 0.7, EmbodiedEnergy = 10, EmbodiedCarbon = 0.71, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net][LCA  ICE (0.31fos + 0.41bio-embodied cabon emissions)]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Douglas fir", Category = @"Timber", Conductivity = 0.12, Density = 530, SpecificHeat = 1600, ThermalEmittance = 0.9, SolarAbsorptance = 0.7, VisibleAbsorptance = 0.7, EmbodiedEnergy = 10, EmbodiedCarbon = 0.71, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net][LCA  ICE (0.31fos + 0.41bio-embodied cabon emissions)]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Oak", Category = @"Timber", Conductivity = 0.18, Density = 690, SpecificHeat = 2400, ThermalEmittance = 0.9, SolarAbsorptance = 0.7, VisibleAbsorptance = 0.7, EmbodiedEnergy = 10, EmbodiedCarbon = 0.71, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net][LCA  ICE (0.31fos + 0.41bio-embodied cabon emissions)]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Spruce", Category = @"Timber", Conductivity = 0.13, Density = 450, SpecificHeat = 1600, ThermalEmittance = 0.9, SolarAbsorptance = 0.7, VisibleAbsorptance = 0.7, EmbodiedEnergy = 10, EmbodiedCarbon = 0.71, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net][LCA  ICE (0.31fos + 0.41bio-embodied cabon emissions)]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Larch", Category = @"Timber", Conductivity = 0.13, Density = 460, SpecificHeat = 1600, ThermalEmittance = 0.9, SolarAbsorptance = 0.7, VisibleAbsorptance = 0.7, EmbodiedEnergy = 10, EmbodiedCarbon = 0.71, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net][LCA  ICE (0.31fos + 0.41bio-embodied cabon emissions)]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Oriented strand board", Category = @"Timber", Conductivity = 0.13, Density = 650, SpecificHeat = 1700, ThermalEmittance = 0.9, SolarAbsorptance = 0.7, VisibleAbsorptance = 0.7, EmbodiedEnergy = 15, EmbodiedCarbon = 0.96, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net][LCA  ICE (0.45fos + 0.54 bio- embodied carbon emissions)]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Medium density fiberboard", Category = @"Timber", Conductivity = 0.09, Density = 500, SpecificHeat = 1700, ThermalEmittance = 0.9, SolarAbsorptance = 0.7, VisibleAbsorptance = 0.7, EmbodiedEnergy = 11, EmbodiedCarbon = 0.72, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net][LCA  ICE (0.39fos + 0.35bio- embodied carbon emissions)]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Chipboard", Category = @"Timber", Conductivity = 0.14, Density = 650, SpecificHeat = 1800, ThermalEmittance = 0.9, SolarAbsorptance = 0.7, VisibleAbsorptance = 0.7, EmbodiedEnergy = 10, EmbodiedCarbon = 0.71, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net][LCA  ICE (0.39fos + 0.35bio- embodied carbon emissions)]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"General Concrete", Category = @"Concrete", Conductivity = 2, Density = 2400, SpecificHeat = 950, ThermalEmittance = 0.9, SolarAbsorptance = 0.7, VisibleAbsorptance = 0.7, EmbodiedEnergy = 0.75, EmbodiedCarbon = 0.1, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net][LCA  ICE, General Concrete data]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Concrete reinforced 20-30 MPa", Category = @"Concrete", Conductivity = 2, Density = 2400, SpecificHeat = 950, ThermalEmittance = 0.9, SolarAbsorptance = 0.7, VisibleAbsorptance = 0.7, EmbodiedEnergy = 0.75, EmbodiedCarbon = 0.1, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] General concrete data used for density through visible absorptance. Data averaged for embodied enegy, embodied carbon and embodied carbon emissions." });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Concrete reinforced 30-50 MPa", Category = @"Concrete", Conductivity = 2, Density = 2400, SpecificHeat = 950, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0.74, EmbodiedCarbon = 0.099, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net]General concrete data used for density through visible absorptance. Data averaged for embodied enegy, embodied carbon and embodied carbon emissions." });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Asphalt low binder content", Category = @"Screed", Conductivity = 0.75, Density = 2350, SpecificHeat = 920, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 3.39, EmbodiedCarbon = 0.191, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] General asphalt data used for density through visible absorptance, averaged data for embodied energy through embodied carbon emissions" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Asphalt high binder content", Category = @"Screed", Conductivity = 0.75, Density = 2350, SpecificHeat = 920, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 4.46, EmbodiedCarbon = 0.216, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] General asphalt data used for density through visible absorptance, averaged data for embodied energy through embodied carbon emissions" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Lightweight concrete", Category = @"Concrete", Conductivity = 1.3, Density = 1800, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0.78, EmbodiedCarbon = 0.106, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE Concrete 20/25 Mpa]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Cement screed", Category = @"Screed", Conductivity = 1.4, Density = 2000, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 1.33, EmbodiedCarbon = 0.221, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [ LCA ICE, Mortar (1:3 cement:sand mix) ]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Basalt", Category = @"Masonry", Conductivity = 3.5, Density = 2850, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 1.26, EmbodiedCarbon = 0.073, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE General Stone]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Granite", Category = @"Masonry", Conductivity = 2.8, Density = 2600, SpecificHeat = 790, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 11, EmbodiedCarbon = 0.64, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE Granite]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Lime stone", Category = @"Masonry", Conductivity = 1.4, Density = 2000, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 1.5, EmbodiedCarbon = 0.087, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE, Limestone]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Adobe 1500kg_m3", Category = @"Masonry", Conductivity = 0.66, Density = 1500, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 3, EmbodiedCarbon = 0.23, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA ICE,  General Clay data]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Sand stone", Category = @"Masonry", Conductivity = 2.3, Density = 2600, SpecificHeat = 710, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 1, EmbodiedCarbon = 0.058, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE Sandstone]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Clinker brick 1400kg_m3", Category = @"Masonry", Conductivity = 0.58, Density = 1400, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 3, EmbodiedCarbon = 0.23, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE General (Common Brick)]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Clinker brick 1600kg_m3", Category = @"Masonry", Conductivity = 0.68, Density = 1600, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 3, EmbodiedCarbon = 0.23, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net]  [LCA, ICE General (Common Brick)]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Clinker brick 1800kg_m3", Category = @"Masonry", Conductivity = 0.81, Density = 1800, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 3, EmbodiedCarbon = 0.23, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net]  [LCA, ICE General (Common Brick)]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Clinker brick 2000kg_m3", Category = @"Masonry", Conductivity = 0.96, Density = 2000, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 3, EmbodiedCarbon = 0.23, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net]  [LCA, ICE General (Common Brick)]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Aerated concrete 350kg_m3", Category = @"Masonry", Conductivity = 0.09, Density = 350, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 3.5, EmbodiedCarbon = 0.3075, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE Autoclaved Aerated Blocks (ACC's)]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Aerated concrete 500kg_m3", Category = @"Masonry", Conductivity = 0.12, Density = 500, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 3.5, EmbodiedCarbon = 0.3075, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE Autoclaved Aerated Blocks (ACC's)]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Ytong block W PP 1.6-0.30", Category = @"Masonry", Conductivity = 0.08, Density = 300, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 3.5, EmbodiedCarbon = 0.31, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE Autoclaved Aerated Blocks (ACC's)]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Poroton Plan-T10", Category = @"Masonry", Conductivity = 0.1, Density = 650, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 3, EmbodiedCarbon = 0.23, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE General Common Brick]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Rigid foam  EPS 035", Category = @"Insulation", Conductivity = 0.035, Density = 30, SpecificHeat = 1500, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 101.5, EmbodiedCarbon = 3.48, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE Polyurethane Rigid Foam]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Rigid foam PUR no coating", Category = @"Insulation", Conductivity = 0.03, Density = 30, SpecificHeat = 1400, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 101.5, EmbodiedCarbon = 3.48, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE Polyurethane Rigid Foam]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Rigid foam PUR alu coating", Category = @"Insulation", Conductivity = 0.025, Density = 30, SpecificHeat = 1400, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 101.5, EmbodiedCarbon = 3.48, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE Polyurethane Rigid Foam]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Rigid foam PUR fleece coating", Category = @"Insulation", Conductivity = 0.028, Density = 30, SpecificHeat = 1400, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 101.5, EmbodiedCarbon = 3.48, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE Polyurethane Rigid Foam]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Wood fiber insulating board", Category = @"Insulation", Conductivity = 0.042, Density = 160, SpecificHeat = 2100, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 45, EmbodiedCarbon = 1.86, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE General Insulation]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Styrofoam", Category = @"Insulation", Conductivity = 0.04, Density = 20, SpecificHeat = 1500, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 101.5, EmbodiedCarbon = 3.48, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] No data found" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Vacuum insulation panel Variotec", Category = @"Insulation", Conductivity = 0.007, Density = 205, SpecificHeat = 900, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 45, EmbodiedCarbon = 1.86, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE, General Insulation]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Wood wool board 15mm", Category = @"Timber", Conductivity = 0.09, Density = 570, SpecificHeat = 2100, ThermalEmittance = 0.9, SolarAbsorptance = 0.7, VisibleAbsorptance = 0.7, EmbodiedEnergy = 20, EmbodiedCarbon = 0.98, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE, Woodwool (Board)]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Wood wool board 25mm", Category = @"Timber", Conductivity = 0.09, Density = 460, SpecificHeat = 2100, ThermalEmittance = 0.9, SolarAbsorptance = 0.7, VisibleAbsorptance = 0.7, EmbodiedEnergy = 20, EmbodiedCarbon = 0.98, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE, Woodwool (Board)]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Wood wool board 35mm", Category = @"Timber", Conductivity = 0.09, Density = 415, SpecificHeat = 2100, ThermalEmittance = 0.9, SolarAbsorptance = 0.7, VisibleAbsorptance = 0.7, EmbodiedEnergy = 20, EmbodiedCarbon = 0.98, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net][LCA, ICE, Woodwool (Board)]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Wood wool board 50mm", Category = @"Timber", Conductivity = 0.09, Density = 390, SpecificHeat = 2100, ThermalEmittance = 0.9, SolarAbsorptance = 0.7, VisibleAbsorptance = 0.7, EmbodiedEnergy = 20, EmbodiedCarbon = 0.98, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE, Woodwool (Board)]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Light adobe NF 700", Category = @"Masonry", Conductivity = 0.21, Density = 700, SpecificHeat = 1200, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 3, EmbodiedCarbon = 0.23, Cost = 0, Comment = @"U-Wert.net [LCA, ICE General (Simple Clay Baked Products)" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Light adobe NF 1200", Category = @"Masonry", Conductivity = 0.47, Density = 1200, SpecificHeat = 1200, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 3, EmbodiedCarbon = 0.23, Cost = 0, Comment = @"U-Wert.net [LCA, ICE General (Simple Clay Baked Products)" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Light adobe NF 1800", Category = @"Masonry", Conductivity = 0.91, Density = 1800, SpecificHeat = 1200, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 3, EmbodiedCarbon = 0.23, Cost = 0, Comment = @"U-Wert.net [LCA, ICE General (Simple Clay Baked Products)" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Shale", Category = @"Masonry", Conductivity = 2.2, Density = 2400, SpecificHeat = 760, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0.03, EmbodiedCarbon = 0.002, Cost = 0, Comment = @"U-Wert.net [LCA, ICE Shale]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Clinker brick", Category = @"Masonry", Conductivity = 0.96, Density = 2000, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 3, EmbodiedCarbon = 0.23, Cost = 0, Comment = @"U-Wert.net [LCA, ICE General Common Bricks]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Ytong block W PP 2-0.35", Category = @"Masonry", Conductivity = 0.09, Density = 350, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 3.5, EmbodiedCarbon = 0.31, Cost = 0, Comment = @"U-Wert.net [LCA, ICE Autoclaved Aerated Blocks (AAC's)]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Ytong block W PP 2-0.40", Category = @"Masonry", Conductivity = 0.1, Density = 400, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 3.5, EmbodiedCarbon = 0.31, Cost = 0, Comment = @"U-Wert.net [LCA, ICE Autoclaved Aerated Blocks (AAC's)]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Ytong block W PP 4-0.50", Category = @"Masonry", Conductivity = 0.12, Density = 500, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 3.5, EmbodiedCarbon = 0.31, Cost = 0, Comment = @"U-Wert.net [LCA, ICE Autoclaved Aerated Blocks (AAC's)]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Ytong block W PP 4-0.55", Category = @"Masonry", Conductivity = 0.14, Density = 550, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 3.5, EmbodiedCarbon = 0.31, Cost = 0, Comment = @"U-Wert.net [LCA, ICE Autoclaved Aerated Blocks (AAC's)]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Ytong block W PP 4-0.60", Category = @"Masonry", Conductivity = 0.16, Density = 600, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 3.5, EmbodiedCarbon = 0.31, Cost = 0, Comment = @"U-Wert.net [LCA, ICE Autoclaved Aerated Blocks (AAC's)]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Ytong block W PP 6-0.65", Category = @"Masonry", Conductivity = 0.18, Density = 650, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 3.5, EmbodiedCarbon = 0.31, Cost = 0, Comment = @"U-Wert.net [LCA, ICE Autoclaved Aerated Blocks (AAC's)]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Poroton T12", Category = @"Masonry", Conductivity = 0.12, Density = 650, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 3, EmbodiedCarbon = 0.23, Cost = 0, Comment = @"U-Wert.net [LCA, ICE General Common Bricks]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Cork", Category = @"Insulation", Conductivity = 0.05, Density = 160, SpecificHeat = 1800, ThermalEmittance = 0.9, SolarAbsorptance = 0.78, VisibleAbsorptance = 0.78, EmbodiedEnergy = 4, EmbodiedCarbon = 0.19, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net][ThermalEmittance SolarAbsorptance VisibleAbsorptance: DesignBuilderv3] [LCA, ICE Cork]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Ytong Multipor DAA ds", Category = @"Insulation", Conductivity = 0.047, Density = 115, SpecificHeat = 1300, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 45, EmbodiedCarbon = 1.86, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE General Insulation]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Ytong Multipor WI WTR DI", Category = @"Insulation", Conductivity = 0.042, Density = 90, SpecificHeat = 1300, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 45, EmbodiedCarbon = 1.86, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE General Insulation]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Ytong Multipor WAP", Category = @"Insulation", Conductivity = 0.045, Density = 110, SpecificHeat = 1300, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 45, EmbodiedCarbon = 1.86, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE General Insulation]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Foam glass", Category = @"Insulation", Conductivity = 0.056, Density = 130, SpecificHeat = 750, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 27, EmbodiedCarbon = 0, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE, Celular Glass]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Reed", Category = @"Insulation", Conductivity = 0.065, Density = 225, SpecificHeat = 1200, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0.24, EmbodiedCarbon = 0.01, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net][ LCA, ICE, Straw]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Vacuum insulation panel Vacupor  NT-B2-S", Category = @"Insulation", Conductivity = 0.007, Density = 190, SpecificHeat = 1050, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 45, EmbodiedCarbon = 1.86, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [LCA, ICE General Insulation]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Stainless Steel", Category = @"Metal", Conductivity = 45, Density = 7800, SpecificHeat = 480, ThermalEmittance = 0.1, SolarAbsorptance = 0.4, VisibleAbsorptance = 0.4, EmbodiedEnergy = 56.7, EmbodiedCarbon = 6.15, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [ LCA ICE, Stainless Steel]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Steel", Category = @"Metal", Conductivity = 45, Density = 7800, SpecificHeat = 480, ThermalEmittance = 0.1, SolarAbsorptance = 0.4, VisibleAbsorptance = 0.4, EmbodiedEnergy = 20.1, EmbodiedCarbon = 1.46, Cost = 0, Comment = @"[lambda  rho c: U-Wert.net] [ LCA ICE, General Steel ]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Rammed Earth", Category = @"Masonry", Conductivity = 0.75, Density = 1730, SpecificHeat = 880, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @" [lambda  rho c: U-Wert.net] [LCA, ICE Mud] [LCA, ICE, Single Clay Brick]" });


            //double check EE values
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"GypsumFibreBoard", Category = @"Boards", Conductivity = 0.32, Density = 1000, SpecificHeat = 1100, ThermalEmittance = 0.9, SolarAbsorptance = 0.7, VisibleAbsorptance = 0.7, EmbodiedEnergy = 45, EmbodiedCarbon = 1.86, Cost = 0, Comment = @"[lambda  rho c: Saint-Gobain Rigips][LCA  ICE (0.31fos + 0.41bio-embodied cabon emissions)]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Cross Laminated Timber", Category = @"Timber", Conductivity = 0.13, Density = 500, SpecificHeat = 1600, ThermalEmittance = 0.9, SolarAbsorptance = 0.7, VisibleAbsorptance = 0.7, EmbodiedEnergy = 10, EmbodiedCarbon = 0.71, Cost = 0, Comment = @"[lambda  rho c: dataholz.com][LCA  ICE (0.31fos + 0.41bio-embodied cabon emissions)]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Plaster", Category = @"Screed", Conductivity = 1.0, Density = 2000, SpecificHeat = 1130, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 1.33, EmbodiedCarbon = 0.221, Cost = 0, Comment = @"[lambda  rho c: dataholz.com] [ LCA ICE, Mortar (1:3 cement:sand mix) ]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Mineral Wool", Category = @"Insulation", Conductivity = 0.041, Density = 155, SpecificHeat = 1130, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = @"[lambda  rho c: dataholz.com]" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"XPS Board",  Category = "Insulation",  Conductivity = 0.034,  Density = 35,  SpecificHeat = 1400,  ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 87.4,  EmbodiedCarbon = 2.8,  Cost = 0.0,  Comment = "" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Sand-Lime Brick", Category = "Masonry", Conductivity = 0.56, Density = 1200, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = "" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Bonded chippings", Category = @"Screed", Conductivity = 0.7, Density = 1800, SpecificHeat = 1000, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0, Comment = "" });
            Library.OpaqueMaterials.Add(new OpaqueMaterial() { Name = @"Impact sound insulation", Category = "Insulation", Conductivity = 0.035, Density = 120, SpecificHeat = 1030, ThermalEmittance = 0.9, SolarAbsorptance = 0.6, VisibleAbsorptance = 0.6, EmbodiedEnergy = 0, EmbodiedCarbon = 0, Cost = 0.0, Comment = "" });

            #endregion



            //Basic wall
            OpaqueConstruction.QuickConstruction("120mmInsulation 200mmConcrete", ConstructionTypes.Facade, new string[] { "XPS Board", "General Concrete" }, new double[] { 0.12, 0.20 }, "Concrete", "", ref Library);
            //Basic roof
            OpaqueConstruction.QuickConstruction("300mmInsulation 200mmConcrete", ConstructionTypes.Facade, new string[] { "XPS Board", "General Concrete" }, new double[] { 0.30, 0.20 }, "Concrete", "", ref Library);
            //Basic floor
            OpaqueConstruction.QuickConstruction("200mmConcrete", ConstructionTypes.InteriorFloor, new string[] { "General Concrete" }, new double[] { 0.20 }, "Concrete", "", ref Library);
            //Basic partition
            OpaqueConstruction.QuickConstruction("115mmSandLimeBrick", ConstructionTypes.Partition, new string[] { "Plaster", "Sand-Lime Brick", "Plaster" }, new double[] { 0.005, 0.08, 0.08 }, "Concrete", "", ref Library);
            //Basic ground
            OpaqueConstruction.QuickConstruction("300mmConcrete 80mmInsulation 80mmScreed", ConstructionTypes.Partition, new string[] { "General Concrete", "XPS Board", "Cement screed" }, new double[] { 0.3, 0.115, 0.005 }, "Concrete", "", ref Library);


            //Solid wood constructions
            OpaqueConstruction.QuickConstruction("300mmInsulation 94mmSolidWood 24mmGypsum", ConstructionTypes.Facade, new string[] { "Medium density fiberboard", "Wood fiber insulating board", "Cross Laminated Timber", "Wood fiber insulating board", "GypsumFibreBoard" }, new double[] { 0.015, 0.3 , 0.094, 0.08, 0.0245 },"Timber", "dataholz.com", ref Library);
            OpaqueConstruction.QuickConstruction("120mmInsulation 78mmSolidWood 13mmGypsum", ConstructionTypes.Facade, new string[] { "Plaster", "Mineral Wool", "Cross Laminated Timber",  "GypsumFibreBoard" }, new double[] { 0.004, 0.12, 0.078, 0.013 }, "Timber", "dataholz.com", ref Library);
            OpaqueConstruction.QuickConstruction("160mmInsulation 94mmSolidWood", ConstructionTypes.Facade, new string[] { "Mineral Wool", "Mineral Wool", "Cross Laminated Timber" }, new double[] { 0.08, 0.08, 0.094 }, "Timber", "dataholz.com", ref Library);
            OpaqueConstruction.QuickConstruction("160mmInsulation 94mmSolidWood", ConstructionTypes.Facade, new string[] { "Mineral Wool", "Mineral Wool", "Cross Laminated Timber" }, new double[] { 0.08, 0.08, 0.094 }, "Timber", "dataholz.com", ref Library);

            OpaqueConstruction.QuickConstruction("12mmGypsum 78mmSolidWood 12mmGypsum", ConstructionTypes.Partition, new string[] { "GypsumFibreBoard", "Cross Laminated Timber", "GypsumFibreBoard" }, new double[] { 0.012, 0.78, 0.012 }, "Timber", "dataholz.com", ref Library);
            OpaqueConstruction.QuickConstruction("78mmSolidWood", ConstructionTypes.Partition, new string[] { "Cross Laminated Timber" }, new double[] {  0.78  }, "Timber", "dataholz.com", ref Library);

            OpaqueConstruction.QuickConstruction("150mmScreedWithImpactSoundInsulation 140mmSolidWood", ConstructionTypes.InteriorFloor, new string[] { "Cross Laminated Timber", "Bonded chippings", "Impact sound insulation" , "Cement screed" }, new double[] { 0.14, 0.06, 0.03, 0.06 }, "Timber", "dataholz.com, gdmnxn02-00", ref Library);



            //List of premade basic glazing materials/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


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
            Library.Add(gm1);
                        
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

            Library.Add( gc1);




            #region schedules

            //--------------------------------------------------------------------------------schedules


            YearSchedule.QuickSchedule("occBedroom", new double[] { 1, 1, 1, 1, 1, 1, 0.8, 0.6, 0.4, 0.4, 0.4, 0.6, 0.8, 0.6, 0.4, 0.4, 0.6, 0.8, 0.8, 0.8, 0.8, 1, 1, 1 },  "Residential" , "SIA Merkblatt 2024", ref Library);
            YearSchedule.QuickSchedule("equipBedroom", new double[] { 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.5, 1, 0.5, 0.5, 0.5, 1, 1, 0.5, 0.5, 0.5, 1, 1, 1, 1, 0.5, 0.5, 0.5, 0.1 }, "Residential" , "SIA Merkblatt 2024", ref Library);
            YearSchedule.QuickSchedule("lightsBedroom", new double[] { 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0 }, "Residential", "based on SIA Merkblatt 2024 Occ Schedule", ref Library);


            YearSchedule.QuickSchedule("occKitchen", new double[] { 0, 0, 0, 0, 0, 0, 0.4, 0.8, 0.4, 0, 0, 0.4, 0.8, 0.4, 0, 0, 0.4, 1, 0.6, 0.4, 0, 0, 0, 0 }, "Residential" , "SIA Merkblatt 2024", ref Library);
            YearSchedule.QuickSchedule("equipKitchen", new double[] { 0.2, 0.2, 0.2, 0.2, 0.2, 0.2, 0.4, 0.8, 0.4, 0.2, 0.2, 0.4, 1, 0.4, 0.2, 0.2, 0.4, 1, 0.4, 0.2, 0.2, 0.2, 0.2, 0.2 }, "Residential",  "SIA Merkblatt 2024", ref Library);
            YearSchedule.QuickSchedule("lightsKitchen", new double[] { 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0 }, "Residential", "based on SIA Merkblatt 2024 Occ Schedule", ref Library);


            YearSchedule.QuickSchedule("occOffice", new double[] { 0, 0, 0, 0, 0, 0, 0, 0.2, 0.4, 0.6, 0.8, 0.8, 0.4, 0.6, 0.8, 0.8, 0.4, 0.2, 0, 0, 0, 0, 0, 0 }, new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, "Office","SIA Merkblatt 2024", ref Library);
            YearSchedule.QuickSchedule("equipOffice", new double[] { 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.2, 0.4, 0.6, 0.8, 0.8, 0.4, 0.6, 0.8, 0.8, 0.4, 0.2, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1 }, new double[] { 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1 } , "Office", "SIA Merkblatt 2024", ref Library);
            YearSchedule.QuickSchedule("lightsOffice", new double[] { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0 }, new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, "Office", "based on SIA Merkblatt 2024 Occ Schedule", ref Library);
 

            YearSchedule.QuickSchedule("occMeetingRoom", new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.6, 1, 0.4, 0, 0, 0.6, 1, 0.4, 0, 0, 0, 0, 0, 0, 0 }, new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, "Office",  "SIA Merkblatt 2024", ref Library);
            YearSchedule.QuickSchedule("equipMeetingRoom", new double[] { 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.2, 0.4, 0.6, 0.8, 0.8, 0.4, 0.6, 0.8, 0.8, 0.4, 0.2, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1 }, new double[] { 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1 }, "Office", "SIA Merkblatt 2024", ref Library);
            YearSchedule.QuickSchedule("lightsMeetingRoom", new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 }, new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, "Office", "based on SIA Merkblatt 2024 Occ Schedule", ref Library);


            YearSchedule.QuickSchedule("occLibrary", new double[] { 0, 0, 0, 0, 0, 0, 0, 0.2, 0.6, 1, 1, 0.2, 0.2, 1, 1, 0.6, 0.2, 0, 0, 0, 0, 0, 0, 0 }, "Education", "SIA Merkblatt 2024", ref Library);
            YearSchedule.QuickSchedule("equipLibrary", new double[] { 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.2, 0.6, 1, 1, 0.2, 0.2, 1, 1, 0.6, 0.4, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1 }, "Education", "SIA Merkblatt 2024", ref Library);
            YearSchedule.QuickSchedule("lightsLibrary", new double[] { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 }, "Education", "based on SIA Merkblatt 2024 Occ Schedule", ref Library);
 

            YearSchedule.QuickSchedule("occLectureHall", new double[] { 0, 0, 0, 0, 0, 0, 0, 0.2, 0.6, 1, 1, 0.2, 0.2, 1, 1, 0.6, 0.2, 0, 0, 0, 0, 0, 0, 0 }, new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, "Education", "SIA Merkblatt 2024", ref Library);
            YearSchedule.QuickSchedule("equipLectureHall", new double[] { 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.2, 0.6, 1, 1, 0.2, 0.2, 1, 1, 0.6, 0.4, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1 }, new double[] { 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1 }, "Education", "SIA Merkblatt 2024", ref Library);
            YearSchedule.QuickSchedule("lightsLectureHall", new double[] { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 }, new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, "Education", "based on SIA Merkblatt 2024 Occ Schedule", ref Library);


            YearSchedule.QuickSchedule("occSuperMarket", new double[] { 0, 0, 0, 0, 0, 0, 0, 0.2, 0.4, 0.4, 0.4, 0.6, 0.6, 0.6, 0.4, 0.4, 0.6, 0.8, 0.6, 0, 0, 0, 0, 0 }, "Commercial", "SIA Merkblatt 2024", ref Library);
            YearSchedule.QuickSchedule("equipSuperMarket", new double[] { 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0.1, 0.1, 0.1, 0.1, 0.1 }, "Commercial", "SIA Merkblatt 2024", ref Library);
            YearSchedule.QuickSchedule("lightsSuperMarket", new double[] { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0 }, "Commercial", "based on SIA Merkblatt 2024 Occ Schedule", ref Library);


            YearSchedule.QuickSchedule("occShopping", new double[] { 0, 0, 0, 0, 0, 0, 0, 0.2, 0.4, 0.4, 0.4, 0.6, 0.6, 0.6, 0.4, 0.4, 0.6, 0.8, 0.6, 0, 0, 0, 0, 0 }, "Commercial", "SIA Merkblatt 2024", ref Library);
            YearSchedule.QuickSchedule("equipShopping", new double[] { 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0.1, 0.1, 0.1, 0.1, 0.1 }, "Commercial", "SIA Merkblatt 2024", ref Library);
            YearSchedule.QuickSchedule("lightsShopping", new double[] { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0 }, "Commercial", "based on SIA Merkblatt 2024 Occ Schedule", ref Library);




            Random r = new Random();
            var Values = new double[8760];
            for (int i = 0; i < 8760; i++) { Values[i] = (double)r.Next(1, 100) / 100.0; }

            Library.Add(new ScheduleArray() { Name = "RandomBehavior", Category = "Random", Values = Values });
            //Library.ArraySchedules.Add(new ScheduleArray() { Name = "RandomBehavior2", Category = "Random", Values = Values });


            #endregion


            Library.Add(new ZoneLoad() { Name = "BedroomLoads", PeopleDensity = 1.0 / 40.0, IlluminanceTarget = 200, LightingPowerDensity = 9.5, EquipmentPowerDensity = 2, OccupancySchedule = "occBedroom", EquipmentAvailibilitySchedule = "equipBedroom", LightsAvailibilitySchedule = "lightsBedroom", Category = "Residential" ,DataSource = "SIA Merkblatt 2024" });
            Library.Add(new ZoneLoad() { Name = "KitchenLoads", PeopleDensity = 1.0 / 5.0, IlluminanceTarget = 500, LightingPowerDensity = 17, EquipmentPowerDensity = 40, OccupancySchedule = "occKitchen", EquipmentAvailibilitySchedule = "equipKitchen", LightsAvailibilitySchedule = "lightsKitchen", Category = "Residential" ,DataSource = "SIA Merkblatt 2024" });
            Library.Add(new ZoneLoad() { Name = "SingleOfficeLoads", PeopleDensity = 1.0 / 14.0, IlluminanceTarget = 500, LightingPowerDensity = 16, EquipmentPowerDensity = 7, OccupancySchedule = "occOffice", EquipmentAvailibilitySchedule = "equipOffice", LightsAvailibilitySchedule = "lightsOffice", Category = "Office", DataSource = "SIA Merkblatt 2024" });
            Library.Add(new ZoneLoad() { Name = "MeetingRoomLoads", PeopleDensity = 1.0 / 3.0, IlluminanceTarget = 500, LightingPowerDensity = 16, EquipmentPowerDensity = 2, OccupancySchedule = "occMeetingRoom", EquipmentAvailibilitySchedule = "equipMeetingRoom", LightsAvailibilitySchedule = "lightsMeetingRoom", Category = "Office", DataSource = "SIA Merkblatt 2024" });

            Library.Add(new ZoneConditioning() { Name = "BedroomHeatingCoolingMechVent", HeatingSetpoint = 19, CoolingSetpoint = 26,  MechVentIsOn = true, MinFreshAirPerson = 2.5, MinFreshAirArea = 0.3, Category = "Residential" });
            Library.Add(new ZoneConditioning() { Name = "KitchenHeatingCoolingMechVent", HeatingSetpoint = 20, CoolingSetpoint = 26, MechVentIsOn = true, MinFreshAirPerson = 3.9, MinFreshAirArea = 0.9, Category = "Residential" });
            Library.Add(new ZoneConditioning() { Name = "OfficeHeatingCoolingMechVent", HeatingSetpoint = 20, CoolingSetpoint = 26, MechVentIsOn = true, MinFreshAirPerson = 2.5, MinFreshAirArea = 0.3, Category = "Office" });
            Library.Add(new ZoneConditioning() { Name = "MeetingRoomHeatingCoolingMechVent", HeatingSetpoint = 20, CoolingSetpoint = 26, MechVentIsOn = true, MinFreshAirPerson = 3.8, MinFreshAirArea = 0.3, Category = "Office" });
            Library.Add(new ZoneConditioning() { Name = "LectureHallHeatingCoolingMechVent", HeatingSetpoint = 20, CoolingSetpoint = 26, MechVentIsOn = true, MinFreshAirPerson = 3.8, MinFreshAirArea = 0.3, Category = "Education" });

            Library.Add(new ZoneVentilation() { Name = "PoorAirTightness", InfiltrationIsOn = true,  InfiltrationAch = 1.2, InfiltrationModel = InfiltrationModel.Wind, Category = "Infiltration" });
            Library.Add(new ZoneVentilation() { Name = "ModerateAirTightness", InfiltrationIsOn = true, InfiltrationAch = 0.5, InfiltrationModel = InfiltrationModel.Wind, Category = "Infiltration" });
            Library.Add(new ZoneVentilation() { Name = "GoodAirTightness", InfiltrationIsOn = true, InfiltrationAch = 0.15, InfiltrationModel = InfiltrationModel.Wind, Category = "Infiltration" });


            Library.Add(new DomHotWater() { Name = "House" , FlowRatePerFloorArea =0.03 , WaterSchedule = "AllOn" , WaterTemperatureInlet = 10, WaterSupplyTemperature = 60 });

            Library.Add(new ZoneConstruction() { Name = "SolidWood", RoofConstruction = "300mmInsulation 94mmSolidWood 24mmGypsum", FacadeConstruction = "300mmInsulation 94mmSolidWood 24mmGypsum", SlabConstruction = "150mmScreedWithImpactSoundInsulation 140mmSolidWood", GroundConstruction = "120mmInsulation 200mmConcrete", PartitionConstruction = "12mmGypsum 78mmSolidWood 12mmGypsum" });


            #region SimpleGlass

            Library.Add(new GlazingConstructionSimple("SinglePaneClr", "Single pane", "Standard clear", 0.913, 5.894, 0.905));
            Library.Add(new GlazingConstructionSimple("DoublePaneClr", "Double pane", "Standard clear", 0.812, 2.720, 0.764));
            Library.Add(new GlazingConstructionSimple("DoublePaneLoEe2", "Double pane", "Low emissivity coating on layer e2", 0.444, 1.493, 0.373));
            Library.Add(new GlazingConstructionSimple("DoublePaneLoEe3", "Double pane", "Low emissivity coating on layer e3", 0.769, 1.507, 0.649));
            Library.Add(new GlazingConstructionSimple("TriplePaneLoE", "Triple pane", "Low emissivity coating on layer e2 and e5", 0.661, 0.785, 0.764));


            #endregion


            #region ZoneDefTemplates


            


            Library.Add(new ZoneDefinition());



            #endregion


            ////--------------------------------------------------------------------------------serialization test


            //string xml = SerializeDeserialize.Serialize(Library);

            //SerializeDeserialize.Deserialize(xml, typeof(ArchsimLib.Lib));

            ////--------------------------------------------------------------------------------out

            return Library;
        }





        //private static void QuickScheduleAdd(string Name ,double[] dayArray, string category, string dataSource ,ref Library Library) {


        //    int[] MonthFrom = { 1 };
        //    int[] DayFrom = { 1 };
        //    int[] MonthTo = { 12 };
        //    int[] DayTo = { 31 };

        //    DaySchedule someDaySchedule = new DaySchedule(Name, "Fraction", dayArray.ToList());
        //    someDaySchedule.DataSource = dataSource;
        //    someDaySchedule.Category = category;
        //    Library.DaySchedules.Add(someDaySchedule);
        //    DaySchedule[] daySchedulesArray = { someDaySchedule, someDaySchedule, someDaySchedule, someDaySchedule, someDaySchedule, someDaySchedule, someDaySchedule };
        //    WeekSchedule someWeekSchedule = new WeekSchedule(Name, daySchedulesArray, "Fraction");
        //    someWeekSchedule.DataSource = dataSource;
        //    someWeekSchedule.Category = category;
        //    Library.WeekSchedules.Add(someWeekSchedule);
        //    WeekSchedule[] weekSchedulesArray = { someWeekSchedule };
        //    YearSchedule someYearSchedule = new YearSchedule(Name, "Fraction", weekSchedulesArray.ToList(), MonthFrom.ToList(), DayFrom.ToList(), MonthTo.ToList(), DayTo.ToList());
        //    someYearSchedule.DataSource = dataSource;
        //    someYearSchedule.Category = category;
        //    Library.YearSchedules.Add(someYearSchedule);

        //}

        //private static void QuickScheduleAdd(string Name, double[] dayArray, double[] weArray, string category, string dataSource, ref Library Library)
        //{


        //    int[] MonthFrom = { 1 };
        //    int[] DayFrom = { 1 };
        //    int[] MonthTo = { 12 };
        //    int[] DayTo = { 31 };

        //    DaySchedule someDaySchedule = new DaySchedule(Name, "Fraction", dayArray.ToList());
        //    someDaySchedule.DataSource = dataSource;
        //    someDaySchedule.Category = category;
        //    Library.DaySchedules.Add(someDaySchedule);
        //    DaySchedule weSchedule = new DaySchedule(Name+"WeekEnd", "Fraction", dayArray.ToList());
        //    weSchedule.DataSource = dataSource;
        //    weSchedule.Category = category;
        //    Library.DaySchedules.Add(someDaySchedule);
        //    DaySchedule[] daySchedulesArray = { someDaySchedule, someDaySchedule, someDaySchedule, someDaySchedule, someDaySchedule, weSchedule, weSchedule };
        //    WeekSchedule someWeekSchedule = new WeekSchedule(Name, daySchedulesArray, "Fraction");
        //    someWeekSchedule.DataSource = dataSource;
        //    someWeekSchedule.Category = category;
        //    Library.WeekSchedules.Add(someWeekSchedule);
        //    WeekSchedule[] weekSchedulesArray = { someWeekSchedule };
        //    YearSchedule someYearSchedule = new YearSchedule(Name, "Fraction", weekSchedulesArray.ToList(), MonthFrom.ToList(), DayFrom.ToList(), MonthTo.ToList(), DayTo.ToList());
        //    someYearSchedule.DataSource = dataSource;
        //    someYearSchedule.Category = category;
        //    Library.YearSchedules.Add(someYearSchedule);

        //}

    }
}
