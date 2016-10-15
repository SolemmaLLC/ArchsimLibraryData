using System.Runtime.Serialization;

namespace ArchsimLib
{

  [DataContract(IsReference = true)]
    public class ZoneConditioning : LibraryComponent
    {
        [DataMember]
        public double CoolingCoeffOfPerf { get; set; } = 1;

        [DataMember]
        public double HeatingCoeffOfPerf { get; set; } = 1;



        [DataMember]
        public double HeatingSetpoint { get; set; } = 20;//needed for trnsys 
        [DataMember]
        public double CoolingSetpoint { get; set; } = 26;//needed for trnsys


        //[DataMember]
        //public string HeatingSetpointSchedule { get; set; }= "AllOn";
        //[DataMember]
        //public string CoolingSetpointSchedule { get; set; }= "AllOn";


        [DataMember]
        public bool HeatIsOn { get; set; } = true;
        [DataMember]
        public bool CoolIsOn { get; set; } = true;
        [DataMember]
        public bool MechVentIsOn { get; set; } = false;
        [DataMember]
        public bool HumidistatOnOff { get; set; } = false;


        [DataMember]
        //[JsonConverter(typeof(StringEnumConverter))]
        public IdealSystemLimit HeatingLimitType { get; set; } = IdealSystemLimit.NoLimit;// "NoLimit";
        [DataMember]
        //[JsonConverter(typeof(StringEnumConverter))]
        public IdealSystemLimit CoolingLimitType { get; set; } = IdealSystemLimit.NoLimit;// "NoLimit";
        [DataMember]
        [Units("W/m2")]
        public double MaxHeatingCapacity { get; set; } = 100; //W/m2
        [DataMember]
        [Units("W/m2")]
        public double MaxCoolingCapacity { get; set; } = 100;
        [DataMember]
        [Units("m3/s/m2")]
        public double MaxHeatFlow { get; set; } = 100; //m3/s/m2
        [DataMember]
        [Units("m3/s/m2")]
        public double MaxCoolFlow { get; set; } = 100;
        [DataMember]
        public string HeatingSchedule { get; set; } = "AllOn";
        [DataMember]
        public string CoolingSchedule { get; set; } = "AllOn";
    


        [DataMember]
        public string MechVentSchedule { get; set; } = "AllOn";
        [DataMember][Units("L/s/p")]
        public double MinFreshAirPerson { get; set; } = 2.5;
        [DataMember][Units("L/s/m2")]
        public double MinFreshAirArea { get; set; } = 0.3;
        [DataMember]
        //[JsonConverter(typeof(StringEnumConverter))]
        public EconomizerItem EconomizerType { get; set; } = EconomizerItem.NoEconomizer;//"NoEconomizer";
        [DataMember]
        //[JsonConverter(typeof(StringEnumConverter))]
        public HeatRecoveryItem HeatRecoveryType { get; set; } = HeatRecoveryItem.None;// "None";
        [DataMember]
        public double HeatRecoveryEfficiencySensible { get; set; } = 0.7;
        [DataMember]
        public double HeatRecoveryEfficiencyLatent { get; set; } = 0.65;



        [DataMember]
        [Units("RH%")]
        public double MinHumidity { get; set; } = 20; 
        [DataMember]
        [Units("RH%")]
        public double MaxHumidity { get; set; } = 80;

        public ZoneConditioning()
        {
        }
        public override string ToString() { return Serialization.Serialize(this); }
    }
}
