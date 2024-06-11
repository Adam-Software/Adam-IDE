using System;
using System.Collections.Generic;

namespace AdamStudio.Services.UdpClientServiceDependency
{
    public class QueueWithEvent<T>
    {
        public event EventHandler RaiseEnqueueEvent;

        private readonly Queue<T> queue = new();

        public int Count { get { return queue.Count; } }

        public virtual void Enqueue(T item)
        {
            queue.Enqueue(item);
            OnRaiseEnqueueEvent();
        }

       
        public virtual T Dequeue()
        {
            T item = queue.Dequeue();
            return item;
        }

        protected virtual void OnRaiseEnqueueEvent()
        {
            EventHandler raiseEvent = RaiseEnqueueEvent;
            raiseEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}
