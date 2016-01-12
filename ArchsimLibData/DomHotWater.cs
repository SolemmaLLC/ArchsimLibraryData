using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace ArchsimLib
{
    [DataContract(IsReference = true)]
    public class DomHotWater //: LibraryComponent
    {
        [DataMember]
        public double WaterTemperatureInlet { get; set; } = 10;
        [DataMember]
        public double WaterSupplyTemperature { get; set; } = 65;
        [DataMember]
        public string WaterSchedule { get; set; } = "AllOn";
        [DataMember][Units("m3/h/m2")]
        public double FlowRatePerFloorArea { get; set; } = 0.03;
        [DataMember]
        public bool IsOn = true;

        public DomHotWater()
        {
        }
        //public static DomHotWater Deserialize(string xml)
        //{
        //    return (DomHotWater)SerializeDeserialize.Deserialize(xml, typeof(DomHotWater));
        //}
        //public string XMLify()
        //{
        //    return SerializeDeserialize.Serialize(this);
        //}
        public override string ToString() { return Serialization.Serialize(this); }
    }
}
