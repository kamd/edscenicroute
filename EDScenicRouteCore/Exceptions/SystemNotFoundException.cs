using System;
using System.Collections.Generic;
using System.Text;

namespace EDScenicRouteCore.DataUpdates
{
    public class SystemNotFoundException : Exception
    {
        public string SystemName { get; set; }
    }
}
