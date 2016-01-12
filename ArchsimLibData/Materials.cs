using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;


namespace ArchsimLib
{
    //---------------------------
    [DataContract(IsReference = true)]
    public class OpaqueMaterialAirGap
    {
        /// <summary>
        /// Resistance {m2-K/w}
        /// </summary>
        [DataMember]
        public double Resistance { get; set; } = 0.2079491;
        [DataMember]
        public string Name { get; set; } = "airGap";

        public OpaqueMaterialAirGap() { }
    }

    [DataContract(IsReference = true)]
    public class OpaqueMaterialNoMass
    {
        /// <summary>
        /// Resistance {m2-K/w}
        /// </summary>

        [DataMember]
        public double Resistance { get; set; } = 0.2079491;
        [DataMember]
        public string Name { get; set; } = "NOMASS";
        [DataMember]
        public string Roughness { get; set; } = "Rough";
        [DataMember]
        public double ThermalAbsorptance { get; set; } = 0.9;
        [DataMember]
        public double SolarAbsorptance { get; set; } = 0.7;
        [DataMember]
        public double VisibleAbsorptance { get; set; } = 0.7;

        public OpaqueMaterialNoMass() { }
    }

    //---------------------------


    [DataContract(IsReference = true)]
    public class OpaqueMaterial : BaseMaterial
    {
        /// <summary>
        /// Conductivity {w/m-K}
        /// </summary>
        [DataMember]
        [Units("W/m.K")]
        public double Conductivity { get; set; } = 2.4;
        /// <summary>
        /// Density {kg/m3}
        /// </summary>
        [DataMember]
        [Units("kg/m3")]
        public double Density { get; set; } = 2400;
        [DataMember]
        public string Roughness { get; set; } = "Rough";

        /// <summary>
        /// Specific Heat {J/kg-K}
        /// </summary>
        [DataMember]
        [Units("J/kg.K")]
        public double SpecificHeat { get; set; } = 840;
        [DataMember]
        public double ThermalEmittance { get; set; } = 0.9;
        [DataMember]
        public double SolarAbsorptance { get; set; } = 0.7;
        [DataMember]
        public double VisibleAbsorptance { get; set; } = 0.7;
        [DataMember]
        public bool PhaseChange { get; set; } = false;
        [DataMember]
        public string PhaseChangeProperties { get; set; } = "";
        [DataMember]
        public bool VariableConductivity { get; set; } = false;
        [DataMember]
        public string VariableConductivityProperties { get; set; } = "";



        public OpaqueMaterial() { }

    //    public OpaqueMaterial(
    //        //string _Roughness,
    //string _Name,
    //string _Type,
    //double _Conductivity,
    //double _Density,
    //double _SpecificHeat,
    //double _ThermalEmittance,
    //double _SolarAbsorptance,
    //double _VisibleAbsorptance)
    //    {
    //        Name = _Name;
    //        Type = _Type;
    //        Conductivity = _Conductivity;
    //        Density = _Density;
    //        SpecificHeat = _SpecificHeat;
    //        ThermalEmittance = _ThermalEmittance;
    //        SolarAbsorptance = _SolarAbsorptance;
    //        VisibleAbsorptance = _VisibleAbsorptance;
    //    }

        //public OpaqueMaterial(
        //    //string _Roughness,
        //  string _Name,
        //  string _Type,
        //  double _Conductivity,
        //  double _Density,
        //  double _SpecificHeat,
        //  double _ThermalEmittance,
        //  double _SolarAbsorptance,
        //  double _VisibleAbsorptance,
        //  double _EmbodiedEnergy,
        //  double _EmbodiedCarbon,
        //  double _Cost,
        //  string _Commments)
        //{
        //    // Roughness = _Roughness;
        //    Name = _Name;
        //    Type = _Type;
        //    Conductivity = _Conductivity;
        //    Density = _Density;
        //    SpecificHeat = _SpecificHeat;
        //    ThermalEmittance = _ThermalEmittance;
        //    SolarAbsorptance = _SolarAbsorptance;
        //    VisibleAbsorptance = _VisibleAbsorptance;
        //    EmbodiedEnergy = _EmbodiedEnergy;
        //    EmbodiedCarbon = _EmbodiedCarbon;
        //    Cost = _Cost;
        //    Comment = _Commments;
        //}

