using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ArchsimLib
{
    public enum ConstructionTypes  {
            Facade,
            Roof,
            GroundFloor,
            InteriorFloor,
            ExteriorFloor,
            Partition
        };
    public enum GlazingConstructionTypes
    {
        Other,
        Single,
        Double,
        Triple,
        Quadruple
    };


    [DataContract]
    public class OpaqueConstruction : BaseConstruction
    {
        [DataMember]
        public List<Layer<OpaqueMaterial>> Layers = new List<Layer<OpaqueMaterial>>();

        [DataMember]
        public ConstructionTypes Type = ConstructionTypes.Facade;

        public OpaqueConstruction() { }


        public bool Correct()
        {
            bool changed = false;

            string cleanName = HelperFunctions.RemoveSpecialCharacters(this.Name);
            if (this.Name != cleanName) { this.Name = cleanName; changed = true; }

            foreach (Layer<OpaqueMaterial> l in Layers) { if (l.Correct()) { changed = true; } }

            return changed;
        }

       
    }
    [DataContract]
    public class GlazingConstruction : BaseConstruction
    {
        [DataMember]
        public List<Layer<WindowMaterialBase>> Layers = new List<Layer<WindowMaterialBase>>();
        public GlazingConstruction() { }
        public bool Correct()
        {
            bool changed = false;

            string cleanName = HelperFunctions.RemoveSpecialCharacters(this.Name);
            if (this.Name != cleanName) { this.Name = cleanName; changed = true; }

            foreach (var l in Layers) { if (l.Correct()) { changed = true; } }

            return changed;
        }

        [DataMember]
        public GlazingConstructionTypes Type = GlazingConstructionTypes.Single;

    }

    [DataContract]
    public class GlazingConstructionSimple : BaseConstruction//BaseMaterial
    {

        [DataMember]
        public double SHGF { get; set; } = 0.837;
        /// <summary>
        /// W/m2-k
        /// </summary>
        [DataMember]
        public double UVAL { get; set; } = 0.075;

        [DataMember]
        public double VisibleTransmittance { get; set; } = 0.898;

        public GlazingConstructionSimple() { }
        public GlazingConstructionSimple(string name, string category,string comment, double tvis, double uval, double shgf ) {

            this.Name = name.Trim();
            this.Category = category.Trim();
            this.Comment = comment.Trim();
            this.VisibleTransmittance = tvis;
            this.UVAL = uval;
            this.SHGF = shgf;
            this.Comment = comment;
        }

        public bool Correct()
        {
            bool changed = false;

            string cleanName = HelperFunctions.RemoveSpecialCharacters(this.Name);
            if (this.Name != cleanName) { this.Name = cleanName; changed = true; }

            if (this.SHGF < 0.0) { this.SHGF = 0.0; changed = true; }
            if (this.SHGF > 1.0) { this.SHGF = 1.0; changed = true; }

            if (this.VisibleTransmittance < 0.0) { this.VisibleTransmittance = 0.0; changed = true; }
            if (this.VisibleTransmittance > 1.0) { this.VisibleTransmittance = 1.0; changed = true; }

            return changed;
        }

    }

    [DataContract]
    public class BaseConstruction : LibraryComponent
    {

        [DataMember]
        public virtual string Category { get; set; } = "Uncategorized";


        [DataMember]
        [Units("MJ/Kg")]
        public double EmbodiedEnergy { get; set; } = 0;

        [DataMember]
        public double EmbodiedEnergyStdDev { get; set; }

        [DataMember]
        [Units("CO2e/Kg")]
        public double EmbodiedCarbon { get; set; } = 0;

        [DataMember]
        public double EmbodiedCarbonStdDev { get; set; }

        [DataMember]
        [Units("$/m3")]
        public double Cost { get; set; } = 0;

        [DataMember]
        [Units("yr")]
        public int Life { get; set; } = 1;


        //[DataMember]
        //public double AssemblyEnergy { get; set; } = 0;
        //[DataMember]
        //public double AssemblyCarbon { get; set; } = 0;
        //[DataMember]
        //public double AssemblyCost { get; set; } = 0;
        //[DataMember]
        //public double DisassemblyCarbon { get; set; } = 0;
        //[DataMember]
        //public double DisassemblyEnergy { get; set; } = 0;


        //[DataMember]
        //public double Life { get; set; } = 0;


        public BaseConstruction() { }

        public override string ToString() { return Name; }
    }


}
