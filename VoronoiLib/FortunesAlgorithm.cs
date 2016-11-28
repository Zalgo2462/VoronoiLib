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
        public static LinkedList<VEdge> Run(List<FortuneSite> sites, double minX, double minY, double maxX, double maxY)
        {
            var eventQueue = new MinHeap<FortuneEvent>(5* sites.Count);
            sites.ForEach(s => eventQueue.Insert(new FortuneSiteEvent(s)));
            //init tree
            var beachLine = new BeachLine();
            var edges = new LinkedList<VEdge>();
            var deleted = new HashSet<FortuneCircleEvent>();

            //init edge list
            while (eventQueue.Count != 0)
            {
                var fEvent = eventQueue.Pop();
                if (fEvent is FortuneSiteEvent)
                    beachLine.AddBeachSection((FortuneSiteEvent) fEvent, eventQueue, deleted, edges);
                else
                {
                    if (deleted.Contains((FortuneCircleEvent) fEvent))
                    {
                        deleted.Remove((FortuneCircleEvent) fEvent);
                    }
                    else
                    {
                        //previously processed is being added to deleted...
                        beachLine.RemoveBeachSection((FortuneCircleEvent) fEvent, eventQueue, deleted, edges);
                    }
                }
            }

            var edgeNode = edges.First;
            while (edgeNode != null)
            {
                var edge = edgeNode.Value;
                var next = edgeNode.Next;
                if (!InBounds(edge.Start, minX, minY, maxX, maxY) &&
                    (edge.End == null || !InBounds(edge.End, minX, minY, maxX, maxY)))
                {
                    edges.Remove(edgeNode);
                }
                else
                {
                    ClipEdgeEnd(edge, minX, minY, maxX, maxY);
                    if (edge.Neighbor != null)
                    {
                        ClipEdgeEnd(edge.Neighbor, minX, minY, maxX, maxY);
                        edge.Start = edge.Neighbor.End;
                        edge.Neighbor = null;
                    }
                    else
                    {
                        ClipEdgeStart(edge, minX, minY, maxX, maxY);
                    }
                }
                edgeNode = next;
            }
            

            return edges;
        }

        private static bool InBounds(VPoint point, double minX, double minY, double maxX, double maxY)
        {
            return point.X.ApproxGreaterThanOrEqualTo(minX) && point.Y.ApproxGreaterThanOrEqualTo(minY) &&
                point.X.ApproxLessThanOrEqualTo(maxX) && point.Y.ApproxLessThanOrEqualTo(maxY);    
        }

        //edge clipping functions
        private static void ClipEdgeStart(VEdge edge, double minX, double minY, double maxX, double maxY)
        {
            if (edge.Start != null && edge.Start.X.ApproxGreaterThanOrEqualTo(minX) &&
                edge.Start.X.ApproxLessThanOrEqualTo(maxX) &&
                edge.Start.Y.ApproxGreaterThanOrEqualTo(minX) &&
                edge.Start.Y.ApproxLessThanOrEqualTo(maxY))
                return;

            //there is probably a cleaner way to write this...
            if (edge.SlopeRise.ApproxEqual(0))
            {
                //horizontal line y stays the same
                if (edge.SlopeRun > 0)
                    edge.Start = new VPoint(minX, edge.Start.Y);
                else
                    edge.Start = new VPoint(maxX, edge.Start.Y);
            }
            else if (edge.SlopeRun.ApproxEqual(0))
            {
                //vertical line x stays the same
                if (edge.SlopeRise > 0)
                    edge.Start = new VPoint(edge.Start.X, minY);
                else
                    edge.Start = new VPoint(edge.Start.X, maxY);
            }
            else if (edge.SlopeRise > 0 && edge.SlopeRun > 0)
            {
                //will hit bottom or left
                bool belowCorner = GetAngleSign(edge.Start.X, edge.Start.Y, edge.Start.X + edge.SlopeRun,
                    edge.Start.Y + edge.SlopeRise, minX, minY);
                if (belowCorner)
                    edge.Start = new VPoint(calcX(edge.Slope.Value, minY, edge.Intercept.Value), minY);
                else
                    edge.Start = new VPoint(minX, CalcY(edge.Slope.Value, minX, edge.Intercept.Value));
            }
            else if (edge.SlopeRise < 0 && edge.SlopeRun < 0)
            {
                //will hit top or right
                bool aboveCorner = GetAngleSign(edge.Start.X, edge.Start.Y, edge.Start.X + edge.SlopeRun, edge.Start.Y + edge.SlopeRise, maxX, maxY);
                if (aboveCorner)
                    edge.Start = new VPoint(calcX(edge.Slope.Value, maxY, edge.Intercept.Value), maxY);
                else
                    edge.Start = new VPoint(maxX, CalcY(edge.Slope.Value, maxX, edge.Intercept.Value));
            }
            else if (edge.SlopeRise > 0 && edge.SlopeRun < 0)
            {
                //will hit bottom or right
                bool aboveCorner = GetAngleSign(edge.Start.X, edge.Start.Y, edge.Start.X + edge.SlopeRun, edge.Start.Y + edge.SlopeRise, maxX, minY);
                if (aboveCorner)
                    edge.Start = new VPoint(maxX, CalcY(edge.Slope.Value, maxX, edge.Intercept.Value));
                else
                    edge.Start = new VPoint(calcX(edge.Slope.Value, minY, edge.Intercept.Value), minY);
            }
            else if (edge.SlopeRise < 0 && edge.SlopeRun > 0)
            {
                //will hit top or left
                bool belowCorner = GetAngleSign(edge.Start.X, edge.Start.Y, edge.Start.X + edge.SlopeRun, edge.Start.Y + edge.SlopeRise, minX, maxY);
                if (belowCorner)
                    edge.Start = new VPoint(minX, CalcY(edge.Slope.Value, minX, edge.Intercept.Value));
                else
                    edge.Start = new VPoint(calcX(edge.Slope.Value, maxY, edge.Intercept.Value), maxY);
            }
            //should never happen
            else
            {
                throw new Exception("Error clipping voronoi edge");
            }
        }

        private static void ClipEdgeEnd(VEdge edge, double minX, double minY, double maxX, double maxY)
        {
            if (edge.End != null && 
                edge.End.X.ApproxGreaterThanOrEqualTo(minX) &&
                edge.End.X.ApproxLessThanOrEqualTo(maxX) &&
                edge.End.Y.ApproxGreaterThanOrEqualTo(minX) &&
                edge.End.Y.ApproxLessThanOrEqualTo(maxY))
                return;

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
                bool belowCorner = GetAngleSign(edge.Start.X, edge.Start.Y, edge.Start.X + edge.SlopeRun, edge.Start.Y + edge.SlopeRise, maxX, maxY);
                if (belowCorner)
                    edge.End = new VPoint(maxX, CalcY(edge.Slope.Value, maxX, edge.Intercept.Value));
                else
                    edge.End = new VPoint(calcX(edge.Slope.Value, maxY, edge.Intercept.Value), maxY);
            }
            else if (edge.SlopeRise < 0 && edge.SlopeRun < 0)
            {
                //will hit bottom or left
                bool aboveCorner = GetAngleSign(edge.Start.X, edge.Start.Y, edge.Start.X + edge.SlopeRun, edge.Start.Y + edge.SlopeRise, minX, minY);
                if (aboveCorner)
                    edge.End = new VPoint(minX, CalcY(edge.Slope.Value, minX, edge.Intercept.Value));
                else
                    edge.End = new VPoint(calcX(edge.Slope.Value, minY, edge.Intercept.Value), minY);
            }
            else if (edge.SlopeRise > 0 && edge.SlopeRun < 0)
            {
                //will hit top or left
                bool aboveCorner = GetAngleSign(edge.Start.X, edge.Start.Y, edge.Start.X + edge.SlopeRun, edge.Start.Y + edge.SlopeRise, minX, maxY);
                if (aboveCorner)
                    edge.End = new VPoint(calcX(edge.Slope.Value, maxY, edge.Intercept.Value), maxY);
                else
                    edge.End = new VPoint(minX, CalcY(edge.Slope.Value, minX, edge.Intercept.Value));
            }
            else if (edge.SlopeRise < 0 && edge.SlopeRun > 0)
            {
                //will hit bottom or right
                bool belowCorner = GetAngleSign(edge.Start.X, edge.Start.Y, edge.Start.X + edge.SlopeRun, edge.Start.Y + edge.SlopeRise, maxX, minY);
                if (belowCorner)
                    edge.End = new VPoint(calcX(edge.Slope.Value, minY, edge.Intercept.Value), minY);
                else
                    edge.End = new VPoint(maxX, CalcY(edge.Slope.Value, maxX, edge.Intercept.Value));
            }
            //should never happen
            else
            {
                throw new Exception("Error clipping voronoi edge");
            }
        }

        //true for positive
        private static bool GetAngleSign(double commonX, double commonY, double x1, double y1, double x2, double y2)
        {
            return ((x1 - commonX)*(y2 - commonY) - (x2 - commonX)*(y1 - commonY)).ApproxGreaterThanOrEqualTo(0);
        }

        private static double CalcY(double m, double x, double b)
        {
            return m*x + b;
        }

        private static double calcX(double m, double y, double b)
        {
            return (y - b)/m;
        }
        
    }
}
