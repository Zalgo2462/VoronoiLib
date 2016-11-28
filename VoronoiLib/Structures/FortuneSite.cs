using System.Collections.Generic;

namespace VoronoiLib.Structures
{
    public class FortuneSite
    {
        public double X { get; }
        public double Y { get; }

        internal List<VEdge> _Cell { get; private set; }

        internal List<FortuneSite> _Neighbors { get; private set; }

        public VEdge[] Cell { get; private set; }

        public FortuneSite[] Neighbors { get; private set; }

        public FortuneSite(double x, double y)
        {
            X = x;
            Y = y;
        }

        internal void Finish()
        {
            Cell = _Cell.ToArray();
            Neighbors = _Neighbors.ToArray();
            _Cell = null;
            _Neighbors = null;
        }
    }
}
