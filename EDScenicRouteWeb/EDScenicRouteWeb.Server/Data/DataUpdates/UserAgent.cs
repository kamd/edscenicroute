using VersionInfo = EDScenicRouteWeb.Server.Models.VersionInfo;

namespace EDScenicRouteWeb.Server.Data.DataUpdates
{
    static class UserAgent
    {
        public static string UserAgentString { get; } = $"EDScenicRouteFinder-elite.kamd.me.uk/{VersionInfo.Version}";
    }
}
