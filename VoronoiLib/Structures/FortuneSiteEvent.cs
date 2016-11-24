namespace VoronoiLib.Structures
{
    internal class FortuneSiteEvent : FortuneEvent
    {
        public int X => Site.X;
        public int Y => Site.Y;
        internal FortuneSite Site { get; }

        internal FortuneSiteEvent(FortuneSite site)
        {
            Site = site;
        }

        public int CompareTo(FortuneEvent other)
        {
            return Y.CompareTo(other.Y);
        }
     
    }
}