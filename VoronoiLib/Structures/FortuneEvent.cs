using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoronoiLib.Structures
{
    abstract class FortuneEvent : IComparable<FortuneEvent>
    {
        protected abstract int GetY();

        public int CompareTo(FortuneEvent other)
        {
            return GetY().CompareTo(other.GetY());
        }
    }
}