        //public OpaqueMaterial(
        //    //string _Roughness,
        //    string _Name,
        //    string _Type,
        //    double _Conductivity,
        //    double _Density,
        //    double _SpecificHeat,
        //    double _ThermalEmittance,
        //    double _SolarAbsorptance,
        //    double _VisibleAbsorptance,
        //    double _EmbodiedEnergy,
        //    double _EmbodiedCarbon, 
        //    double _Cost,
        //    int _Life,
        //    string _Commments)
        //{
        //    // Roughness = _Roughness;
        //    Name = _Name;
        //    Type = _Type;
        //    Conductivity = _Conductivity;
        //    Density = _Density;
        //    SpecificHeat = _SpecificHeat;
        //    ThermalEmittance = _ThermalEmittance;
        //    SolarAbsorptance = _SolarAbsorptance;
        //    VisibleAbsorptance = _VisibleAbsorptance;
        //    EmbodiedEnergy = _EmbodiedEnergy;
        //    EmbodiedCarbon = _EmbodiedCarbon;
        //    Cost = _Cost;
        //    Life = _Life;
        //    Comment = _Commments;
        //}

        //public void Update(
        //    //string _Roughness,
        //  string _Name,
        //  string _Type,
        //  double _Conductivity,
        //  double _Density,
        //  double _SpecificHeat,
        //  double _ThermalEmittance,
        //  double _SolarAbsorptance,
        //  double _VisibleAbsorptance,
        //  double _EmbodiedEnergy,
        //  double _EmbodiedCarbon,
        //  double _Cost,
        //  int _Life,
        //  string _Commments)
        //{
        //    // Roughness = _Roughness;
        //    Name = _Name;
        //    GasType = _Type;
        //    Conductivity = _Conductivity;
        //    Density = _Density;
        //    SpecificHeat = _SpecificHeat;
        //    ThermalEmittance = _ThermalEmittance;
        //    SolarAbsorptance = _SolarAbsorptance;
        //    VisibleAbsorptance = _VisibleAbsorptance;
        //    EmbodiedEnergy = _EmbodiedEnergy;
        //    EmbodiedCarbon = _EmbodiedCarbon;
        //    Cost = _Cost;
        //    Life = _Life;
        //    Comment = _Commments;
        //}
        public bool Correct()
        {
            bool changed = false;

            string cleanName = HelperFunctions.RemoveSpecialCharacters(this.Name);
            if (this.Name != cleanName) { this.Name = cleanName; changed = true; }

            if (this.VisibleAbsorptance < 0.001) { this.VisibleAbsorptance = 0.001; changed = true; }
            if (this.VisibleAbsorptance > 0.999) { this.VisibleAbsorptance = 0.999; changed = true; }
            if (this.SolarAbsorptance < 0.001) { this.SolarAbsorptance = 0.001; changed = true; }
            if (this.SolarAbsorptance > 0.999) { this.SolarAbsorptance = 0.999; changed = true; }
            if (this.ThermalEmittance < 0.001) { this.ThermalEmittance = 0.001; changed = true; }
            if (this.ThermalEmittance > 0.999) { this.ThermalEmittance = 0.999; changed = true; }
            if (this.SpecificHeat < 100) { this.SpecificHeat = 100; changed = true; }
            if (this.SpecificHeat > 5000) { this.SpecificHeat = 2000; changed = true; }
            if (this.Density < 10) { this.Density = 10; changed = true; }
            if (this.Density > 5000) { this.Density = 5000; changed = true; }
            if (this.Conductivity < 0.001) { this.Conductivity = 0.001; changed = true; }
            if (this.Conductivity > 5000) { this.Conductivity = 5000; changed = true; }

            return changed;
        }
    }


