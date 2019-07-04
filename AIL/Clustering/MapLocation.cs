using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIL.Shared;

namespace AIL.Clustering
{
    public class MapLocation : IClusterableLocation
    {
        public int Id { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public int ClusterId { get; set; }
        public int Weight { get; set; }
       
    }
}
