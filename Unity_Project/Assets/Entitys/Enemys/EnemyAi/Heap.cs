using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Heap<T> where T : IHeapItem<T>
{
    T[] items;

    int currentCount;

    public Heap(int maxSize)
    {
        items = new T[maxSize];
    }

    public int Count
    {
        get
        {
           return currentCount;
        }
    }

    public void Add(T item)
    {
        item.HeapIndex = currentCount;
        items[currentCount] = item;

        SortUp(item);

        currentCount++;

    }

    public bool Contains(T item)
    {
        return Equals(items[item.HeapIndex], item);
    }

    public T RemoveFirst()
    {
        T firstItem = items[0];
        currentCount--;

        items[0] = items[currentCount];
        items[0].HeapIndex = 0;

        SortDown(items[0]);

        return firstItem;
    }

    void SortUp(T item)
    {
        int parentIndex = (item.HeapIndex - 1) / 2;

        while (item.CompareTo(items[parentIndex]) > 0)
        {
            Swap(item, items[parentIndex]);

            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    void SortDown(T item)
    {
        while(true)
        {
            int indexLeftChild = item.HeapIndex * 2 + 1;
            int indexRightChild = item.HeapIndex * 2 + 2;

            int swapIndex = 0;

            if (indexLeftChild < currentCount)
            {
                swapIndex = indexLeftChild;

                if (indexRightChild < currentCount)
                {
                    if (items[indexLeftChild].CompareTo(items[indexRightChild]) < 0)
                    {
                        swapIndex = indexRightChild;
                    }
                }

                if (item.CompareTo(items[swapIndex]) < 0)
                    Swap(item, items[swapIndex]);
                else return;
            }
            else return;
        }


    }

    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    void Swap(T item1, T item2)
    {
        items[item1.HeapIndex] = item2;
        items[item2.HeapIndex] = item1;

        int auxIndex = item1.HeapIndex;
        item1.HeapIndex = item2.HeapIndex;
        item2.HeapIndex = auxIndex;
    }

    public void Clear()
    {
        while (currentCount != 0)
        {
            items[currentCount] = default(T);
            currentCount--;
        }

        items[0] = default(T);

    }



}
public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex
    {
        get;
        set;
    }
}
