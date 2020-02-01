using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CacheSet;
using Cache;

namespace Tests.CacheSet
{
    /// <summary>
    /// Summary description for SetProxyTest
    /// </summary>
    [TestClass]
    public class SetProxyTest
    {
        public SetProxyTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void CreateSetProxyTest()
        {
            SetProxy setProxy = new SetProxy(4, 2, 16);
            byte[] bytes1 = Encoding.ASCII.GetBytes("test array 1");
            byte[] bytes2 = Encoding.ASCII.GetBytes("test array 2");
            Word word1 = new Word(42, bytes1);
            Word word2 = new Word(43, bytes2);
            List<Word> words = new List<Word> { word1, word2 };
            setProxy.PutWord(42, words);
        }
    }
}
