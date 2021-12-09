using UnityEngine;

namespace Core.Pool.Controllers
{
    public class PoolControllerGameObject<T> : IObjectPoolController<T> where T : Component
    {
        private readonly Transform _root;
        private readonly T _prefab;
        
        private int _internalCounter;
        
        public PoolControllerGameObject(T prefab, Transform root)
        {
            _prefab = prefab;
            _root = root;
        }

        public void Activate(T component)
        {
            component.gameObject.SetActive(true);
        }

        public void Deactivate(T component)
        {
            component.gameObject.SetActive(false);
        }

        public  void DestroyElement(T element)
        {
            Object.Destroy(element.gameObject);
        }

        public T Create()
        {
            _internalCounter += 1;
            
            var instance = Object.Instantiate(_prefab, _root);
            instance.name = $"PoolObject<{_prefab.name}_{_internalCounter}>";

            return instance;
        }
    }
}