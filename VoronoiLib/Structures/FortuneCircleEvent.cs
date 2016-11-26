using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoronoiLib.Structures
{
    internal class FortuneCircleEvent : FortuneEvent
    {
        internal VPoint Lowest { get; }
        internal double Center { get; }
        internal RBTreeNode<BeachSection> ToDelete { get; }

        internal FortuneCircleEvent(VPoint lowest, double center, RBTreeNode<BeachSection> toDelete)
        {
            Lowest = lowest;
            Center = center;
            ToDelete = toDelete;
        }

        public int CompareTo(FortuneEvent other)
        {
            return Y.CompareTo(other.Y);
        }

        public double X => Lowest.X;
        public double Y => Lowest.Y;
    }
}
