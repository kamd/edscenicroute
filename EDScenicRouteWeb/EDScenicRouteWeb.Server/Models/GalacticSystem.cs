using System;
using System.Web;

namespace EDScenicRouteWeb.Server.Models
{
    [Serializable]
    public class GalacticSystem : IGalacticPoint
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Vector3 Coordinates { get; set; }
        public string GalMapUrl => $"https://www.edsm.net/en/system/id/{Id}/name/{HttpUtility.UrlEncode(Name)}";



        public override string ToString()
        {
            return "System: Name = " + Name;
        }

        public static readonly GalacticSystem SOL = new GalacticSystem() { Name = "Sol", Coordinates = Vector3.Zero };
    }
}
