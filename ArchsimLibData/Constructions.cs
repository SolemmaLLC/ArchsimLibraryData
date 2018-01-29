using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;

namespace ArchsimLib
{



    [DataContract]
    public class OpaqueConstruction : BaseConstruction
    {
        [DataMember]
        public List<Layer<OpaqueMaterial>> Layers = new List<Layer<OpaqueMaterial>>();

        [DataMember]
        public ConstructionCategory Type { get; set; } = ConstructionCategory.Facade;

        public OpaqueConstruction() { }


        public bool Correct()
        {
            bool changed = false;

            string cleanName = Formating.RemoveSpecialCharactersNotStrict(this.Name);
            if (this.Name != cleanName) { this.Name = cleanName; changed = true; }

            foreach (Layer<OpaqueMaterial> l in Layers) { if (l.Correct()) { changed = true; } }

            return changed;
        }

        public static OpaqueConstruction QuickConstruction(string name, ConstructionCategory type, string[] layers, double[] thickness, string category, string source, ref Library Library)
        {

            OpaqueConstruction oc = new OpaqueConstruction();
            for (int i = 0; i < layers.Length; i++)
            {
                try
                {
                    if (thickness.Length != layers.Length) { continue; }
                    if (!(thickness[i] > 0)) { continue; }

                    if (Library.OpaqueMaterials.Any(x => x.Name == layers[i]))
                    {

                        var mat = Library.OpaqueMaterials.First(o => o.Name == layers[i]);
                        Layer<OpaqueMaterial> layer = new Layer<OpaqueMaterial>(thickness[i], mat);
                        oc.Layers.Add(layer);
                    }
                    else
                    {

                        Debug.WriteLine("ERROR: " + "Could not find " + layers[i]);
                        Logger.WriteLine("ERROR: " + "Could not find " + layers[i]);
                        return null;

                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }

            }

            oc.Name = name;
            oc.Type = type;
            oc.Category = category;
            oc.DataSource = source;


            Library.Add(oc);
            return oc;

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

            string cleanName = Formating.RemoveSpecialCharactersNotStrict(this.Name);
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
        public GlazingConstructionSimple(string name, string category, string comment, double tvis, double uval, double shgf)
        {

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

            string cleanName = Formating.RemoveSpecialCharactersNotStrict(this.Name);
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
        [Units("MJ/m2")]
        public double EmbodiedEnergy { get; set; } = 0;

        //[DataMember]
        //public double EmbodiedEnergyStdDev { get; set; }

        [DataMember]
        [Units("kgCO2eq/m2")]
        public double EmbodiedCarbon { get; set; } = 0;

        //[DataMember]
        //public double EmbodiedCarbonStdDev { get; set; }

        [DataMember]
        [Units("$/m2")]
        public double Cost { get; set; } = 0;

        [DataMember]
        [Units("yr")]
        public int Life { get; set; } = 1;

        public BaseConstruction() { }

        public override string ToString() { return Name; }
    }


}