    [DataContract(IsReference = true)]
    public class GlazingMaterial : WindowMaterialBase
    {
        /// <summary>
        /// Conductivity {W/m.K}
        /// </summary>
        [DataMember]
        [Units("W/m.K")]
        public double Conductivity { get; set; } = 0;
        /// <summary>
        /// Density {kg/m3}
        /// </summary>
        [DataMember]
        [Units("kg/m3")]
        public double Density { get; set; } = 2500;
        /// <summary>
        /// Optical data type {SpectralAverage or Spectral}
        /// </summary>
        [DataMember]
        public string Optical { get; set; } = "SpectralAverage";
        /// <summary>
        /// Name of spectral data set when Optical Data GasType = Spectral
        /// </summary>
        [DataMember]
        public string OpticalData { get; set; } = "";
        /// <summary>
        /// Solar transmittance at normal incidence
        /// </summary>
        [DataMember]
        public double SolarTransmittance { get; set; } = 0.837;
        /// <summary>
        /// Solar reflectance at normal incidence: front side
        /// </summary>
        [DataMember]
        public double SolarReflectanceFront { get; set; } = 0.075;
        /// <summary>
        /// Solar reflectance at normal incidence: back side
        /// </summary>
        [DataMember]
        public double SolarReflectanceBack { get; set; } = 0.075;
        /// <summary>
        /// Visible transmittance at normal incidence
        /// </summary>
        [DataMember]
        public double VisibleTransmittance { get; set; } = 0.898;
        /// <summary>
        /// Visible reflectance at normal incidence: front side
        /// </summary>
        [DataMember]
        public double VisibleReflectanceFront { get; set; } = 0.081;
        /// <summary>
        /// Visible reflectance at normal incidence: back side
        /// </summary>
        [DataMember]
        public double VisibleReflectanceBack { get; set; } = 0.081;
        /// <summary>
        /// IR transmittance at normal incidence
        /// </summary>
        [DataMember]
        public double IRTransmittance { get; set; } = 0.0;
        /// <summary>
        /// IR emissivity: front side
        /// </summary>
        [DataMember]
        public double IREmissivityFront { get; set; } = 0.84;
        /// <summary>
        /// IR emissivity: back side
        /// </summary>
        [DataMember]
        public double IREmissivityBack { get; set; } = 0.84;
        /// <summary>
        /// Dirt Correction Factor for Solar and Visible Transmittance
        /// </summary>
        [DataMember]
        public double DirtFactor { get; set; } = 1;

        // public string SolarDiffusion = "No";

        // public int youngsModulus = 72000000000;

        // public double PoissonsRatio = 0.22;


        public GlazingMaterial() { }

        public GlazingMaterial(
    string _Name,
    string _Type,
    double _Conductivity,
    double _Density,
    double _SolarTransmittance,
    double _SolarReflectanceFront,
    double _SolarReflectanceBack,
    double _VisibleTransmittance,
    double _VisibleReflectanceFront,
    double _VisibleReflectanceBack,
    double _IRTransmittance,
    double _IREmissivityFront,
    double _IREmissivityBack
    )
        {
            Name = _Name;
            Type = _Type;
            Conductivity = _Conductivity;
            Density = _Density;
            SolarTransmittance = _SolarTransmittance;
            SolarReflectanceFront = _SolarReflectanceFront;
            SolarReflectanceBack = _SolarReflectanceBack;
            VisibleTransmittance = _VisibleTransmittance;
            VisibleReflectanceFront = _VisibleReflectanceFront;
            VisibleReflectanceBack = _VisibleReflectanceBack;
            IRTransmittance = _IRTransmittance;
            IREmissivityFront = _IREmissivityFront;
            IREmissivityBack = _IREmissivityBack;

        }

