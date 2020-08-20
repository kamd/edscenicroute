using System;

namespace EDScenicRouteWeb.Server.Exceptions
{
    public class SystemNotFoundException : Exception
    {
        public string SystemName { get; set; }
    }
}
