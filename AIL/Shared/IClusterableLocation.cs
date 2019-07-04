using System;
using AIL.Clustering;

namespace AIL.Shared
{
    public interface IClusterableLocation : ILocatable
    {
        int Id { get; set; }
        int ClusterId { set; get; }
        int Weight { get; set; }

    }
}
