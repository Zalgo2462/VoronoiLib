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
        public static void Run(List<FortuneSite> sites)
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
                    beachLine.AddBeachSection(((FortuneSiteEvent) fEvent).Site, eventQueue, edges);
                else
                    HandleCircleEvent((FortuneCircleEvent) fEvent, eventQueue, beachLine, edges);
            }
        }

        private static void HandleCircleEvent(FortuneCircleEvent fEvent, MinHeap<FortuneEvent> eventQueue, BeachLine beachLine,
            object edges)
        {
            var leaf = fEvent.ToDelete;
        }
    }
}
