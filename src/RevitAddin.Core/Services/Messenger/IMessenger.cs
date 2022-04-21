using System;

namespace RevitAddin.Core.Messaging
{
    /// <summary>
    /// Interface IMessenger exposes the required functionality for a Messenger
    /// class to implement.
    /// </summary>
    public interface IMessenger
    {


        /// <summary>
        /// Subscribes the specified subscriber.
        /// </summary>
        /// <typeparam name="T"> The type of message to subscribe to. </typeparam>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="action">The notification action.</param>
        /// <param name="context">The context.</param>
        void Subscribe<T>(object subscriber, Action<T> action, object context);



        /// <summary>
        /// Unsubscribes the specified subscriber.
        /// </summary>
        /// <typeparam name="T">The type of message to unsubscribe from.</typeparam>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="context">The context.</param>
        void Unsubscribe<T>(object subscriber, object context);



        /// <summary>
        /// Publishes the specified message to the subscribers.
        /// </summary>
        /// <typeparam name="T"> The type of message to publish. </typeparam>
        /// <param name="message">The message.</param>
        /// <param name="context">The context.</param>
        void Publish<T>(T message, object context);

    }
}
