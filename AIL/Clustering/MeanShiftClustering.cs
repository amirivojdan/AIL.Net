using System.Collections.Generic;
using AIL.Shared;

namespace AIL.Clustering
{
    public class MeanShiftClustering
    {
        private List<IWindow> TessellateSpace(List<IClusterableLocation> locations, int resolution)
        {
            var containingWindow = Helper.GetContainingWindow(locations);
            var topleftX = containingWindow.Center.X - containingWindow.Width / 2;
            var topleftY = containingWindow.Center.Y - containingWindow.Height / 2;

            var widthUnit = containingWindow.Width / resolution;
            var heightUnit = containingWindow.Height / resolution;

            var tesselationWindows = new List<IWindow>();

            for (int j = 0; j < resolution; j++)
            {
                var yCenter = topleftY + heightUnit / 2 + j * heightUnit;
                for (int i = 0; i < resolution; i++)
                {
                    var xCenter = topleftX + widthUnit / 2 + i * widthUnit;
                    tesselationWindows.Add(new SlidingWindow(new MapLocation { X = xCenter, Y = yCenter }, widthUnit, heightUnit));
                }
            }

            return tesselationWindows;
        }

        public List<IWindow> Learn(List<IClusterableLocation> locations, int resolution)
        {
            var windows = TessellateSpace(locations, 3);
            var clusterWindows = new List<IWindow>();
            foreach (var window in windows)
            {
                for (int i = 0; i < 100; i++)
                {
                    int pointsCount = 0;
                    double xSum = 0;
                    double ySum = 0;
                    foreach (var location in locations)
                    {
                        var distance = Helper.EuclideanDistance(window.Center, location);
                        if (distance <= window.Radius)
                        {
                            xSum += location.X;
                            ySum += location.Y;
                            pointsCount++;
                        }
                    }

                    window.Center.X = xSum / pointsCount;
                    window.Center.Y = ySum / pointsCount;

                }
            }

            foreach (var window in windows)
            {
                int pointsCount = 0;

                foreach (var location in locations)
                {
                    var distance = Helper.EuclideanDistance(window.Center, location);
                    if (distance <= window.Radius)
                    {
                        pointsCount++;
                    }
                }
                if (pointsCount != 0)
                {
                    clusterWindows.Add(window);
                }

            }

            return clusterWindows;
        }
    }
}
