using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ArchsimLib.Definitions
{
    [DataContract(IsReference = true)]
    public class BuildingDefinition : LibraryComponent
    {

        List<FloorDefinition> Floors = new List<FloorDefinition>();

        [DataMember, DefaultValue(0)]
        public int BasementFloors { get; set; } = 0;

    }

    [DataContract(IsReference = true)]
    public class FloorDefinition : LibraryComponent
    {
        [DataMember]
        public FloorType Type = FloorType._Unset_;
        [DataMember, DefaultValue(1)]
        public int Repeat { get; set; } = 1;


        [DataMember]
        public ZoneDefinition Perimeter { get; set; } = new ZoneDefinition();
        [DataMember]
        public ZoneDefinition Core { get; set; } = new ZoneDefinition();



    }


    public enum FloorType {

        _Unset_,
        Basement,
        GroundFloor,
        Roof,
        Floor

    }

}


