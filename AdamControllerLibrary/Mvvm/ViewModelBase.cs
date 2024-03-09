

using Prism.Mvvm;
using Prism.Navigation;

namespace AdamController.Core.Mvvm
{
    public class ViewModelBase : BindableBase, IDestructible
    {
        protected ViewModelBase() { }
        public void Destroy() { }
    }
}
