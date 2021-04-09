using Microsoft.VisualStudio.TestTools.UnitTesting;
using VoronoiLib;

namespace VoronoiLibTests
{
    [TestClass]
    public class ParabolaTest
    {
        private const double EPSILON = double.Epsilon * 1E100;
        [TestMethod]
        public void TestEvalXAtFZero()
        {
            int fX = 0;
            int fY = 0;
            int directrix = 2;
            Assert.AreEqual(ParabolaMath.EvalParabola(fX, fY, directrix, 0), 1, EPSILON);
            Assert.AreEqual(ParabolaMath.EvalParabola(fX, fY, directrix, 10), -24, EPSILON);
            Assert.AreEqual(ParabolaMath.EvalParabola(fX, fY, directrix, 10), ParabolaMath.EvalParabola(fX, fY, directrix, -10), EPSILON);
        }
        [TestMethod]
        public void TestEvalXAtFOne()
        {
            int fX = 1;
            int fY = 1;
            int directrix = 3;
            Assert.AreEqual(ParabolaMath.EvalParabola(fX, fY, directrix, 1), 2, EPSILON);
            Assert.AreEqual(ParabolaMath.EvalParabola(fX, fY, directrix, 15), -47, EPSILON);
            Assert.AreEqual(ParabolaMath.EvalParabola(fX, fY, directrix, 15), ParabolaMath.EvalParabola(fX, fY, directrix, -13), EPSILON);
        }
        [TestMethod]
        public void TestEvalXAt123()
        {
            int fX = 1;
            int fY = 2;
            int directrix = 3;
            Assert.AreEqual(5.0 / 2, ParabolaMath.EvalParabola(fX, fY, directrix, 1), EPSILON);
            Assert.AreEqual(-95.0 / 2, ParabolaMath.EvalParabola(fX, fY, directrix, 11), EPSILON);
            Assert.AreEqual(ParabolaMath.EvalParabola(fX, fY, directrix, -9), ParabolaMath.EvalParabola(fX, fY, directrix, 11), EPSILON);
        }

        [TestMethod]
        public void TestCollinearIntersect()
        {
            int fX1 = 0;
            int fY1 = 0;
            int fX2 = 5;
            int fY2 = 0;
            int directrix = 5;
            Assert.AreEqual(5.0 / 2, ParabolaMath.IntersectParabolaX(fX1,fY1,fX2,fY2,directrix), EPSILON);
        }

        [TestMethod]
        public void TestIntersect()
        {
            int fX1 = 1;
            int fY1 = 2;
            int fX2 = 5;
            int fY2 = 4;
            int directrix = 14;
            Assert.AreEqual(.50510257, ParabolaMath.IntersectParabolaX(fX1, fY1, fX2, fY2, directrix), .00000001);
            Assert.AreEqual(49.49489743, ParabolaMath.IntersectParabolaX(fX2, fY2, fX1, fY1, directrix), .00000001);
        }
    }
}
