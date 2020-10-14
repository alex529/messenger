using System;

namespace BRE
{
    public interface IMessenger
    {
        /// <summary>
        /// Publishes a message on the messenger, all registered subscribers will be called concurrently with the payload.
        /// </summary>
        /// <typeparam name="T">Supports any type</typeparam>
        /// <param name="message">Used to specify the message payload</param>
        void Publish<T>(T message);

        /// <summary>
        /// Registers a "processor" for the type of message
        /// </summary>
        /// <typeparam name="T">Supports any type</typeparam>
        /// <param name="subscription">Used to specify the processor, which will be called when a message of type T is Published</param>
        void Subscribe<T>(Action<Message<T>> subscription);

        /// <summary>
        /// Unregisters handlers
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subscription"></param>
        void Unsubscribe<T>(Action<Message<T>> subscription);
    }

    /// <summary>
    /// Wrapper class for messages, used to support "metadata" further down the line
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Message<T>
    {
        public T Payload { get; private set; }
        public Message(T payload)
        {
            Payload = payload;
        }
    }
}
