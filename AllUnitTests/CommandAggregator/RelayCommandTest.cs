﻿using System;
using System.Windows.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UwpCommandAggregator;

namespace UnitTests.CommandAggregator
{
    /// <summary>
    /// Test class for RelayCommand.
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
    ///   <description>Apr/12/2015</description>
    ///   </item>
    ///   <item>
    ///   <term><b>Remarks:</b></term>
    ///   <description>Initial version.</description>
    ///   </item>
    ///   </list>
    /// </remarks>
    [TestClass]
    public class RelayCommandTest
    {
        private class LocalTestingClass
        {
            internal string Test { get; set; }

            public LocalTestingClass()
            {
                this.Test = string.Empty;
            }
        }

        [TestMethod]
        public void DefaultTest()
        {
            LocalTestingClass testObject = new LocalTestingClass();
            ICommand cmd = new RelayCommand(p1 => testObject.Test = p1.ToString(), p2 => true);
            cmd.Execute("defaulttest");

            Assert.AreEqual("defaulttest", testObject.Test);
        }

        [TestMethod]
        public void PreActionTest()
        {

            LocalTestingClass testObject = new LocalTestingClass();

            Action preAction = new Action(() => testObject.Test = "start");

            ICommand cmd = new RelayCommand(p1 => testObject.Test += p1.ToString(), p2 => true, preAction, null);
            cmd.Execute("pretest");

            Assert.AreEqual("startpretest", testObject.Test);
        }

        [TestMethod]
        public void PostActionTest()
        {

            LocalTestingClass testObject = new LocalTestingClass();

            Action postAction = new Action(() => testObject.Test += "end");

            ICommand cmd = new RelayCommand(p1 => testObject.Test += p1.ToString(), p2 => true, null, postAction);
            cmd.Execute("pretest");

            Assert.AreEqual("pretestend", testObject.Test);
        }

        [TestMethod]
        public void OverridePostActionTest()
        {
            LocalTestingClass testObject = new LocalTestingClass();

            RelayCommand cmd = new RelayCommand(p1 => testObject.Test += p1.ToString(), p2 => true);
            cmd.OverridePostActionDelegate(() => testObject.Test += "postOverriden");
            cmd.Execute("");

            Assert.AreEqual("postOverriden", testObject.Test);
        }

        [TestMethod]
        public void OverridePreActionTest()
        {
            LocalTestingClass testObject = new LocalTestingClass();

            RelayCommand cmd = new RelayCommand(p1 => testObject.Test += p1.ToString(), p2 => true);
            cmd.OverridePreActionDelegate(() => testObject.Test += "preOverriden");
            cmd.Execute("");

            Assert.AreEqual("preOverriden", testObject.Test);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MissingExecuteDelegateConstructorTestOne()
        {
            RelayCommand cmd = new RelayCommand(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MissingExecuteDelegateConstructorTestTwo()
        {
            RelayCommand cmd = new RelayCommand(null, ce => true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MissingExecuteDelegateConstructorTestThree()
        {
            RelayCommand cmd = new RelayCommand(null, ce => true, () => { }, () => { });
        }
    }
}
