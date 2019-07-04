using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIL.Shared;

namespace AIL.Clustering
{
    public class SlidingWindow : IWindow
    {
        private SlidingWindow()
        {

        }

        public SlidingWindow(ILocatable location, double width, double height)
        {
            Center = location;
            _height = height;
            _width = width;
            _radius = Math.Min(height, width) / 2;
        }
        public SlidingWindow(ILocatable location, double radius)
        {
            Center = location;
            _radius = radius;

            _height = radius * 2;
            _width = radius * 2;
        }
        public ILocatable Center { get; set; }

        private double _width;
        public double Width
        {
            get { return _width; }
        }
        private double _height;
        public double Height
        {
            get { return _height; }
        }

        private double _radius;
        public double Radius
        {
            get { return _radius; }
        }
    }
}
