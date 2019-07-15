using System;
using System.Collections.Generic;

namespace VoronoiLib.Structures
{
    public class VCell
    {
        public VPoint Center;
        private List<VPoint> _points;

        internal VCell(VPoint center)
        {
            // the points are ordered based on their relative position to the center
            _points = new List<VPoint>();
            Center = center;
        }

        public List<VPoint> Points

        {
            get
            {
                // it would probably be better to sort these as they are added to improve performance
                _points.Sort(new Comparison<VPoint>(SortCornersClockwise));

                return _points;
            }
        }

        public void AddPoint(VPoint point)
        {
            // add point if it is not null or already in list
            if (point == null)
                return;

            if (!_points.Contains(point))
            {
                _points.Add(point);
            }
        }

        public bool Contains(VPoint testPoint)
        {
            // helper method to determine if a point is inside the cell
            // based on meowNET's answer from: https://stackoverflow.com/questions/4243042/c-sharp-point-in-polygon
            bool result = false;
            int j = Points.Count - 1;
            for (int i = 0; i < Points.Count; i++)
            {
                if (Points[i].Y < testPoint.Y && Points[j].Y >= testPoint.Y || Points[j].Y < testPoint.Y && Points[i].Y >= testPoint.Y)
                {
                    if (Points[i].X + ((testPoint.Y - Points[i].Y) / (Points[j].Y - Points[i].Y) * (Points[j].X - Points[i].X)) < testPoint.X)
                    {
                        result = !result;
                    }
                }
                j = i;
            }
            return result;
        }

        public int SortCornersClockwise(VPoint A, VPoint B)
        {
            // based on: https://social.msdn.microsoft.com/Forums/en-US/c4c0ce02-bbd0-46e7-aaa0-df85a3408c61/sorting-list-of-xy-coordinates-clockwise-sort-works-if-list-is-unsorted-but-fails-if-list-is?forum=csharplanguage

            // comparer to sort the array based on the points relative position to the center
            var atanA = Math.Atan2(A.Y - Center.Y, A.X - Center.X);
            var atanB = Math.Atan2(B.Y - Center.Y, B.X - Center.X);

            if (atanA < atanB) return -1;
            else if (atanA > atanB) return 1;
            return 0;
        }

        internal void AddEdge(VEdge value)
        {
            // overload to add both points of an edge
            AddPoint(value.Start);
            AddPoint(value.End);
        }
    }
}