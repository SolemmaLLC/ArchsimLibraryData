using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ArchsimLib
{


    [DataContract(IsReference = true)]
    public class FloorDefinition : LibraryComponent
    {
        [DataMember]
        public string Type = "Int";
        [DataMember]
        public string BuildingID = "";
        [DataMember]
        public string Floor = "";
        [DataMember]
        public double NorthWWR = 0.5;
        [DataMember]
        public double EastWWR = 0.5;
        [DataMember]
        public double SouthWWR = 0.5;
        [DataMember]
        public double WestWWR = 0.5;
        [DataMember]
        public double RoofWWR = 0.5;
        [DataMember]
        public string NorthWindowDefinition = "";
        [DataMember]
        public string EastWindowDefinition = "";
        [DataMember]
        public string SouthWindowDefinition = "";
        [DataMember]
        public string WestWindowDefinition = "";
        [DataMember]
        public string RoofWindowDefinition = "";



        [DataMember]
        public string PerimeterZoneDefinition { get; set; } = "";
        [DataMember]
        public string CoreZoneDefinition        { get; set; } = "";



    }



    //[DataContract(IsReference = true)]
    //public class BuildingDefinition : LibraryComponent
    //{

    //    List<FloorDefinition> Floors = new List<FloorDefinition>();

    //    [DataMember, DefaultValue(0)]
    //    public int BasementFloors { get; set; } = 0;

    //}

    //[DataContract(IsReference = true)]
    //public class FloorDefinition : LibraryComponent
    //{
    //    [DataMember]
    //    public FloorType Type = FloorType._Unset_;
    //    [DataMember, DefaultValue(1)]
    //    public int Repeat { get; set; } = 1;


    //    [DataMember]
    //    public ZoneDefinition Perimeter { get; set; } = new ZoneDefinition();
    //    [DataMember]
    //    public ZoneDefinition Core { get; set; } = new ZoneDefinition();



    //}


    //public enum FloorType {

    //    _Unset_,
    //    Basement,
    //    GroundFloor,
    //    Roof,
    //    Floor

    //}

}


