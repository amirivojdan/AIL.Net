using System.Device.Location;
using AIL.Shared;

namespace AIL.Geo
{
    public class GeoHelper
    {
        public static double GetDistance(ILocatable p1, ILocatable p2)
        {
            var g1 = new GeoCoordinate(p1.X, p1.Y);
            var g2 = new GeoCoordinate(p2.X, p2.Y);
            return g1.GetDistanceTo(g2);
        }
    }
}
