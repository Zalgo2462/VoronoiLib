using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoronoiLib.Structures
{
    internal class FortuneCircleEvent : FortuneEvent
    {
        internal FortuneSite Lowest { get; }
        internal FortuneTreeLeaf ToDelete { get; }

        internal FortuneCircleEvent(FortuneSite lowest, FortuneTreeLeaf toDelete)
        {
            Lowest = lowest;
            ToDelete = toDelete;
        }

        public int CompareTo(FortuneEvent other)
        {
            return Y.CompareTo(other.Y);
        }

        public int X => Lowest.X;
        public int Y => Lowest.Y;
    }
}
