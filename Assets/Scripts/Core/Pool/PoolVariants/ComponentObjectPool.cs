using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.Pool.PoolVariants
{
    public sealed class ComponentObjectPool<T> : ObjectPool<T> where T : Component
    {
        public ComponentObjectPool(Func<T> constructor = null, Action<T> deactivator = null) : base(constructor ?? Create, ActivateComponent, deactivator ?? DeactivateComponent, DestroyComponent)
        {
        }
        
        private static void ActivateComponent(T component)
        {
            component.gameObject.SetActive(true);
        }

        private static void DeactivateComponent(T component)
        {
            component.gameObject.SetActive(false);
        }

        private static void DestroyComponent(T element)
        {
            Object.Destroy(element.gameObject);
        }

        private static T Create()
        {
            var go = new GameObject($"PoolObject<{typeof(T)}>");
            var comp = go.AddComponent<T>();
            return comp;
        }
    }
}