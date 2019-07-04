using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIL.Shared
{
    public interface IWindow
    {
        ILocatable Center { set; get; }
        double Width { get; }
        double Height { get; }
        double Radius { get; }

    }
}
