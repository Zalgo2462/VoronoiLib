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
        public static List<VEdge> Run(List<FortuneSite> sites)
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
            return edges;
        }
        
    }
}
