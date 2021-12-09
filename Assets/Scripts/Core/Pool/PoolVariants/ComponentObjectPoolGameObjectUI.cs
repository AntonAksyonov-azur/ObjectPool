using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.Pool.PoolVariants
{
    public sealed class ComponentObjectPoolGameObjectUI<T> : ObjectPool<T> where T : Component
    { 
        public ComponentObjectPoolGameObjectUI(T prefab, RectTransform root) : base(() => Create(prefab, root), ActivateComponent, DeactivateComponent, DestroyComponent)
        {
        }

        private static T Create(T prefab, RectTransform root)
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