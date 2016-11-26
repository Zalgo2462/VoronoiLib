using System;

namespace VoronoiLib
{
    public static class ParabolaMath
    {
        public static double EvalParabola(double focusX, double focusY, double directrix, double x)
        {
            return .5*(Math.Pow(x - focusX, 2)/(focusY - directrix) + focusY + directrix);
        }

        //gives the intersect point such that parabola 1 will be on top of parabola 2 slightly before the intersect
        public static double IntersectParabolaX(double focus1X, double focus1Y, double focus2X, double focus2Y,
            double directrix)
        {
            if (focus1Y.ApproxEqual(focus2Y))
                return (focus1X + focus2X)/2;
            //admittedly this is pure voodoo.
            //there is attached documentation for this function
            var firstIntersect = (focus1X*(directrix - focus2Y) + focus2X*(focus1Y - directrix) +
                                  Math.Sqrt((directrix - focus1Y)*(directrix - focus2Y)*
                                            (Math.Pow(focus1X - focus2X, 2) + Math.Pow(focus1Y - focus2Y, 2))))/
                                 (focus1Y - focus2Y);
            return firstIntersect;
        }

        public static bool ApproxEqual(this double value1, double value2)
        {
            return Math.Abs(value1 - value2) <= double.Epsilon * 1E100;
        }

        public static bool ApproxGreaterThanOrEqualTo(this double value1, double value2)
        {
            return value1 > value2 || value1.ApproxEqual(value2);
        }

        public static bool ApproxLessThanOrEqualTo(this double value1, double value2)
        {
            return value1 < value2 || value1.ApproxEqual(value2);
        }
    }
}
