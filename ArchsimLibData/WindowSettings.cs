using System.Diagnostics;
using System.Runtime.Serialization;

namespace ArchsimLib
{
    [DataContract(IsReference = true)]
    public class WindowSettings : LibraryComponent
    {
        public WindowSettings() { }
        public override string ToString() { return this.Serialize(); }
        public bool isValid()
        {

            var props = typeof(WindowSettings).GetProperties();

            foreach (var prop in props)
            {

                object value = prop.GetValue(this, null); // against prop.Name
                if (value == null) Debug.WriteLine(prop.Name.ToString() + " IS NULL");
            }

            return true;
        }




        [DataMember]
        public windowType Type { get; set; } = windowType.External;
        [DataMember][Units("W/m2")]
        public double ShadingSystemSetPoint { get; set; } = 180;
        [DataMember]
        public double ShadingSystemTransmittance { get; set; } = 0.5;
        [DataMember]
        public bool IsVirtualPartition { get; set; } = false;
        [DataMember]
        public bool ZoneMixingIsOn { get; set; } = false;
        [DataMember]
        public double ZoneMixingDeltaTemperature { get; set; } = 2.0;
        [DataMember]
        [Units("m3/s")]
        public double ZoneMixingFlowRate { get; set; } = 0.001; // Volumenstrom m3/s  == 1 l/s

        [DataMember]
        public string Construction { get; set; } = "defaultGlazing";



        [DataMember]
        public string ShadingSystemAvailibilitySchedule { get; set; } = "AllOn";


        [DataMember]
        public double OperableArea { get; set; } = 0.8;
        [DataMember]
        public bool ShadingSystemIsOn { get; set; } = true;
        [DataMember]
        //[JsonConverter(typeof(StringEnumConverter))]
        public ShadingType ShadingSystemType { get; set; } = ShadingType.ExteriorShade;//"ExteriorShade";
        [DataMember]
        public string ZoneMixingAvailibilitySchedule { get; set; } = "AllOn";

        //AFN

        [DataMember]
        [Units("C")]
        public double AFN_TEMP_SETPT { get; set; } = 20;

        [DataMember]
        public string AFN_WIN_AVAIL { get; set; } = "AllOn";

        [DataMember]
        public double AFN_DISCHARGE_C { get; set; } = 0.65;


        [DataMember]
        public AFNVentilationControl VentControl { get; set; } = AFNVentilationControl.Temperature;


        //Frame
        [DataMember]
        public  bool HasFrame = false;

        [DataMember, Units("m")]
        public double FrameWidth { get; set; } = 0.05;
        [DataMember, Units("m")]
        public double FrameProjection { get; set; } = 0.01;
        [DataMember, Units("W/m2-K")]
        public double FrameConductance { get; set; } = 5.0;
 

        [DataMember, Units("m")]
        public double DividerWidth { get; set; } = 0.025;

        [DataMember, Units("m")]
        public double InsideSillRevealDepth { get; set; } = 0.2;


        public static WindowSettings fromJSON(string json)
        {
            return Serialization.Deserialize<WindowSettings>(json);
        }

        public string toJSON()
        {
            return Serialization.Serialize<WindowSettings>(this);
        }
    }

}
