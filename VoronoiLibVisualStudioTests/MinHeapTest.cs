using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VoronoiLib.Structures;

namespace VoronoiLibTests
{
    [TestClass]
    public class MinHeapTest
    {
        [TestMethod]
        public void Sort5Test()
        {
            var heap = new MinHeap<int>(5);
            heap.Insert(5);
            heap.Insert(4);
            heap.Insert(3);
            heap.Insert(2);
            heap.Insert(1);
            Assert.AreEqual(1, heap.Pop());
            Assert.AreEqual(2, heap.Pop());
            Assert.AreEqual(3, heap.Pop());
            Assert.AreEqual(4, heap.Pop());
            Assert.AreEqual(5, heap.Pop());
        }

        [TestMethod]
        public void DeleteTest()
        {
            var heap = new MinHeap<int>(5);
            for (int i = 1; i <= 5; i++)
            {
                //insert 5 through 1
                for (int j = 5; j >= 1; j--)
                {
                    heap.Insert(j);
                }
                //remove i
                heap.Remove(i);
                //the order of pops should be sorted without i
                for (int j = 1; j <= 5; j++)
                {
                    if (j != i)
                        Assert.AreEqual(j, heap.Pop());
                }
            }
        }

        [TestMethod]
        public void SortRandom()
        {
            var numbers = new List<double>();
            var random = new Random();
            const int size = 10000;
            var heap = new MinHeap<double>(size);
            for (int i = 0; i < size; i++)
            {
                var number = 100*random.NextDouble();
                numbers.Add(number);
                heap.Insert(number);
            }
            numbers.Sort();
            foreach (var number in numbers)
            {
                Assert.AreEqual(heap.Pop(), number);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PopEmpty()
        {
            var heap = new MinHeap<int>(10);
            heap.Pop();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PeekEmpty()
        {
            var heap = new MinHeap<int>(10);
            heap.Peek();
        }
    }
}
