
namespace EDScenicRouteWeb.Server.Models
{
    public interface IGalacticPoint
    {
        string Name { get; set; }
        System.Numerics.Vector3 Coordinates { get; }
    }
}
