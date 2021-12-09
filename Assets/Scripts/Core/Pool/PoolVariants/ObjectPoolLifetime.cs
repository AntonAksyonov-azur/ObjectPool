using System;
using System.Collections.Generic;
using Core.Pool.Controllers;
using Core.Pool.Lifetime;
using Core.Pool.PoolContainer;

namespace Core.Pool.PoolVariants
{
    public class ObjectPoolLifetime<T, TR> where T : ILifetimeEntry<TR>
    {
        private readonly float _expectedInstancesLifetime;
        private readonly Dictionary<TR, T> _usedInstances = new Dictionary<TR, T>();
        
        private readonly ObjectPool<T> _internalPool;
        private readonly List<T> _removeList = new List<T>();
        
        public ObjectPoolLifetime(IObjectPoolController<T> controller, float expectedInstancesLifetime)
        {
            _expectedInstancesLifetime = expectedInstancesLifetime;
            
            _internalPool = new ObjectPool<T>(controller, new PoolContainerDefault<T>());
        }
        
        public void Warmup(int initialCount, float touchTs)
        {
            for (var i = 0; i < initialCount; i++)
            {
                var result = Get(touchTs);
                if (result == null)
                {
                    throw new Exception($"{nameof(ObjectPool<T>)} creation exception: constructor can't create valid instance");
                }

                Release(result);
            }
        }
        
        public void RemoveUnused(float compareTs, bool onePerCall = false)
        {
            _removeList.Clear();
            
            // Check lifetime
            var enumerable = _internalPool.EnumerateFree();
            foreach (var lifetimeEntry in enumerable)
            {
                if (lifetimeEntry.IsExpired(compareTs))
                {
                    _removeList.Add(lifetimeEntry);

                    // Для целей оптимизации можно удалять по одному элементу за раз
                    // Удаление большого количества элементов сразу может привести к фризу
                    if (onePerCall)
                    {
                        _internalPool.Destroy(lifetimeEntry);
                        return;
                    }
                }
            }

            // Destroy
            foreach (var lifetimeEntry in _removeList)
            {
                _internalPool.Destroy(lifetimeEntry);
            }
        }

        public TR Get(float touchTs)
        {
            var entry = _internalPool.Get();
            entry.Touch(touchTs + _expectedInstancesLifetime);

            var r = entry.Get();
            _usedInstances.Add(r, entry);

            return r;
        }

        public void Release(TR instance)
        {
            if (_usedInstances.TryGetValue(instance, out var entry))
            {
                _internalPool.Release(entry);
                _usedInstances.Remove(instance);
            }
        }

        public void ReleaseAll()
        {
            _usedInstances.Clear();
            _internalPool.ReleaseAll();
        }
    }
}