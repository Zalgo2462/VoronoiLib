using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoronoiLib.Structures
{
    class FortuneTreeBranch : FortuneTreeNode
    {
        internal FortuneSite Left { get; }
        internal FortuneSite Right { get; }
        internal FortuneTreeBranch(FortuneSite left, FortuneSite right)
        {
            Left = left;
            Right = right;
        }

        public int GetX(int directrix)
        {
            return (int) Parabola.IntersectParabolaX(Left.X, Left.Y, Right.X, Right.Y, directrix);
        }

        public int GetY(int directrix)
        {
            return (int) Parabola.EvalParabola(Left.X, Left.Y, directrix, GetX(directrix));
        }
    }
}
