using UnityEngine;

namespace Core.Pool.Controllers
{
    public class PoolControllerGameObjectSimple : IObjectPoolController<GameObject>
    {
        private readonly Transform _root;
        private readonly GameObject _prefab;
        
        private int _internalCounter;
        
        public PoolControllerGameObjectSimple(GameObject prefab, Transform root)
        {
            _prefab = prefab;
            _root = root;
        }

        public void Activate(GameObject component)
        {
            component.gameObject.SetActive(true);
        }

        public void Deactivate(GameObject component)
        {
            component.gameObject.SetActive(false);
        }

        public  void DestroyElement(GameObject element)
        {
            Object.Destroy(element.gameObject);
        }

        public GameObject Create()
        {
            _internalCounter += 1;
            
            var instance = Object.Instantiate(_prefab, _root);
            instance.name = $"PoolObject<GameObject_{_internalCounter}>";

            return instance;
        }
    }
}