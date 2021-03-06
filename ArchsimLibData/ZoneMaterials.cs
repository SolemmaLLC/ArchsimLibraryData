﻿using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

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



        // use this istead of PartritionRatio
        [DataMember, DefaultValue("defaultConstruction")]
        public string InternalMassConstruction { get; set; } = "defaultConstruction";
        // use this istead of PartritionRatio
        [DataMember, DefaultValue(0)]
        public double InternalMassExposedAreaPerArea { get; set; } = 0;



		[DataMember, DefaultValue(InConvAlgo.TARP)] //, JsonConverter(typeof(StringEnumConverter)),   //---- enum converter removed due to:  http://stackoverflow.com/questions/37776866/json-net-8-0-error-creating-stringenumconverter-on-mono-4-5-mac
		public InConvAlgo SurfaceConvectionModelInside { get; set; } = InConvAlgo.TARP;

        [DataMember, DefaultValue(OutConvAlgo.DOE2)] //, JsonConverter(typeof(StringEnumConverter))
		public OutConvAlgo SurfaceConvectionModelOutside { get; set; } = OutConvAlgo.DOE2;



        [DataMember, DefaultValue(1)]
        public int ZonePriority { get; set; } = 1;

        [DataMember, DefaultValue(1)]
        public double DaylightMeshResolution { get; set; } = 1.0;

        [DataMember, DefaultValue(0.8)]
        public double DaylightWorkplaneHeight { get; set; } = 0.8;

    }
}
