using AdamController.Services.Interfaces;
using Prism.Mvvm;
using Prism.Navigation;

namespace AdamController.Core.Mvvm
{
    public class ViewModelBase : BindableBase, IDestructible
    {
        public event SubRegionChangeEventHandler RaiseRegionChangeEvent;

        protected ViewModelBase() 
        { 

        }

        public void Destroy() 
        { 
        }
    }
}
