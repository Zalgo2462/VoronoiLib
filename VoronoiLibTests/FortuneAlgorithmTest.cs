using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VoronoiLib;
using VoronoiLib.Structures;

namespace VoronoiLibTests
{
    [TestClass]
    public class FortuneAlgorithmTest
    {
        [TestMethod]
        public void FortuneThreePoints()
        {
            var points = new List<FortuneSite>
            {
                new FortuneSite(100, 100),
                new FortuneSite(200, 200),
                new FortuneSite(200, 150)
            };
            var edges = FortunesAlgorithm.Run(points);
            //edge 1
            Assert.AreEqual(200, edges[0].Start.X);
            Assert.AreEqual(25, edges[0].Start.Y);
            Assert.AreEqual(125, edges[0].End.X);
            Assert.AreEqual(175, edges[0].End.Y);
            Assert.IsNotNull(edges[0].Neighbor);
            Assert.AreEqual(425, edges[0].Neighbor.Intercept);

            //edge 2
            Assert.AreEqual(200, edges[1].Start.X);
            Assert.AreEqual(175, edges[1].Start.Y);
            Assert.AreEqual(125, edges[1].End.X);
            Assert.AreEqual(175, edges[1].End.Y);
            Assert.IsNotNull(edges[1].Neighbor);
            Assert.IsNull(edges[1].Neighbor.Intercept);

            //edge 3
            Assert.AreEqual(125, edges[2].Start.X);
            Assert.AreEqual(175, edges[2].Start.Y);
            Assert.AreEqual(300, edges[2].Intercept);
            Assert.IsNull(edges[2].Neighbor);
        }
    }
}
