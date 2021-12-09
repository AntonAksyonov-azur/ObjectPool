using System.Collections.Generic;

namespace Core.Pool.PoolContainer
{
    public interface IPoolContainer<T>
    {
        void AddToFree(T element);
        void RemoveFree(T element);
        T GetFree();
        IEnumerator<T> EnumerateFree();
        void ClearFree();

        void AddToUsed(T element);
        void RemoveUsed(T element);
        IEnumerator<T> EnumerateActive();
        void ClearUsed();
        T[] CopyAllActive();

        int FreeCount { get; }
        int UsedCount { get; }
    }
}