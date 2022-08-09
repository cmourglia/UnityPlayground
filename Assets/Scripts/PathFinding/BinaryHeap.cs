using System;

public interface IHeapElement<T> : IComparable<T> {
    int HeapIndex { get; set; }
}

public class BinaryHeap<T> where T : IHeapElement<T> {
    private readonly T[] elements;

    public BinaryHeap(int maxSize) {
        elements = new T[maxSize];
        Count = 0;
    }

    public int Count { get; private set; }

    public void Add(T element) {
        element.HeapIndex = Count;
        elements[Count] = element;

        BubbleUp(element);

        Count += 1;
    }

    public T Pop() {
        T firstElement = elements[0];
        Count -= 1;

        elements[0] = elements[Count];
        elements[0].HeapIndex = 0;

        BubbleDown(elements[0]);

        return firstElement;
    }

    private void BubbleUp(T element) {
        // Sort inserted value
        while (true) {
            int parentIndex = (element.HeapIndex - 1) / 2;
            T parentElement = elements[parentIndex];

            if (element.CompareTo(parentElement) > 0) {
                Swap(element, parentElement);
            } else {
                break;
            }
        }
    }

    private void BubbleDown(T element) {
        while (true) {
            int leftChildIndex = element.HeapIndex * 2 + 1;
            int rightChildIndex = element.HeapIndex * 2 + 2;

            if (leftChildIndex >= Count) break;

            int largestChildIndex = leftChildIndex;

            if (rightChildIndex < Count && elements[leftChildIndex].CompareTo(elements[rightChildIndex]) < 0) {
                largestChildIndex = rightChildIndex;
            }

            if (element.CompareTo(elements[largestChildIndex]) < 0) {
                Swap(element, elements[largestChildIndex]);
            } else {
                break;
            }
        }
    }

    private void Swap(T element1, T element2) {
        elements[element1.HeapIndex] = element2;
        elements[element2.HeapIndex] = element1;
        int element1Index = element1.HeapIndex;
        element1.HeapIndex = element2.HeapIndex;
        element2.HeapIndex = element1Index;
    }
}
