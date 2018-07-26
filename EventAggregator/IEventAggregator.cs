using System;

namespace UwpEventAggregator
{
    /// <summary>
    /// Event Aggregator Interface.
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
    public interface IEventAggregator
    {
        /// <summary>
        /// Publishes the specified message.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="P"></typeparam>
        /// <param name="payload">The payload.</param>
        void Publish<T, P>(P payload) where T : IEventType<P>;

        /// <summary>
        /// Subscribes the specified action.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="P"></typeparam>
        /// <param name="action">The action.</param>
        void Subscribe<T, P>(Action<P> action) where T : IEventType<P>;

        /// <summary>
        /// Uns the subscribe.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="P"></typeparam>
        /// <param name="action">The action.</param>
        void UnSubscribe<T, P>(Action<P> action) where T : IEventType<P>;

        /// <summary>
        /// Clears all subscriptions.
        /// </summary>
        void ClearAllSubscriptions();

        /// <summary>
        /// Determines whether the specified subscriber has registrations.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        /// <returns>
        ///   <c>true</c> if the specified subscriber has registrations; otherwise, <c>false</c>.
        /// </returns>
        bool HasRegistrations(object subscriber);
    }
}