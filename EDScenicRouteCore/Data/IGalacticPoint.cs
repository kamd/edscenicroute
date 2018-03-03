using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace EDScenicRouteCore.Data
{
    public interface IGalacticPoint
    {
        string Name { get; set; }
        Vector3 Coordinates { get; set; }
    }
}
