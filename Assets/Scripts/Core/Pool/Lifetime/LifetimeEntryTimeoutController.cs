using Core.Pool.Controllers;

namespace Core.Pool.Lifetime
{
    public class LifetimeEntryTimeoutController<T> : IObjectPoolController<LifetimeEntryTimeout<T>>
    {
        private readonly IObjectPoolController<T> _controller;

        public LifetimeEntryTimeoutController(IObjectPoolController<T> controller)
        {
            _controller = controller;
        }

        public LifetimeEntryTimeout<T> Create()
        {
            var entry = new LifetimeEntryTimeout<T>(_controller.Create());
            return entry;
        }

        public void Activate(LifetimeEntryTimeout<T> element)
        {
            _controller.Activate(element.Get());
        }

        public void Deactivate(LifetimeEntryTimeout<T> element)
        {
            _controller.Deactivate(element.Get());
        }

        public void DestroyElement(LifetimeEntryTimeout<T> element)
        {
            _controller.DestroyElement(element.Get());
        }
    }
}