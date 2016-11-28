using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoronoiLib.Structures;

namespace VoronoiLib
{
    public static class FortunesAlgorithm
    {
        public static List<VEdge> Run(List<FortuneSite> sites, double minX, double minY, double maxX, double maxY)
        {
            var eventQueue = new MinHeap<FortuneEvent>(sites.Count);
            sites.ForEach(s => eventQueue.Insert(new FortuneSiteEvent(s)));
            //init tree
            var beachLine = new BeachLine();
            var edges = new List<VEdge>();
            //init edge list
            while (eventQueue.Count != 0)
            {
                var fEvent = eventQueue.Pop();
                if (fEvent is FortuneSiteEvent)
                    beachLine.AddBeachSection((FortuneSiteEvent) fEvent, eventQueue, edges);
                else
                    beachLine.RemoveBeachSection((FortuneCircleEvent) fEvent, eventQueue, edges);
            }
            foreach (var edge in edges)
            {
                if (edge.End == null)
                    clipEdge(edge, minX, minY, maxX, maxY);
                if (edge.Neighbor != null && edge.Neighbor.End == null)
                    clipEdge(edge.Neighbor, minX, minY, maxX, maxY);
            }
            
            foreach (var edge in edges)
            {
                if (edge.Neighbor != null)
                {
                    edge.Start = edge.Neighbor.End;
                    edge.Neighbor = null;
                }
            }
            
            return edges;
        }

        //edge clipping functions
        private static void clipEdge(VEdge edge, double minX, double minY, double maxX, double maxY)
        {

            //there is probably a cleaner way to write this...
            if (edge.SlopeRise.ApproxEqual(0))
            {
                //horizontal line y stays the same
                if (edge.SlopeRun > 0)
                    edge.End = new VPoint(maxX, edge.Start.Y);
                else
                    edge.End = new VPoint(minX, edge.Start.Y);
            }
            else if (edge.SlopeRun.ApproxEqual(0))
            {
                //vertical line x stays the same
                if (edge.SlopeRise > 0)
                    edge.End = new VPoint(edge.Start.X, maxY);
                else 
                    edge.End = new VPoint(edge.Start.X, minY);
            }
            else if (edge.SlopeRise > 0 && edge.SlopeRun > 0)
            {
                //will hit top or right
                bool belowCorner = getAngleSign(edge.Start.X, edge.Start.Y, edge.Start.X + edge.SlopeRun, edge.Start.Y + edge.SlopeRise, maxX, maxY);
                if (belowCorner)
                    edge.End = new VPoint(maxX, calcY(edge.Slope.Value, maxX, edge.Intercept.Value));
                else
                    edge.End = new VPoint(calcX(edge.Slope.Value, maxY, edge.Intercept.Value), maxY);
            }
            else if (edge.SlopeRise < 0 && edge.SlopeRun < 0)
            {
                //will hit bottom or left
                bool aboveCorner = getAngleSign(edge.Start.X, edge.Start.Y, edge.Start.X + edge.SlopeRun, edge.Start.Y + edge.SlopeRise, minX, minY);
                if (aboveCorner)
                    edge.End = new VPoint(minX, calcY(edge.Slope.Value, minX, edge.Intercept.Value));
                else
                    edge.End = new VPoint(calcX(edge.Slope.Value, minY, edge.Intercept.Value), minY);
            }
            else if (edge.SlopeRise > 0 && edge.SlopeRun < 0)
            {
                //will hit top or left
                bool aboveCorner = getAngleSign(edge.Start.X, edge.Start.Y, edge.Start.X + edge.SlopeRun, edge.Start.Y + edge.SlopeRise, minX, maxY);
                if (aboveCorner)
                    edge.End = new VPoint(calcX(edge.Slope.Value, maxY, edge.Intercept.Value), maxY);
                else
                    edge.End = new VPoint(minX, calcY(edge.Slope.Value, minX, edge.Intercept.Value));
            }
            else if (edge.SlopeRise < 0 && edge.SlopeRun > 0)
            {
                //will hit bottom or right
                bool belowCorner = getAngleSign(edge.Start.X, edge.Start.Y, edge.Start.X + edge.SlopeRun, edge.Start.Y + edge.SlopeRise, maxX, minY);
                if (belowCorner)
                    edge.End = new VPoint(calcX(edge.Slope.Value, minY, edge.Intercept.Value), minY);
                else
                    edge.End = new VPoint(maxX, calcY(edge.Slope.Value, maxX, edge.Intercept.Value));
            }
            //should never happen
            else
            {
                throw new Exception("Error clipping voronoi edge");
            }
        }

        //true for positive
        private static bool getAngleSign(double commonX, double commonY, double x1, double y1, double x2, double y2)
        {
            return ((x1 - commonX)*(y2 - commonY) - (x2 - commonX)*(y1 - commonY)).ApproxGreaterThanOrEqualTo(0);
        }

        private static double calcY(double m, double x, double b)
        {
            return m*x + b;
        }

        private static double calcX(double m, double y, double b)
        {
            return (y - b)/m;
        }
        
    }
}