        public GlazingMaterial(
            string _Name,
            string _Type,
            double _Conductivity,
            double _Density,
            double _EmbodiedEnergy,
            double _EmbodiedCarbon,
            //double _EmbodiedEnergyUncert,
            //double _SubstituionTimeStep,
            //double[] _SubstituionRatePattern,
            double _Cost,
            string _Comments,
            double _SolarTransmittance,
            double _SolarReflectanceFront,
            double _SolarReflectanceBack,
            double _VisibleTransmittance,
            double _VisibleReflectanceFront,
            double _VisibleReflectanceBack,
            double _IRTransmittance,
            double _IREmissivityFront,
            double _IREmissivityBack
            )
        {
            Name = _Name;
            Type = _Type;
            Conductivity = _Conductivity;
            Density = _Density;
            EmbodiedEnergy = _EmbodiedEnergy;
            EmbodiedCarbon = _EmbodiedCarbon;
            //EmbodiedEnergyUncert = _EmbodiedEnergyUncert;
            //SubstituionTimeStep = _SubstituionTimeStep;
            //SubstituionRatePattern = _SubstituionRatePattern;
            Cost = _Cost;
            // DataSource = _DataSource;
            Comment = _Comments;

            SolarTransmittance = _SolarTransmittance;
            SolarReflectanceFront = _SolarReflectanceFront;
            SolarReflectanceBack = _SolarReflectanceBack;
            VisibleTransmittance = _VisibleTransmittance;
            VisibleReflectanceFront = _VisibleReflectanceFront;
            VisibleReflectanceBack = _VisibleReflectanceBack;
            IRTransmittance = _IRTransmittance;
            IREmissivityFront = _IREmissivityFront;
            IREmissivityBack = _IREmissivityBack;

        }
        public GlazingMaterial(
            string _Name,
            string _Type,
            double _Conductivity,
            double _Density,
            double _EmbodiedEnergy,
            double _EmbodiedCarbon,
            int _Life,
            //double _SubstituionTimeStep,
            //double[] _SubstituionRatePattern,
            double _Cost,
            string _Comments,
            double _SolarTransmittance,
            double _SolarReflectanceFront,
            double _SolarReflectanceBack,
            double _VisibleTransmittance,
            double _VisibleReflectanceFront,
            double _VisibleReflectanceBack,
            double _IRTransmittance,
            double _IREmissivityFront,
            double _IREmissivityBack
            )
        {
            Name = _Name;
            Type = _Type;
            Conductivity = _Conductivity;
            Density = _Density;
            EmbodiedEnergy = _EmbodiedEnergy;
            EmbodiedCarbon = _EmbodiedCarbon;
            //EmbodiedEnergyUncert = _EmbodiedEnergyUncert;
            //SubstituionTimeStep = _SubstituionTimeStep;
            //SubstituionRatePattern = _SubstituionRatePattern;
            Cost = _Cost;
            Life = _Life;
            // DataSource = _DataSource;
            Comment = _Comments;

            SolarTransmittance = _SolarTransmittance;
            SolarReflectanceFront = _SolarReflectanceFront;
            SolarReflectanceBack = _SolarReflectanceBack;
            VisibleTransmittance = _VisibleTransmittance;
            VisibleReflectanceFront = _VisibleReflectanceFront;
            VisibleReflectanceBack = _VisibleReflectanceBack;
            IRTransmittance = _IRTransmittance;
            IREmissivityFront = _IREmissivityFront;
            IREmissivityBack = _IREmissivityBack;

        }
        //public void Update(
        //    string _Name,
        //    string _Type,
        //    double _Conductivity,
        //    double _Density,
        //    double _EmbodiedEnergy,
        //    double _EmbodiedCarbon,
        //    int _Life,
        //    //double _SubstituionTimeStep,
        //    //double[] _SubstituionRatePattern,
        //    double _Cost,
        //    string _Comments,
        //    double _SolarTransmittance,
        //    double _SolarReflectanceFront,
        //    double _SolarReflectanceBack,
        //    double _VisibleTransmittance,
        //    double _VisibleReflectanceFront,
        //    double _VisibleReflectanceBack,
        //    double _IRTransmittance,
        //    double _IREmissivityFront,
        //    double _IREmissivityBack
        //    )
        //{
        //    Name = _Name;
        //    GasType = _Type;
        //    Conductivity = _Conductivity;
        //    Density = _Density;
        //    EmbodiedEnergy = _EmbodiedEnergy;
        //    EmbodiedCarbon = _EmbodiedCarbon;
        //    //EmbodiedEnergyUncert = _EmbodiedEnergyUncert;
        //    //SubstituionTimeStep = _SubstituionTimeStep;
        //    //SubstituionRatePattern = _SubstituionRatePattern;
        //    Cost = _Cost;
        //    Life = _Life;
        //    // DataSource = _DataSource;
        //    Comment = _Comments;

        //    SolarTransmittance = _SolarTransmittance;
        //    SolarReflectanceFront = _SolarReflectanceFront;
        //    SolarReflectanceBack = _SolarReflectanceBack;
        //    VisibleTransmittance = _VisibleTransmittance;
        //    VisibleReflectanceFront = _VisibleReflectanceFront;
        //    VisibleReflectanceBack = _VisibleReflectanceBack;
        //    IRTransmittance = _IRTransmittance;
        //    IREmissivityFront = _IREmissivityFront;
        //    IREmissivityBack = _IREmissivityBack;

