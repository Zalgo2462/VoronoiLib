using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoronoiLib.Structures
{
    class FortuneTreeLeaf : FortuneTreeNode
    {
        internal FortuneSite Site { get; }
        internal FortuneCircleEvent DeleteEvent { get; set; }
        internal FortuneTreeLeaf(FortuneSite site)
        {
            Site = site;
            DeleteEvent = null;
        }

        public int GetX(int directrix)
        {
            return Site.X;
        }

        public int GetY(int directrix)
        {
            return Site.Y;
        }
    }
}
