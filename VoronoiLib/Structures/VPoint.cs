using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoronoiLib.Structures
{
    public class VPoint
    {
        public double X { get; }
        public double Y { get; }

        internal VPoint(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
