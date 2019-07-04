using System;
using System.Collections.Generic;
using System.Linq;
using AIL.Clustering;

namespace AIL.Shared
{
    public class Helper
    {
        public static double EuclideanDistance(ILocatable p1, ILocatable p2)
        {
            var xDif = p1.X - p2.X;
            var yDif = p1.Y - p2.Y;
            return Math.Sqrt(xDif * xDif + yDif * yDif);
        }

        public static IWindow GetContainingWindow(List<IClusterableLocation> locations)
        {
            var minX = locations.Min(i => i.X);
            var minY = locations.Min(i => i.Y);

            var maxX = locations.Max(i => i.X);
            var maxY = locations.Max(i => i.Y);

            var xCenter = minX + (maxX - minX) / 2;
            var yCenter = minY + (maxY - minY) / 2;

            return new SlidingWindow(new MapLocation { X = xCenter, Y = yCenter }, maxX - minX, maxY - minY);
        }

    }
}
