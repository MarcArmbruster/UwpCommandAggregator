using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UwpCommandAggregator;

namespace UnitTests.CommandAggregator
{
    /// <summary>
    /// Private test class -> testing object.
    /// </summary>
    internal class MyBaseTestViewModel : BaseVm
    {
        internal string TestTag { get; set; }

        private int inputValue;
        public int InputValue
        {
            get => this.inputValue;
            set => this.SetPropertyValue(ref this.inputValue, value);
        }

        [DependsOn(nameof(InputValue))]
        public int SquareValue => this.InputValue * this.InputValue;        

        private string testProperty;
        public string TestProperty
        {
            get => this.testProperty;
            set => this.SetPropertyValue(ref this.testProperty, value);
        }

        private string extendedTestProperty;
        public string ExtendedTestProperty
        {
            get => this.extendedTestProperty;
            set => this.SetPropertyValue(
                ref this.extendedTestProperty,
                value,
                () => this.TestTag = "previous",
                () => this.TestTag = "after");
        }

        protected override void InitCommands()
        {
            this.CmdAgg.AddOrSetCommand("TestCommand", new RelayCommand(new Action<object>(p1 => { })));
        }

        public MyBaseTestViewModel()
        {
            this.AutoTriggerCommandNotification = true;
        }
    }

    /// <summary>
    /// Test class for WPF Command Aggregator.
    /// </summary>   
    /// <remarks>
    ///   <para><b>History</b></para>
    ///   <list type="table">
    ///   <item>
    ///   <term><b>Author:</b></term>
    ///   <description>Marc Armbruster</description>
    ///   </item>
    ///   <item>
    ///   <term><b>Date:</b></term>
    ///   <description>Jun/24/2016</description>
    ///   </item>
    ///   <item>
    ///   <term><b>Remarks:</b></term>
    ///   <description>Initial version.</description>
    ///   </item>
    ///   </list>
    /// </remarks>
    [TestClass]
    public class BaseVmTest
    {        
        [TestMethod]
        public void CmdAggNotNull()
        {
            MyBaseTestViewModel testVm = new MyBaseTestViewModel();

            // CommandAggregator is instanciated by construction
            Assert.IsNotNull(testVm.CmdAgg);

            // Test command is available
            Assert.AreEqual(1, testVm.CmdAgg.Count());
        }

        [TestMethod]
        public void SetPropertyTest()
        {
            string eventMsg = string.Empty;
            MyBaseTestViewModel testVm = new MyBaseTestViewModel();
            testVm.PropertyChanged += (s, e) => eventMsg = e.PropertyName;

            testVm.AutoTriggerCommandNotification = true;
            testVm.ExtendedTestProperty = "something";            
            Assert.AreEqual("after", testVm.TestTag);
            Assert.AreEqual("CmdAgg", eventMsg);

            testVm.AutoTriggerCommandNotification = false;
            testVm.ExtendedTestProperty = "somethingElse";
            Assert.AreEqual("after", testVm.TestTag);
            Assert.AreEqual("ExtendedTestProperty", eventMsg);
        }

        [TestMethod]
        public void AddAndExistsTest()
        {
            MyBaseTestViewModel testVm = new MyBaseTestViewModel();

            testVm.TestProperty = "Dummy";
            Assert.AreEqual("Dummy", testVm.TestProperty);

            Assert.IsTrue(testVm.CmdAgg.Exists("TestCommand"));
        }

        [TestMethod]
        public void NotifyPropetyChangeTest()
        {
            string eventMsg = string.Empty;
            MyBaseTestViewModel testVm = new MyBaseTestViewModel();
            testVm.PropertyChanged += (s, e) => eventMsg = e.PropertyName;

            testVm.AutoTriggerCommandNotification = true;
            testVm.NotifyPropertyChanged(nameof(testVm.TestProperty));            
            Assert.AreEqual("CmdAgg", eventMsg);

            testVm.AutoTriggerCommandNotification = false;
            testVm.NotifyPropertyChanged(nameof(testVm.TestProperty));            
            Assert.AreEqual("TestProperty", eventMsg);
        }

        [TestMethod]
        public void DependsOnTest()
        {
            List<string> eventMsgs = new List<string>();

            MyBaseTestViewModel testVm = new MyBaseTestViewModel();
            testVm.PropertyChanged += (s, e) => eventMsgs.Add(e.PropertyName);

            testVm.InputValue = 4;

            Assert.AreEqual(4, eventMsgs.Count);
            CollectionAssert.Contains(eventMsgs, nameof(MyBaseTestViewModel.CmdAgg));
            CollectionAssert.Contains(eventMsgs, nameof(MyBaseTestViewModel.InputValue));
            CollectionAssert.Contains(eventMsgs, nameof(MyBaseTestViewModel.SquareValue));
        }

        [TestMethod]
        public void SuppressNotificationsTest()
        {
            string eventMsg = string.Empty;
            MyBaseTestViewModel testVm = new MyBaseTestViewModel();
            testVm.PropertyChanged += (s, e) => eventMsg = e.PropertyName;

            testVm.SuppressNotifications = false;
            testVm.NotifyPropertyChanged(nameof(testVm.TestProperty));
            Assert.AreEqual("CmdAgg", eventMsg);

            eventMsg = string.Empty;
            testVm.SuppressNotifications = true;
            testVm.NotifyPropertyChanged(nameof(testVm.TestProperty));
            Assert.IsTrue(string.IsNullOrEmpty(eventMsg));
        }
    }
}
