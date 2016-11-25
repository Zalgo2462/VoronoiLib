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
            var fTree = new FortuneTree();
            var edges = new List<VEdge>();
            //init edge list
            while (eventQueue.Count != 0)
            {
                var fEvent = eventQueue.Pop();
                if (fEvent is FortuneSiteEvent)
                    HandleSiteEvent((FortuneSiteEvent) fEvent, eventQueue, fTree, edges);
                else
                    HandleCircleEvent((FortuneCircleEvent) fEvent, eventQueue, fTree, edges);
            }
        }

        private static void HandleSiteEvent(FortuneSiteEvent fEvent, MinHeap<FortuneEvent> eventQueue, FortuneTree fTree,
            List<VEdge> edges)
        {
            
        }

        private static void HandleCircleEvent(FortuneCircleEvent fEvent, MinHeap<FortuneEvent> eventQueue, FortuneTree fTree,
            object edges)
        {
            var leaf = fEvent.ToDelete;
        }
    }
}