        //}
        public bool Correct()
        {
            bool changed = false;

            string cleanName = HelperFunctions.RemoveSpecialCharacters(this.Name);
            if (this.Name != cleanName) { this.Name = cleanName; changed = true; }


            if (this.SolarTransmittance < 0.0) { this.SolarTransmittance = 0.0; changed = true; }
            if (this.SolarTransmittance > 1.0) { this.SolarTransmittance = 1.0; changed = true; }

            if (this.SolarReflectanceFront < 0.0) { this.SolarReflectanceFront = 0.0; changed = true; }
            if (this.SolarReflectanceFront > 1.0) { this.SolarReflectanceFront = 1.0; changed = true; }

            if (this.SolarReflectanceBack < 0.0) { this.SolarReflectanceBack = 0.0; changed = true; }
            if (this.SolarReflectanceBack > 1.0) { this.SolarReflectanceBack = 1.0; changed = true; }

            if (this.VisibleTransmittance < 0.0) { this.VisibleTransmittance = 0.0; changed = true; }
            if (this.VisibleTransmittance > 1.0) { this.VisibleTransmittance = 1.0; changed = true; }

            if (this.VisibleReflectanceFront < 0.0) { this.VisibleReflectanceFront = 0.0; changed = true; }
            if (this.VisibleReflectanceFront > 1.0) { this.VisibleReflectanceFront = 1.0; changed = true; }

            if (this.VisibleReflectanceBack < 0.0) { this.VisibleReflectanceBack = 0.0; changed = true; }
            if (this.VisibleReflectanceBack > 1.0) { this.VisibleReflectanceBack = 1.0; changed = true; }

            if (this.IRTransmittance < 0.0) { this.IRTransmittance = 0.0; changed = true; }
            if (this.IRTransmittance > 1.0) { this.IRTransmittance = 1.0; changed = true; }

            if (this.IREmissivityFront < 0.0) { this.IREmissivityFront = 0.0; changed = true; }
            if (this.IREmissivityFront > 1.0) { this.IREmissivityFront = 1.0; changed = true; }

            if (this.IREmissivityBack < 0.0) { this.IREmissivityBack = 0.0; changed = true; }
            if (this.IREmissivityBack > 1.0) { this.IREmissivityBack = 1.0; changed = true; }

            if (this.Density < 10) { this.Density = 10; changed = true; }
            if (this.Density > 5000) { this.Density = 5000; changed = true; }

            if (this.Conductivity < 0.001) { this.Conductivity = 0.001; changed = true; }
            if (this.Conductivity > 5000) { this.Conductivity = 5000; changed = true; }

            return changed;
        }
    }


    public enum GasTypes
    {
        AIR,
        ARGON,
        KRYPTON,
        XENON,
        SF6
    };

    [DataContract(IsReference = true)]
    public class GasMaterial : WindowMaterialBase
    {
      
        string[] gases;

        [DataMember]
        public GasTypes GasType = GasTypes.AIR;

        public GasMaterial()
        {
            Name = GasType.ToString();
            //string[] _gases = { "AIR", "ARGON", "KRYPTON", "XENON", "SF6" };//, "Custom"
            //gases = _gases;
        }
        public GasMaterial(GasTypes _Type)
        {

           //string[] _gases = { "AIR", "ARGON", "KRYPTON", "XENON", "SF6" };//, "Custom"
           // gases = _gases;
            GasType = _Type;
            Name = Name = GasType.ToString(); ;

            //if (gases.Contains(_Type.ToUpper()))
            //{
            //    GasType = _Type.ToUpper();
            //}
            //else { GasType = gases[0]; }

        }
    }


    [DataContract(IsReference = true)]
    [KnownType(typeof(GasMaterial))]
    [KnownType(typeof(GlazingMaterial))]
    public abstract class WindowMaterialBase : BaseMaterial
    {
        // TODO: Add stuff for window calculations
    }


    public abstract class BaseMaterial  : LibraryComponent
    {
        [DataMember]
        public string Type { get; set; } = "Default";

        [DataMember][Units("MJ/Kg")]
        public double EmbodiedEnergy { get; set; } = 0;

        [DataMember]
        public double EmbodiedEnergyStdDev { get; set; }
        /// <summary>
        /// Bla bla
        /// </summary>
        [DataMember][Units("CO2e/Kg")]
        public double EmbodiedCarbon { get; set; } = 0;

        [DataMember]
        public double EmbodiedCarbonStdDev { get; set; }

        [DataMember]
         public double Cost { get; set; } = 0;

        [DataMember]
        public int Life { get; set; } = 1;

        //duplicate of life
        [DataMember]
        public double[] SubstitutionRatePattern { get; set; }
        //duplicate of life
        [DataMember]
        public double SubstitutionTimestep { get; set; }


        [DataMember]
        public double TransportCarbon { get; set; }

        [DataMember]
        public double TransportDistance { get; set; }

        [DataMember]
        public double TransportEnergy { get; set; }

        public BaseMaterial() { }


    }



}
