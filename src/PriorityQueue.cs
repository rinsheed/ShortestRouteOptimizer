
namespace ShortestPathFinder;

public class PriorityQueue<T> where T : IComparable<T>
{
    private List<T> _heap;

    public int Count { get { return _heap.Count; } }

    public PriorityQueue()
    {
        _heap = new List<T>();
    }

    public void Enqueue(T item)
    {
        _heap.Add(item);
        int i = _heap.Count - 1;
        while (i > 0)
        {
            int parent = (i - 1) / 2;
            if (_heap[parent].CompareTo(_heap[i]) <= 0)
            {
                break;
            }

            T temp = _heap[i];
            _heap[i] = _heap[parent];
            _heap[parent] = temp;
            i = parent;
        }
    }

    public T Dequeue()
    {
        T item = _heap[0];
        _heap[0] = _heap[_heap.Count - 1];
        _heap.RemoveAt(_heap.Count - 1);

        int i = 0;
        while (i < _heap.Count)
        {
            int leftChild = 2 * i + 1;
            int rightChild = 2 * i + 2;
            int minIndex = i;

            if (leftChild < _heap.Count && _heap[leftChild].CompareTo(_heap[minIndex]) < 0)
            {
                minIndex = leftChild;
            }

            if (rightChild < _heap.Count && _heap[rightChild].CompareTo(_heap[minIndex]) < 0)
            {
                minIndex = rightChild;
            }

            if (minIndex == i)
            {
                break;
            }

            T temp = _heap[i];
            _heap[i] = _heap[minIndex];
            _heap[minIndex] = temp;
            i = minIndex;
        }

        return item;
    }
}
