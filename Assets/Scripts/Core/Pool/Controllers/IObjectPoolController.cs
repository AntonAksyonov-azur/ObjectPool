namespace Core.Pool.Controllers
{
    public interface IObjectPoolController<T>
    {
        T Create();
        void Activate(T element);
        void Deactivate(T element);
        void DestroyElement(T element);
    }
}