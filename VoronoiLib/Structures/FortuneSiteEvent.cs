namespace VoronoiLib.Structures
{
    internal class FortuneSiteEvent : FortuneEvent
    {
        public double X => Site.X;
        public double Y => Site.Y;
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