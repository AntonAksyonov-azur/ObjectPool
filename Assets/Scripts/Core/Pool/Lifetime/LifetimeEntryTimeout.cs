namespace Core.Pool.Lifetime
{
    public class LifetimeEntryTimeout<T> : ILifetimeEntry<T>
    {
        private readonly T _instance;
        private float _ts;
        
        public bool IsExpired(float compareTs)
        {
            return _ts < compareTs;
        }

        public void Touch(float newTs)
        {
            _ts = newTs;
        }

        public T Get()
        {
            return _instance;
        }

        public LifetimeEntryTimeout(T instance)
        {
            _instance = instance;
        }
    }
}