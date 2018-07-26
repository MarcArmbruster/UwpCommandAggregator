using Microsoft.VisualStudio.TestTools.UnitTesting;
using UwpEventAggregator;

namespace EventAggregatorTests
{
    [TestClass]
    public class EventAggregatorTests
    {
        [TestMethod]
        public void HasRegistrationsNullTest()
        {
            IEventAggregator eventAggregator = new EventAggregator();
            Assert.IsFalse(eventAggregator.HasRegistrations(null));
        }

        [TestMethod]
        public void HasRegistrationsTrueTest()
        {
            IEventAggregator eventAggregator = new EventAggregator();

            TestSubscriber sub = new TestSubscriber();
            TestEvent<string> testEvent = new TestEvent<string>();

            eventAggregator.Subscribe<TestEvent<string>, string>(s => sub.TestCallBack(s));

            Assert.IsTrue(eventAggregator.HasRegistrations(sub));
        }

        [TestMethod]
        public void HasRegistrationsFalseTest()
        {
            IEventAggregator eventAggregator = new EventAggregator();

            TestSubscriber sub = new TestSubscriber();
            TestSubscriber sub2 = new TestSubscriber();
            TestEvent<string> testEvent = new TestEvent<string>();

            eventAggregator.Subscribe<TestEvent<string>, string>(s => sub.TestCallBack(s));

            Assert.IsFalse(eventAggregator.HasRegistrations(sub));
        }

        [TestMethod]
        public void RemoveTest()
        {
            IEventAggregator eventAggregator = new EventAggregator();

            TestSubscriber sub1 = new TestSubscriber();
            TestSubscriber sub2 = new TestSubscriber();
            TestEvent<string> testEvent1 = new TestEvent<string>();
            TestEvent<string> testEvent2 = new TestEvent<string>();

            eventAggregator.Subscribe<TestEvent<string>, string>(s => sub1.TestCallBack(s));
            eventAggregator.Subscribe<TestEvent<string>, string>(s => sub2.TestCallBack(s));

            Assert.IsTrue(eventAggregator.HasRegistrations(sub1));
            Assert.IsTrue(eventAggregator.HasRegistrations(sub2));

            eventAggregator.UnSubscribe<TestEvent<string>, string>(s => sub1.TestCallBack(s));

            Assert.IsFalse(eventAggregator.HasRegistrations(sub1));
            Assert.IsTrue(eventAggregator.HasRegistrations(sub2));
        }

        [TestMethod]
        public void ClearTest()
        {
            IEventAggregator eventAggregator = new EventAggregator();

            TestSubscriber sub1 = new TestSubscriber();
            TestSubscriber sub2 = new TestSubscriber();
            TestEvent<string> testEvent1 = new TestEvent<string>();
            TestEvent<string> testEvent2 = new TestEvent<string>();

            eventAggregator.Subscribe<TestEvent<string>, string>(s => sub1.TestCallBack(s));
            eventAggregator.Subscribe<TestEvent<string>, string>(s => sub2.TestCallBack(s));

            Assert.IsTrue(eventAggregator.HasRegistrations(sub1));
            Assert.IsTrue(eventAggregator.HasRegistrations(sub2));

            eventAggregator.ClearAllSubscriptions();

            Assert.IsFalse(eventAggregator.HasRegistrations(sub1));
            Assert.IsFalse(eventAggregator.HasRegistrations(sub2));
        }

        [TestMethod]
        [ExpectedException(typeof(AlreadyRegisteredException))]
        public void DuplicateExceptionTest()
        {
            IEventAggregator eventAggregator = new EventAggregator();
            TestSubscriber sub1 = new TestSubscriber();
            
            eventAggregator.Subscribe<TestEvent<string>, string>(s => sub1.TestCallBack(s));
            eventAggregator.Subscribe<TestEvent<string>, string>(s => sub1.TestCallBack(s));
        }
    }
}