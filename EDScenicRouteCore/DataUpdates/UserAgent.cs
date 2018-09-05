using System;
using System.Collections.Generic;
using System.Text;
using EDScenicRouteCoreModels;

namespace EDScenicRouteCore.DataUpdates
{
    static class UserAgent
    {
        public static string UserAgentString { get; } = $"EDScenicRouteFinder-elite.kamd.me.uk/{VersionInfo.Version}";
    }
}
