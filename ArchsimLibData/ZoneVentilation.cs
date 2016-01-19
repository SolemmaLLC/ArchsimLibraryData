using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace ArchsimLib
{
     [DataContract(IsReference = true)]
    public class ZoneVentilation : LibraryComponent
    {
        [DataMember]
        public double InfiltrationAch { get; set; } = 0.1;
        [DataMember]
        public double ScheduledVentilationAch { get; set; } = 0.6;
        [DataMember]
        public bool NatVentIsOn { get; set; } = false;
        [DataMember]
        public bool SchedVentIsOn { get; set; } = false;
        [DataMember]
        public bool InfiltrationIsOn { get; set; } = true;
        [DataMember]
        public string ScheduledVentilationSchedule { get; set; } = "AllOn";
        [DataMember]
        public string NatVentSchedule { get; set; } = "AllOn";
        [DataMember]
        public double ScheduledVentilationSetPoint { get; set; } = 18;
        [DataMember]
        public bool BuoyancyDrivenIsOn { get; set; } = true;
        [DataMember]
        public bool WindDrivenIsOn { get; set; } = false;
        [DataMember]
        public double NatVentSetPoint { get; set; } = 18;
        [DataMember]
        public double NatVentMinOutAirTemp { get; set; } = 0;
        [DataMember]
        public double NatVentMaxOutAirTemp { get; set; } = 30;
        [DataMember]
        public double NatVentMaxRelHum { get; set; } = 90;
        [DataMember]
        public bool AFN { get; set; } = false;

        public ZoneVentilation()
        {
        }

        public override string ToString() { return Serialization.Serialize(this); }
    }
}
