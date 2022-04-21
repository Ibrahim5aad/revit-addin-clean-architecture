using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace RevitAddin.Core.Messaging
{

    /// <summary>
    /// A Messenger class to handle communication between ViewModels.
    /// </summary>
    public class Messenger : IMessenger
    {

        #region Private Fields

        private static readonly object _creationLock = new object();

        private static readonly ConcurrentDictionary<MessengerKey, object> _dictionary =
                                                      new ConcurrentDictionary<MessengerKey, object>();

        private static Messenger _instance;

        #endregion

        #region Singleton

        /// <summary>
        /// Gets the single instance of the Messenger.
        /// </summary>
        public static Messenger Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_creationLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new Messenger();
                        }
                    }
                }

                return _instance;
            }
            set => _instance = value;
        }

        #endregion

        #region Private Constructor


        /// <summary>
        /// Initializes a new instance of the Messenger class.
        /// </summary>
        private Messenger() { }


        #endregion

        #region Methods

        public void Reset()
        {
            _dictionary.Clear();
        }
        /// <summary>
        /// Subscribes a subscriber for a type of message T. The action parameter will be executed
        /// when a corresponding message is published.
        /// </summary>
        /// <typeparam name="T">The type of message to subscribe to.</typeparam>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="action">The action.</param>
        public void Subscribe<T>(object subscriber, Action<T> action)
        {
            Subscribe(subscriber, action, null);
        }

        /// <summary>
        /// Subscribes a subscriber for a type of message T and a matching context.
        /// The action parameter will be executed when a corresponding message is published.
        /// </summary>
        /// <typeparam name="T">The type of message to subscribe to.</typeparam>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="action">The notification action.</param>
        /// <param name="context">The context.</param>
        public void Subscribe<T>(object subscriber, Action<T> action, object context)
        {
            var key = new MessengerKey(subscriber, context, typeof(T));
            _dictionary.TryAdd(key, action);
        }

        /// <summary>
        /// Unsubscribes a messenger subscriber completely.
        /// After this method is executed, the subscriber will
        /// no longer receive any messages.
        /// </summary>
        /// <typeparam name="T">The type of message to unsubscribe from.</typeparam>
        /// <param name="subscriber">The subscriber.</param>
        public void Unsubscribe<T>(object subscriber)
        {
            Unsubscribe<T>(subscriber, null);
        }

        /// <summary>
        /// Unsubscribes a messenger subscriber with a matching context completely.
        /// After this method is executed, the subscriber will no longer receive any messages.
        /// </summary>
        /// <typeparam name="T">The type of message to unsubscribe from.</typeparam>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="context">The context.</param>
        public void Unsubscribe<T>(object subscriber, object context)
        {
            var key = new MessengerKey(subscriber, context, typeof(T));
            _dictionary.TryRemove(key, out object action);
        }

        /// <summary>
        /// Publishes a message to subscribers. The message will reach all subscribers that are
        /// subscribed for this message type.
        /// </summary>
        /// <typeparam name="T">The type of message to publish.</typeparam>
        /// <param name="message">The message.</param>
        public void Publish<T>(T message)
        {
            Publish(message, null);
        }

        /// <summary>
        /// Publishes a message to subscribers. The message will reach all subscribers that are
        /// subscribed for this message type and matching context.
        /// </summary>
        /// <typeparam name="T">The type of message to publish.</typeparam>
        /// <param name="message">The message.</param>
        /// <param name="context">The context.</param>
        public void Publish<T>(T message, object context)
        {
            IEnumerable<KeyValuePair<MessengerKey, object>> result;

            if (context == null)
            {
                // Get all subscribers where the context is null.
                result = from r in _dictionary where r.Key.Context == null select r;
            }
            else
            {
                // Get all subscribers where the context is matching.
                result = from r in _dictionary where r.Key.Context != null && r.Key.Context.Equals(context) select r;
            }

            foreach (var action in result.Select(x => x.Value).OfType<Action<T>>())
            {
                // Send the message to all subscribers.
                action(message);
            }
        }

        #endregion


        /// <summary>
        /// Class MessengerKey used as a key for message subscribers.
        /// Implements the <see cref="System.IEquatable{Application.Messaging.Messenger.MessengerKey}" />
        /// </summary>
        /// <seealso cref="System.IEquatable{Application.Messaging.Messenger.MessengerKey}" />
        protected class MessengerKey : IEquatable<MessengerKey>
        {

            #region Constructor

            /// <summary>
            /// Initializes a new instance of the MessengerKey class.
            /// </summary>
            /// <param name="subscriber">The subscriber.</param>
            /// <param name="context">The context.</param>
            /// <param name="type">The type of message.</param>
            public MessengerKey(object subscriber, object context, Type type)
            {
                Subscriber = subscriber;
                Context = context;
                Type = type;
            }

            #endregion

            #region Properties

            public object Subscriber { get; }
            public object Context { get; }
            public Type Type { get; }

            #endregion

            #region Equality Members

            /// <summary>
            /// Indicates whether the current object is equal to another object of the same type.
            /// </summary>
            /// <param name="other">An object to compare with this object.</param>
            /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
            public bool Equals(MessengerKey other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;

                return Equals(Subscriber, other.Subscriber) && Equals(Context, other.Context) && Equals(Type, other.Type);
            }

            /// <summary>
            /// Determines whether the specified object is equal to the current object.
            /// </summary>
            /// <param name="obj">The object to compare with the current object.</param>
            /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;

                return Equals((MessengerKey)obj);
            }

            /// <summary>
            /// Serves as the default hash function.
            /// </summary>
            /// <returns>A hash code for the current object.</returns>
            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = Subscriber?.GetHashCode() ?? 0;
                    hashCode = (hashCode * 397) ^ (Context?.GetHashCode() ?? 0);
                    hashCode = (hashCode * 397) ^ (Type?.GetHashCode() ?? 0);
                    return hashCode;
                }
            }


            /// <summary>
            /// Implements the == operator.
            /// </summary>
            /// <param name="left">The left operand.</param>
            /// <param name="right">The right operand.</param>
            /// <returns>The result of the operator.</returns>
            public static bool operator ==(MessengerKey left, MessengerKey right)
            {
                return Equals(left, right);
            }


            /// <summary>
            /// Implements the != operator.
            /// </summary>
            /// <param name="left">The left operand.</param>
            /// <param name="right">The right operand.</param>
            /// <returns>The result of the operator.</returns>
            public static bool operator !=(MessengerKey left, MessengerKey right)
            {
                return !Equals(left, right);
            }

            #endregion
        }
    }
}
