using System;
using System.Collections.Generic;
using System.Linq;

namespace dOSC.Utilities;

public class ShiftedList<T>
{
    private readonly object lockObject = new();
    private readonly List<T> internalList = new();

    public int Count
    {
        get
        {
            lock (lockObject)
            {
                return internalList.Count;
            }
        }
    }

    public IEnumerable<T> this[Range range]
    {
        get
        {
            lock (lockObject)
            {
                var (offset, length) = range.GetOffsetAndLength(internalList.Count);
                if (offset < 0 || offset + length > internalList.Count) throw new IndexOutOfRangeException();
                return internalList.GetRange(offset, length);
            }
        }
        set
        {
            lock (lockObject)
            {
                var (start, length) = range.GetOffsetAndLength(internalList.Count);
                if (start < 0 || start + length > internalList.Count) throw new ArgumentException("Invalid range.");
                internalList.RemoveRange(start, length);
                internalList.InsertRange(start, value);
            }
        }
    }
    public void Clear()
    {
        lock (lockObject)
        {
            internalList.Clear();
        }
    }

    public void Add(T item)
    {
        lock (lockObject)
        {
            internalList.Add(item);
        }
    }

    public void ShiftLeft(int positions)
    {
        if (positions <= 0)
            return;

        lock (lockObject)
        {
            if (!internalList.Any())
                return;
            var effectivePositions = positions % internalList.Count;

            var temp = new List<T>(internalList.GetRange(0, effectivePositions));
            internalList.RemoveRange(0, effectivePositions);
            internalList.AddRange(temp);
        }
    }

    public void Resize(int newLength)
    {
        lock (lockObject)
        {
            if (newLength < internalList.Count)
                internalList.RemoveRange(newLength, internalList.Count - newLength);
            else
                while (internalList.Count < newLength)
                    // You can choose a default value for the new elements or leave them uninitialized.
                    internalList.Add(default);
        }
    }

    public List<T> GetCopy()
    {
        lock (lockObject)
        {
            return new List<T>(internalList);
        }
    }
}