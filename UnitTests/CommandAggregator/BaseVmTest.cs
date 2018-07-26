using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using UwpCommandAggregator;

namespace UnitTests.CommandAggregator
{
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
        /// <summary>
        /// Private test class -> testing object.
        /// </summary>
        private class BaseTestViewModel : BaseVm
        {
            private string testProperty;
            public string TestProperty
            {
                get
                {
                    return this.testProperty;
                }

                set
                {
                    this.testProperty = value;
                    this.NotifyPropertyChanged("TestProperty");
                }
            }

            protected override void InitCommands()
            {
                this.CmdAgg.AddOrSetCommand("TestCommand", new RelayCommand(new Action<object>(p1 => { })));
            }
        }

        [TestMethod]
        public void AddAndExistsTest()
        {
            BaseTestViewModel testVm = new BaseTestViewModel();

            testVm.TestProperty = "Dummy";
            Assert.AreEqual("Dummy", testVm.TestProperty);

            Assert.IsTrue(testVm.CmdAgg.Exists("TestCommand"));
        }
    }
}
