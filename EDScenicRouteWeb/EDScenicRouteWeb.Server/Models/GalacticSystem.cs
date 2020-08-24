using System;
using System.Numerics;
using System.Web;

namespace EDScenicRouteWeb.Server.Models
{
    [Serializable]
    public class GalacticSystem : IGalacticPoint
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public Vector3 Coordinates => new Vector3(X, Y, Z);
        public string GalMapUrl => $"https://www.edsm.net/en/system/id/{Id}/name/{HttpUtility.UrlEncode(Name)}";
        
        public override string ToString()
        {
            return "System: Name = " + Name;
        }
    }
}
