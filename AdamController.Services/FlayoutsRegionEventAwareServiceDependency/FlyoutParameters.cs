using System.Collections;
using System.Collections.Generic;

namespace AdamController.Services.FlayoutsRegionEventAwareServiceDependency
{
    public class FlyoutParameters : IEnumerable
    {
        IDictionary<string, object> parameters;

        public FlyoutParameters()
        {
            parameters = new Dictionary<string, object>();
        }

        public void AddParameter(string name, object payload)
        {
            parameters.Add(name, payload);
        }

        public object this[string key]
        {
            get { return parameters[key]; }
            set { parameters[key] = value; }
        }

        public bool ContainsKey(string key)
        {
            return parameters.ContainsKey(key);
        }

        public IEnumerator GetEnumerator()
        {
            return parameters.GetEnumerator();
        }
    }
}
