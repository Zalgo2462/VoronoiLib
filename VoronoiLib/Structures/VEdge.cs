using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoronoiLib.Structures
{
    public class VHalfEdge
    {
        public VPoint Start { get; }
        public FortuneSite Site { get; }
        public double SlopeRise { get; }
        public double SlopeRun { get; }
        public double? Intercept { get; }

        public VHalfEdge Neighbor { get; internal set; }

        internal VHalfEdge(VPoint start, FortuneSite left, FortuneSite right)
        {
            Start = start;
            Site = left;
            //from negative reciprocal of slope of line from left to right
            //ala m = (left.y -right.y / left.x - right.x)
            SlopeRise = right.X - left.X;
            SlopeRun = left.Y - right.Y;
            Intercept = null;

            if (SlopeRise.ApproxEqual(0) || SlopeRun.ApproxEqual(0)) return;
            double m = SlopeRise/SlopeRun;
            Intercept = start.Y - m*start.X;
        }
    }
}
