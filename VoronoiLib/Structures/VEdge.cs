using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoronoiLib.Structures
{
    public class VEdge
    {
        public VPoint Start { get; }
        public FortuneSite Left { get; }
        public FortuneSite Right { get; }
        public VEdge Neighbor { get; internal set; }

        //called where 
        //start = para, x= new site.x intersect
        //left = para site
        //right = new site
        internal VEdge(VPoint start, FortuneSite left, FortuneSite right)
        {
            Start = start;
            Left = left;
            Right = right;
        }

    }
}
