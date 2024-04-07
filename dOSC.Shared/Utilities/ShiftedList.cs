using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dOSCEngine.Utilities
{

    public class ShiftedList<T>
    {
        private List<T> internalList = new List<T>();
        private readonly object lockObject = new object();

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

        public void Clear()
        {
            lock(lockObject)
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
                int effectivePositions = positions % internalList.Count;

                List<T> temp = new List<T>(internalList.GetRange(0, effectivePositions));
                internalList.RemoveRange(0, effectivePositions);
                internalList.AddRange(temp);
            }
        }

        public void Resize(int newLength)
        {
            lock (lockObject)
            {
                if (newLength < internalList.Count)
                {
                    internalList.RemoveRange(newLength, internalList.Count - newLength);
                }
                else
                {
                    while (internalList.Count < newLength)
                    {
                        // You can choose a default value for the new elements or leave them uninitialized.
                        internalList.Add(default(T));
                    }
                }
            }
        }

        public T this[int index]
        {
            get
            {
                lock (lockObject)
                {
                    if (index < 0 || index >= internalList.Count)
                    {
                        throw new IndexOutOfRangeException();
                    }
                    return internalList[index];
                }
            }
            set
            {
                lock (lockObject)
                {
                    if (index < 0 || index >= internalList.Count)
                    {
                        throw new IndexOutOfRangeException();
                    }
                    internalList[index] = value;
                }
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

}
