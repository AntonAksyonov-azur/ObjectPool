using Core.Pool.Controllers;
using Core.Pool.PoolContainer;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.Pool.PoolVariants
{
    public sealed class GameObjectPool : ObjectPool<GameObject>
    {
        public GameObjectPool(GameObject prefab, Transform root) : base(() => Create(prefab, root), ActivateComponent, DeactivateComponent, DestroyComponent)
        {
        }

        public GameObjectPool(IObjectPoolController<GameObject> controller, IPoolContainer<GameObject> container) : base(controller, container)
        {
        }
        
        private static GameObject Create(GameObject prefab, Transform root)
        {
            var instance = Object.Instantiate(prefab, root);
            return instance;
        }

        private static void ActivateComponent(GameObject component)
        {
            component.gameObject.SetActive(true);
        }

        private static void DeactivateComponent(GameObject component)
        {
            component.gameObject.SetActive(false);
        }

        private static void DestroyComponent(GameObject element)
        {
            Object.Destroy(element);
        }
    }
}