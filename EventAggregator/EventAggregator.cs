using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace UwpEventAggregator
{
    /// <summary>
    /// Event Aggregator Implementation.
    /// </summary>
    /// <remarks>
    ///   <para><b>History</b></para>
    ///   <list type="table">
    ///   <item>
    ///   <term><b>Author:</b></term>
    ///   <description>M. Armbruster, PTA GmbH</description>
    ///   </item>
    ///   <item>
    ///   <term><b>Date:</b></term>
    ///   <description>21-Feb-2014</description>
    ///   </item>
    ///   <item>
    ///   <term><b>Remarks:</b></term>
    ///   <description>initial version</description>
    ///   </item>
    ///   </list>
    /// </remarks>
    public sealed class EventAggregator : IEventAggregator
    {
        /// <summary>
        /// The subscriptions.
        /// </summary>
        private readonly ConcurrentDictionary<Type, List<Action>> subscriptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventAggregator"/> class.
        /// </summary>
        public EventAggregator()
        {
            this.subscriptions = new ConcurrentDictionary<Type, List<Action>>();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="EventAggregator"/> class.
        /// </summary>
        ~EventAggregator()
        {
            this.ClearAllSubscriptions();
        }

        /// <summary>
        /// Publishes the specified event.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="P"></typeparam>
        /// <param name="payload">The payload.</param>
        /// <exception cref="ArgumentNullException">payload</exception>
        public void Publish<T, P>(P payload) where T : IEventType<P>
        {
            if (payload == null)
            {
                throw new ArgumentNullException("payload");
            }

            var keyType = typeof(T);
            
            if (this.subscriptions.ContainsKey(keyType))
            {
                foreach (var subscription in this.subscriptions[keyType].Cast<Action<P>>())
                {
                    subscription.Invoke(payload);                    
                }
            }
        }

        /// <summary>
        /// Subscribes the specified action.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="P"></typeparam>
        /// <param name="action">The action.</param>
        public void Subscribe<T, P>(Action<P> action) where T : IEventType<P>
        {
            if (action == null)
            {
                return;
            }

            Type messageType = typeof(T);
            Action registerAction = action as Action;


            if (this.subscriptions.ContainsKey(messageType))
            {
                if (this.subscriptions[messageType].Any(a => a != null && a.Equals(registerAction)))
                {
                    var message = $"The event [{messageType.FullName}] is already registered for object [{action.Target.GetType().FullName}]";
                    throw new AlreadyRegisteredException(message);
                }

                this.subscriptions[messageType].Add(registerAction as Action);
            }
            else
            {
                this.subscriptions.TryAdd(messageType, new List<Action> { registerAction });
            }            
        }

        /// <summary>
        /// Unsubscribe.
        /// </summary>
        /// <typeparam name="TPayload">The type of the message.</typeparam>
        /// <param name="subscription">The subscription.</param>
        public void UnSubscribe<T, P>(Action<P> action) where T : IEventType<P>
        {
            Type messageType = typeof(T);
            Action registerAction = action as Action;

            if (this.subscriptions.ContainsKey(messageType))
            {
                var targets = this.subscriptions[messageType].Where(a => a != null && a.Equals(registerAction));
                if (targets.Any())
                {
                    foreach (var target in targets)
                    {
                        this.subscriptions[messageType].Remove(target);
                    }
                }
            }
        }

        /// <summary>
        /// Clears all subscriptions.
        /// </summary>
        public void ClearAllSubscriptions()
        {
            this.subscriptions.Clear();
        }

        /// <summary>
        /// Determines whether the specified subscriber has registrations.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        /// <returns>
        ///   <c>true</c> if the specified subscriber has registrations; otherwise, <c>false</c>.
        /// </returns>
        public bool HasRegistrations(object subscriber)
        {
            if (subscriber == null)
            {
                return false;
            }

            return false;
            //return this.subscriptions.Select(s => s.Value).Where(i => i.Any(a => a != null && a.Equals(subscriber)).Any();
        }
    }
}