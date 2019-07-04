using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AIL.Shared;

namespace AIL.LineSimplification
{
    public class RamerDouglasPeucker
    {
        public double Tolerance { set; get; }
        public RamerDouglasPeucker(double tolerance)
        {
            Tolerance = tolerance;
        }
        public List<IClusterableLocation> DouglasPeuckerReduction(List<IClusterableLocation> points)
        {
            if (points == null || points.Count < 3)
                return points;

            var firstPoint = 0;
            var lastPoint = points.Count - 1;
            var pointIndexsToKeep = new List<int>();

            //Add the first and last index to the keepers
            pointIndexsToKeep.Add(firstPoint);
            pointIndexsToKeep.Add(lastPoint);

            //The first and the last point cannot be the same
            while (points[firstPoint].Equals(points[lastPoint]))
            {
                lastPoint--;
            }

            DouglasPeuckerReduction(points, firstPoint, lastPoint, ref pointIndexsToKeep);

            var returnPoints = new List<IClusterableLocation>();
            pointIndexsToKeep.Sort();
            foreach (int index in pointIndexsToKeep)
            {
                returnPoints.Add(points[index]);
            }

            return returnPoints;
        }

        /// <summary>
        /// Douglases the peucker reduction.
        /// </summary>
        /// <param name="points">The points.</param>
        /// <param name="firstPoint">The first point.</param>
        /// <param name="lastPoint">The last point.</param>
        /// <param name="pointIndexsToKeep">The point index to keep.</param>
        private void DouglasPeuckerReduction(List<IClusterableLocation> points, int firstPoint, int lastPoint, ref List<int> pointIndexsToKeep)
        {
            double maxDistance = 0;
            int indexFarthest = 0;

            for (int index = firstPoint; index < lastPoint; index++)
            {
                double distance = PerpendicularDistance
                    (points[firstPoint], points[lastPoint], points[index]);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    indexFarthest = index;
                }
            }

            if (maxDistance > Tolerance && indexFarthest != 0)
            {
                //Add the largest point that exceeds the tolerance
                pointIndexsToKeep.Add(indexFarthest);

                DouglasPeuckerReduction(points, firstPoint, indexFarthest, ref pointIndexsToKeep);
                DouglasPeuckerReduction(points, indexFarthest, lastPoint, ref pointIndexsToKeep);
            }
        }

        /// <summary>
        /// The distance of a point from a line made from point1 and point2.
        /// </summary>
        /// <param name="p1">The PT1.</param>
        /// <param name="p2">The PT2.</param>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        private double PerpendicularDistance(ILocatable p1, ILocatable p2, ILocatable p)
        {
            var area = Math.Abs(.5 * (p1.X * p2.Y + p2.X *
            p.Y + p.X * p1.Y - p2.X * p1.Y - p.X *
            p2.Y - p1.X * p.Y));
            var bottom = Math.Sqrt(Math.Pow(p1.X - p2.X, 2) +
            Math.Pow(p1.Y - p2.Y, 2));
            var height = area / bottom * 2;
            return height;
        }
    }



}
