using System;
using System.Collections.Generic;
using System.Linq;
using AIL.Shared;

namespace AIL.Clustering
{
    public class AglomerativeClustering
    {

        public class DistanceUnit
        {
            public double Distance { set; get; }
            public int I { set; get; }
            public int J { set; get; }
        }

        public List<DistanceUnit> GenerateDistanceMatrix(List<IClusterableLocation> locations)
        {
            var distanceList = new List<DistanceUnit>();
            for (int i = 1; i < locations.Count; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    var distance = Helper.EuclideanDistance(locations[i], locations[j]);
                    if (double.IsNaN(distance))
                    {
                        distance = 0;
                    }
                    distanceList.Add(new DistanceUnit
                    {
                        Distance = distance,
                        I = i,
                        J = j
                    });
                }
            }

            return distanceList;
        }

        public List<IClusterableLocation> MergeNearestDistancePoints(List<IClusterableLocation> locations, List<DistanceUnit> distanceMatrix)
        {
            var locationsCopy = locations.ToList();

            var minDistance = distanceMatrix.Min(p => p.Distance);
            var minDistanceItems = distanceMatrix.Where(i => i.Distance == minDistance).ToList();

            foreach (var minDistanceItem in minDistanceItems)
            {
                var totalWeight = locations[minDistanceItem.I].Weight + locations[minDistanceItem.J].Weight;
                var p1 = locations[minDistanceItem.I];
                var p2 = locations[minDistanceItem.J];

                var x = (p1.X * p1.Weight + p2.X * p2.Weight) / totalWeight;
                var y = (p1.Y * p1.Weight + p2.Y * p2.Weight) / totalWeight;

                locationsCopy.Add(new MapLocation
                {
                    X = x,
                    Y = y,
                    Weight = totalWeight
                });
                locationsCopy.Remove(locations[minDistanceItem.I]);
                locationsCopy.Remove(locations[minDistanceItem.J]);
            }

            return locationsCopy;
        }

        public List<IClusterableLocation> RemoveSameLocations(List<IClusterableLocation> locations)
        {
            var newlist = new List<IClusterableLocation>();
            foreach (var clusterableLocation in locations)
            {
                if (!newlist.Any(i => i.X == clusterableLocation.X && i.Y == clusterableLocation.Y))
                {
                    var count = locations.Count(i => i.X == clusterableLocation.X && i.Y == clusterableLocation.Y);
                    newlist.Add(new MapLocation
                    {
                        X = clusterableLocation.X,
                        Y = clusterableLocation.Y,
                        Weight = count
                    });
                }

            }

            return newlist;
        }
        public List<List<IClusterableLocation>> Learn(List<IClusterableLocation> locations)
        {
            var layers = new List<List<IClusterableLocation>>();
            locations = RemoveSameLocations(locations);

            layers.Add(locations);
            while (true)
            {
                var lastLayer = layers.LastOrDefault();

                var distanceMatrix = GenerateDistanceMatrix(lastLayer);
                var mergedPoints = MergeNearestDistancePoints(lastLayer, distanceMatrix);
                layers.Add(mergedPoints);
                if (mergedPoints.Count == 1)
                    break;
            }

            return layers;
        }
    }
}
