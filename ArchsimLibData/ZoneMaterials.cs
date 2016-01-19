using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Diagnostics;

namespace ArchsimLib
{
    [DataContract(IsReference = true)]
    public class ZoneConstruction : LibraryComponent
    {
        public ZoneConstruction() { }


        public override string ToString() { return Serialization.Serialize(this); }
        public bool isValid()
        {

            var props = typeof(ZoneConstruction).GetProperties();

            foreach (var prop in props)
            {
                object value = prop.GetValue(this, null); // against prop.Name
                if (value == null) Debug.WriteLine(prop.Name + " IS NULL");
            }

            return true;
        }

        [DataMember]
        public string RoofConstruction { get; set; } = "defaultConstruction";
        [DataMember]
        public string FacadeConstruction { get; set; } = "defaultConstruction";
        [DataMember]
        public string SlabConstruction { get; set; } = "defaultConstruction";
        [DataMember]
        public string PartitionConstruction { get; set; } = "defaultConstruction";
        [DataMember]
        public string GroundConstruction { get; set; } = "defaultConstruction";
        [DataMember]
        public bool GroundIsAdiabatic { get; set; } = false;
        [DataMember]
        public bool RoofIsAdiabatic { get; set; } = false;
        [DataMember]
        public bool FacadeIsAdiabatic { get; set; } = false;
        [DataMember]
        public bool SlabIsAdiabatic { get; set; } = false;
        [DataMember]
        public bool PartitionIsAdiabatic { get; set; } = false;

    }
}
