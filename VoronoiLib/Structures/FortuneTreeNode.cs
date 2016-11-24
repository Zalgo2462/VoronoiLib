using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoronoiLib.Structures
{
    interface FortuneTreeNode
    {
        int GetX(int directrix);
        int GetY(int directrix);
    }
}
