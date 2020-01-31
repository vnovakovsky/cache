using System;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mocks;

namespace Tests.ClientLib
{
    [TestClass]
    public class DatabaseStorageMockTest
    {
        DatabaseStorageMock<int, string> databaseStorage_ = new DatabaseStorageMock<int, string>();

        [TestMethod]
        public void WriteWordTest()
        {
            databaseStorage_.WriteWord(1, Convert("a"));
            databaseStorage_.WriteWord(2, Convert("b"));
            databaseStorage_.WriteWord(3, Convert("c"));
            databaseStorage_.WriteWord(4, Convert("d"));
            databaseStorage_.WriteWord(5, Convert("e"));
            Assert.AreEqual(databaseStorage_.database_.Count, 5);
        }
        static byte[] Convert(string inValue)
        {
            return Encoding.ASCII.GetBytes(inValue);
        }
        [TestMethod]
        public void ReadWordTest()
        {
            WriteWordTest();
            
            Assert.IsTrue((databaseStorage_.ReadWord(1)).SequenceEqual(Convert("a")));
            Assert.IsTrue((databaseStorage_.ReadWord(2)).SequenceEqual(Convert("b")));
            Assert.IsTrue((databaseStorage_.ReadWord(3)).SequenceEqual(Convert("c")));
            Assert.IsTrue((databaseStorage_.ReadWord(4)).SequenceEqual(Convert("d")));
            Assert.IsTrue((databaseStorage_.ReadWord(5)).SequenceEqual(Convert("e")));
        }
    }
}
