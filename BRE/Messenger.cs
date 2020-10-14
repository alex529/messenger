using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BRE
{
    /// <summary>
    /// Uses the messenger patter to allow communication between data providers and consumers.
    /// The class implements a singleton pattern.
    /// </summary>
    public sealed class Messenger : IMessenger, IDisposable
    {
        private static Messenger _instance;
        private readonly ConcurrentDictionary<Type, List<Delegate>> _subscribers; //the delegate list should also implement concurrent container 

        /// <summary>
        /// Singleton
        /// </summary>
        public static Messenger Instance => _instance ??= new Messenger();//singleton implementation, needs to be shared with all consumers/providers
        private Messenger() => _subscribers = new ConcurrentDictionary<Type, List<Delegate>>();

        /// <inheritdoc/>
        public void Publish<T>(T message)
        {
            if (message == null)
                return;
            if (!_subscribers.ContainsKey(typeof(T)))
                return;
            if (!_subscribers.TryGetValue(typeof(T), out var delegates))
                return;
            if (delegates == null || delegates.Count == 0)
                return;

            foreach (var handler in delegates.OfType<Action<Message<T>>>())
                Task.Run(() => handler?.Invoke(new Message<T>(message)));
        }

        /// <inheritdoc/>
        public void Subscribe<T>(Action<Message<T>> subscription)
        {
            // retry is needed, after retry if TryGet still fails maybe throe exception?
            if (!(_subscribers.ContainsKey(typeof(T)) && _subscribers.TryGetValue(typeof(T), out var delegates)))
                delegates = new List<Delegate>();

            if (delegates.Contains(subscription))
                return;

            delegates.Add(subscription);
            _subscribers.TryAdd(typeof(T), delegates);
        }

        /// <inheritdoc/>
        public void Unsubscribe<T>(Action<Message<T>> subscription)
        {
            if (!_subscribers.ContainsKey(typeof(T)))
                return;
            if (!_subscribers.TryGetValue(typeof(T), out var delegates))
                return;

            if (delegates.Contains(subscription))
                delegates.Remove(subscription);

            if (delegates.Count == 0)
                _subscribers.TryRemove(typeof(T), out _);
        }

        public void Dispose() => _subscribers?.Clear();
    }
}
