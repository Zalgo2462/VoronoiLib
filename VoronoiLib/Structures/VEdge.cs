using System;

namespace VoronoiLib.Structures
{
    public class VEdge
    {
        public VPoint Start { get; internal set; }
        public VPoint End { get; internal set; }
        public FortuneSite Left { get; }
        public FortuneSite Right { get; }
        public double SlopeRise { get; }
        public double SlopeRun { get; }
        public double? Intercept { get; }

        public VEdge Neighbor { get; internal set; }

        internal VEdge(VPoint start, FortuneSite left, FortuneSite right)
        {
            Start = start;
            Left = left;
            Right = right;
            //from negative reciprocal of slope of line from left to right
            //ala m = (left.y -right.y / left.x - right.x)
            SlopeRise = left.X - right.X;
            SlopeRun = -(left.Y - right.Y);
            Intercept = null;

            if (SlopeRise.ApproxEqual(0) || SlopeRun.ApproxEqual(0)) return;
            var m = SlopeRise/SlopeRun;
            Intercept = start.Y - m*start.X;
        }
    }
}
