using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoronoiLib.Structures
{
    interface FortuneEvent : IComparable<FortuneEvent>
    {
        double X { get; }
        double Y { get; }
    }
}
