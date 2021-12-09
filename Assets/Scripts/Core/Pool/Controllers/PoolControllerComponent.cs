using UnityEngine;

namespace Core.Pool.Controllers
{
    public class PoolControllerComponent<T> : IObjectPoolController<T> where T : Component
    {
        private int _internalCounter;
        
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
            
            var go = new GameObject($"PoolObject<{typeof(T)}_{_internalCounter}>");
            var comp = go.AddComponent<T>();

            return comp;
        }
    }
}