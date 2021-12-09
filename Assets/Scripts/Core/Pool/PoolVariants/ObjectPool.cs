using System;
using System.Collections.Generic;
using Core.Pool.Controllers;
using Core.Pool.PoolContainer;

namespace Core.Pool.PoolVariants
{
    public class ObjectPool<T>
    {
        private readonly IPoolContainer<T> _container;

        private readonly Func<T> _constructor;
        private readonly Action<T> _activationMethod;
        private readonly Action<T> _deactivationMethod;
        private readonly Action<T> _destroyMethod;
        
        public event Action<T> AfterItemActivated;

        #region Constructors
        
        public ObjectPool(Func<T> constructor, Action<T> activationMethod, Action<T> deactivationMethod, Action<T> destroyMethod)
        {
            _container = new PoolContainerDefault<T>();
            
            _constructor = constructor;
            
            _activationMethod = activationMethod;
            _deactivationMethod = deactivationMethod;
            
            _destroyMethod = destroyMethod;
        }

        public ObjectPool(IObjectPoolController<T> controller, IPoolContainer<T> container)
        {
            _container = container;
            
            _constructor = controller.Create;
            
            _activationMethod = controller.Activate;
            _deactivationMethod = controller.Deactivate;
            
            _destroyMethod = controller.DestroyElement;
        }
        
        #endregion

        public T Get()
        {
            var result = _container.FreeCount > 0 ? _container.GetFree() : _constructor.Invoke();
            if (result == null)
            {
                throw new Exception($"{nameof(ObjectPool<T>)} creation exception: constructor can't create valid instance");
            }

            _container.AddToUsed(result);
            _activationMethod.Invoke(result);

            AfterItemActivated?.Invoke(result);

            return result;
        }

        public void Warmup(int creationCount)
        {
            for (var i = 0; i < creationCount; i++)
            {
                Get();
            }
            
            ReleaseAll();
        }

        public T[] GetAllActive()
        {
            return _container.CopyAllActive();
        }

        public IEnumerable<T> EnumerateActive()
        {
            var enumerator = _container.EnumerateActive();
            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;    
            }
        }
        
        public IEnumerable<T> EnumerateFree()
        {
            var enumerator = _container.EnumerateFree();
            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;    
            }
        }
        
        public void Release(T item)
        {
            _container.RemoveUsed(item);
            _container.AddToFree(item);
            
            _deactivationMethod.Invoke(item);
        }

        public void ReleaseAll(bool deactivate = true)
        {
            var used = _container.EnumerateActive();
            while (used.MoveNext())
            {
                _container.AddToFree(used.Current);
                if (deactivate)
                {
                    _deactivationMethod.Invoke(used.Current);
                }
            }

            _container.ClearUsed();
        }

        public void Destroy(T item)
        {
            _container.RemoveUsed(item);
            _container.RemoveFree(item);
            
            _destroyMethod.Invoke(item);
        }
        
        public void DestroyAll()
        {
            var free = _container.EnumerateFree();
            while (free.MoveNext())
            {
                _destroyMethod.Invoke(free.Current);
            }
         
            var used = _container.EnumerateActive();
            while (used.MoveNext())
            {
                _destroyMethod.Invoke(used.Current);
            }
            
            _container.ClearFree();
            _container.ClearUsed();
        }
    }
}