using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using AIL.Shared;

namespace AIL.Clustering
{
    public class KMeansClustering
    {

        public int NumberOfClusters { set; get; }
        public int MaxIterations { set; get; }
        public KMeansClustering(int maxIterations, int numberOfClusters = 3)
        {
            NumberOfClusters = numberOfClusters;
            Centroids = new List<IClusterableLocation>();
            MaxIterations = maxIterations;
        }

        public List<IClusterableLocation> Centroids { set; get; }

        private void InitializeClusters(List<IClusterableLocation> locations)
        {
            var random = new Random(DateTime.Now.Millisecond);

            //To ensure each cluster at least contains one member!
            for (int i = 0; i < NumberOfClusters; i++)
            {
                locations[i].ClusterId = i;
                Centroids.Add(new MapLocation());
            }

            for (int i = NumberOfClusters; i < locations.Count; i++)
            {
                locations[i].ClusterId = random.Next(0, NumberOfClusters);
            }

        }

        private void ComputeMeans(List<IClusterableLocation> locations)
        {
            foreach (var centroid in Centroids)
            {
                centroid.X = 0;
                centroid.Y = 0;
                centroid.Weight = 0;

            }

            foreach (var location in locations)
            {
                Centroids[location.ClusterId].X += location.X;
                Centroids[location.ClusterId].Y += location.Y;
                Centroids[location.ClusterId].Weight++;
            }

            foreach (var centroid in Centroids)
            {
                centroid.X /= centroid.Weight;
                centroid.Y /= centroid.Weight;
            }
        }

        private void UpdateClusters(List<IClusterableLocation> locations)
        {
            foreach (var location in locations)
            {
                var minCentroidDistanceIndex = 0;
                double minDistance = Helper.EuclideanDistance(Centroids[0], location);
                for (int i = 0; i < NumberOfClusters; i++)
                {
                    var distance = Helper.EuclideanDistance(Centroids[i], location);
                    if (distance < minDistance)
                    {
                        minCentroidDistanceIndex = i;
                        minDistance = distance;
                    }
                }

                location.ClusterId = minCentroidDistanceIndex;
            }
        }



        public List<IClusterableLocation> Learn(List<IClusterableLocation> locations)
        {
            InitializeClusters(locations);
            for (int i = 0; i < MaxIterations; i++) //if centroids  didn't changed
            {
                ComputeMeans(locations);
                UpdateClusters(locations);
            }

            return locations;
        }


    }
}