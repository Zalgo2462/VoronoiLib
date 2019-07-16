using System;
using System.Collections.Generic;

namespace VoronoiLib.Structures
{
    public class FortuneSite
    {
        private List<VPoint> _points;

        public FortuneSite(double x, double y)
        {
            X = x;
            Y = y;
            Cell = new List<VEdge>();
            Neighbors = new List<FortuneSite>();
        }

        public List<VEdge> Cell { get; }
        public List<FortuneSite> Neighbors { get; private set; }
        public List<VPoint> Points

        {
            get
            {
                if (_points == null)
                {
                    // it would probably be better to sort these as they are added to improve performance
                    _points = new List<VPoint>();

                    foreach (var edge in Cell)
                    {
                        _points.Add(edge.Start);
                        _points.Add(edge.End);
                    }
                    _points.Sort(new Comparison<VPoint>(SortCornersClockwise));
                }

                return _points;
            }
        }

        public double X { get; }

        public double Y { get; }

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
            var atanA = Math.Atan2(A.Y - Y, A.X - X);
            var atanB = Math.Atan2(B.Y - Y, B.X - X);

            if (atanA < atanB) return -1;
            else if (atanA > atanB) return 1;
            return 0;
        }

        internal void AddEdge(VEdge value)
        {
            if (value.Start == null || value.End == null
                || double.IsNaN(value.Start.X) || double.IsNaN(value.Start.Y)
                || double.IsNaN(value.End.X) || double.IsNaN(value.End.Y))
            {
                return;
            }

            Cell.Add(value);
            _points = null;
        }
    }
}