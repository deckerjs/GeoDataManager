using System;
using System.Collections.Generic;

/// <summary>
/// Origin: https://github.com/jamiewest/XamarinHosting
/// </summary>
namespace Microsoft.Extensions.Hosting
{
    public class LifecycleRegister : ILifecycleRegister
    {
        private readonly HashSet<Action> _callbacks = new HashSet<Action>();

        public void Register(Action callback)
        {
            _callbacks.Add(callback);
        }

        public void Notify()
        {
            foreach (var callback in _callbacks)
            {
                callback.Invoke();
            }
        }
    }
}
