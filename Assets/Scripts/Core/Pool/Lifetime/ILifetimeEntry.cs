namespace Core.Pool.Lifetime
{
    public interface ILifetimeEntry<out T>
    {
        bool IsExpired(float compareTs);
        void Touch(float newTs);
        
        T Get();
    }
}