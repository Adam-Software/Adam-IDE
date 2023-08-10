namespace AdamControllerLibrary.ExtendedComponentModel
{
    public class ItemChangedEventArgs<T>
    {
        public T ChangedItem { get; }
        public string PropertyName { get; }

        public ItemChangedEventArgs(T item, string propertyName)
        {
            ChangedItem = item;
            PropertyName = propertyName;
        }
    }
}
