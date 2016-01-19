using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace ArchsimLib
{
  [DataContract(IsReference = true)]
    public class ZoneLoad :LibraryComponent
    {
        [DataMember]
        public DimmingItem DimmingType { get; set; } = DimmingItem.Continuous; //"Continuous";
        [DataMember][Units("p/m2")]
        public double PeopleDensity { get; set; } = 0.2;
        [DataMember]
        [Units("W/m2")]
        public double EquipmentPowerDensity { get; set; } = 12;
        [DataMember]
        [Units("W/m2")]
        public double LightingPowerDensity { get; set; } = 12;
        [DataMember]
        [Units("lux")]
        public double IlluminanceTarget { get; set; } = 500;
        [DataMember]
        public string OccupancySchedule { get; set; } = "AllOn";
        [DataMember]
        public string EquipmentAvailibilitySchedule { get; set; } = "AllOn";
        [DataMember]
        public string LightsAvailibilitySchedule { get; set; } = "AllOn";
        [DataMember]
        public bool PeopleIsOn { get; set; } = true;
        [DataMember]
        public bool EquipmentIsOn { get; set; } = true;
        [DataMember]
        public bool LightsIsOn { get; set; } = true;

        public ZoneLoad()
        {
        }
        public override string ToString() { return Serialization.Serialize(this); }
    }
}
