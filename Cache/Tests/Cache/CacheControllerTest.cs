using System;
using System.Text;
using Cache;
using Cache.ReplacementStrategy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mocks;

namespace Tests.Cache
{
    [TestClass]
    public class CacheControllerTest
    {
        DatabaseStorageMock<int, string> databaseStorage_ = new DatabaseStorageMock<int, string>();
        [TestMethod]
        public void ReadWordTest()
        {
            databaseStorage_.database_.Clear();
            FillDatabaseTest();
            CacheGeometry cacheGeometry = new CacheGeometry ( numberOfWays : 4
                                                            , linesDegree : 5
                                                            , wordsInLine : 4
                                                            , wordSize : 8);
            IReplacementStrategy<int> replacementStrategy = new LRUStrategy<int>(cacheGeometry);
            CacheController<int> cacheController = 
                CacheFactory<int>.Create(cacheGeometry, databaseStorage_, replacementStrategy);
        }
        [TestMethod]
        public void FillDatabaseTest()
        {
            for(int i = 0; i < Math.Pow(2, 8); ++i)
            {
                string s = string.Format("test{0:0000}", i);
                databaseStorage_.WriteWord(i, Convert(s));
            }
 
            Assert.AreEqual(databaseStorage_.database_.Count, Math.Pow(2, 8));
        }
        static byte[] Convert(string inValue)
        {
            return Encoding.ASCII.GetBytes(inValue);
        }
    }
}
