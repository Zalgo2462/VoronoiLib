using System;

namespace VoronoiLib.Structures
{
    public class MinHeap<T> where T : IComparable<T>
    {
        private readonly T[] items;
        public int Capacity { get; }
        public int Count { get; private set; }

        public MinHeap(int capacity)
        {
            if (capacity < 2)
            {
                capacity = 2;
            }
            Capacity = capacity;
            items = new T[Capacity];
            Count = 0;
        }

        public bool Insert(T obj)
        {
            if (Count == Capacity)
                return false;
            items[Count] = obj;
            Count++;
            PercolateUp(Count - 1);
            return true;
        }

        public T Pop()
        {
            if (Count == 0)
                throw new InvalidOperationException("Min heap is empty");
            if (Count == 1)
            {
                Count--;
                return items[Count];
            }

            var min = items[0];
            items[0] = items[Count - 1];
            Count--;
            PercolateDown(0);
            return min;
        }

        public T Peek()
        {
            if (Count == 0)
                throw new InvalidOperationException("Min heap is empty");
            return items[0];
        }

        private void PercolateDown(int index)
        {
            var left = 2*index + 1;
            var right = 2*index + 2;
            var largest = index;

            if (left < Count && LeftLessThanRight(left, largest))
                largest = left;
            if (right < Count && LeftLessThanRight(right, largest))
                largest = right;
            if (largest == index)
                return;
            Swap(index, largest);
            PercolateDown(largest);
        }

        private void PercolateUp(int index)
        {
            if (index >= Count || index <= 0)
                return;
            var parent = (index - 1)/2;

            if (LeftLessThanRight(parent, index))
                return;

            Swap(index, parent);
            PercolateUp(parent);
        }

        private bool LeftLessThanRight(int left, int right)
        {
            return items[left].CompareTo(items[right]) < 0;
        }

        private void Swap(int left, int right)
        {
            var temp = items[left];
            items[left] = items[right];
            items[right] = temp;
        }
    }
}
