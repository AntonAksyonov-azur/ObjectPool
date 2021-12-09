using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.Pool.PoolVariants
{
    public sealed class ComponentObjectPoolGameObject<T> : ObjectPool<T> where T : Component
    { 
        public ComponentObjectPoolGameObject(T prefab, Transform root) : base(() => Create(prefab, root), ActivateComponent, DeactivateComponent, DestroyComponent)
        {
        }

        private static T Create(T prefab, Transform root)
        {
            var instance = Object.Instantiate(prefab, root);
            return instance;
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
    }
}