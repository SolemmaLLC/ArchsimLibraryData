using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Diagnostics;

namespace ArchsimLib
{
    [DataContract(IsReference = true)]
    public class ZoneDefinition : LibraryComponent
    {
        [DataMember, DefaultValue(InConvAlgo.TARP),JsonConverter(typeof(StringEnumConverter))]
        public InConvAlgo SurfaceConvectionModelInside { get; set; } = InConvAlgo.TARP;

        [DataMember, DefaultValue(OutConvAlgo.DOE2),JsonConverter(typeof(StringEnumConverter))]
        public OutConvAlgo SurfaceConvectionModelOutside { get; set; } = OutConvAlgo.DOE2;

        [DataMember, DefaultValue(1.0)]
        public double ZoneMultiplier { get; set; } = 1.0;

        [DataMember, DefaultValue(1)]
        public int ZonePriority { get; set; } = 1;

        [DataMember, DefaultValue(1)]
        public double DaylightMeshResolution { get; set; } = 1.0;

        [DataMember, DefaultValue(0.8)]
        public double DaylightWorkplaneHeight { get; set; } = 0.8;


        // use this istead of PartritionRatio
        [DataMember, DefaultValue("defaultConstruction")]
        public string InternalMassConstruction { get; set; } = "defaultConstruction";    
        // use this istead of PartritionRatio
        [DataMember, DefaultValue(0)]
        public double InternalMassExposedAreaPerArea { get; set; } = 0;


        //MAT
        //-----
        [DataMember]
        public ZoneConstruction Materials { get; set; } = new ZoneConstruction();

        //LOADS
        //-----
        [DataMember]
        public ZoneLoad Loads { get; set; } = new ZoneLoad();

        //IdealLoadsAirSystem
        //-------------------
        [DataMember]
        public ZoneConditioning Conditioning { get; set; } = new ZoneConditioning();

        //DOM HOT WAT
        [DataMember]
        public DomHotWater DomHotWater { get; set; } = new DomHotWater();

        //AIRFLOW / VENT
        //--------------
        [DataMember]
        public ZoneVentilation Ventilation { get; set; } = new ZoneVentilation();


        public ZoneDefinition()
        {
        }

        public static ZoneDefinition Clone(ZoneDefinition zsc)
        {
            string s = zsc.toJSON();
            return ZoneDefinition.fromJSON(s);
        }

        public override string ToString() { return this.Serialize(); }
        public bool isValid()
        {

            var props = typeof(ZoneDefinition).GetProperties();

            foreach (var prop in props)
            {

                object value = prop.GetValue(this, null); // against prop.Name
                if (value == null) Debug.WriteLine(prop.Name + " IS NULL");
            }

            return true;
        }




        public static ZoneDefinition fromJSON(string json)
        {
            return Serialization.Deserialize<ZoneDefinition>(json);
        }

        public string toJSON()
        {
            return Serialization.Serialize<ZoneDefinition>(this);
        }

    }
}
