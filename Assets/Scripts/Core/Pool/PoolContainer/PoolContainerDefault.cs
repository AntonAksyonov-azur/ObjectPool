using System.Collections.Generic;
using System.Linq;

namespace Core.Pool.PoolContainer
{
    public class PoolContainerDefault<T> : IPoolContainer<T>
    {
        private readonly Queue<T> _free = new Queue<T>();
        private readonly HashSet<T> _used = new HashSet<T>();

        public void AddToFree(T element)
        {
            _free.Enqueue(element);
        }

        public void RemoveFree(T element)
        {
            var list = _free.ToList();
            list.Remove(element);
            
            _free.Clear();
            
            foreach (var item in list)
            {
                _free.Enqueue(item);    
            }
        }

        public T GetFree()
        {
            return _free.Dequeue();
        }

        public IEnumerator<T> EnumerateFree()
        {
            return _free.GetEnumerator();
        }

        public void ClearFree()
        {
            _free.Clear();
        }

        public void AddToUsed(T element)
        {
            _used.Add(element);
        }

        public void RemoveUsed(T element)
        {
            _used.Remove(element);
        }

        public IEnumerator<T> EnumerateActive()
        {
            return _used.GetEnumerator();
        }

        public void ClearUsed()
        {
            _used.Clear();
        }

        public T[] CopyAllActive()
        {
            var array = _used.ToArray();
            return array;
        }

        public int FreeCount => _free.Count;
        public int UsedCount => _used.Count;
    }
}