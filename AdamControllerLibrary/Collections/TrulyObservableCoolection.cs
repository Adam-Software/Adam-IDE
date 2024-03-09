
using AdamController.Core.ExtendedComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;


namespace AdamController.Core.Collections
{
    public sealed class TrulyObservableCollection<T> : ObservableCollection<T>, ICollectionItemPropertyChanged<T> where T : INotifyPropertyChanged
    {
        public event EventHandler<ItemChangedEventArgs<T>> ItemChanged;

        public TrulyObservableCollection()
        {
            CollectionChanged += FullObservableCollectionCollectionChanged;
        }

        public TrulyObservableCollection(IEnumerable<T> pItems) : this()
        {
            foreach (T item in pItems)
            {
                Add(item);
            }
        }

        private void FullObservableCollectionCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (object item in e.NewItems)
                {
                    (item as INotifyPropertyChanged).PropertyChanged += ItemPropertyChanged;
                }
            }
            if (e.OldItems != null)
            {
                foreach (object item in e.OldItems)
                {
                    (item as INotifyPropertyChanged).PropertyChanged -= ItemPropertyChanged;
                }
            }
        }

        private void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ItemChangedEventArgs<T> args = new((T)sender, e.PropertyName);
            ItemChanged?.Invoke(this, args);
        }
    }

    public interface ICollectionItemPropertyChanged<T>
    {
        event EventHandler<ItemChangedEventArgs<T>> ItemChanged;
    }
}
