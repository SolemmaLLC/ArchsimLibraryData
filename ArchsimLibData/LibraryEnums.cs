using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchsimLib
{
    public enum ShadingType { ExteriorShade, InteriorShade };
    public enum DimmingItem { Off, Stepped, Continuous };
    public enum HeatRecoveryItem { None, Sensible, Enthalpy };
    public enum EconomizerItem { NoEconomizer, DifferentialDryBulb, DifferentialEnthalpy };
    public enum IdealSystemLimit { NoLimit, LimitFlowRate, LimitCapacity, LimitFlowRateAndCapacity };
    public enum InConvAlgo { Simple, TARP, TrombeWall, AdaptiveConvectionAlgorithm };
    public enum OutConvAlgo { DOE2, TARP, MoWiTT, SimpleCombined, AdaptiveConvectionAlgorithm }; //  DOE-2,  



    public enum windowType
    {
        External,
        Internal
    };


}
