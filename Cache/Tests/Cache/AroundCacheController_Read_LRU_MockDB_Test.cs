using System;
using System.Text;
using Cache;
using Cache.CacheController;
using Cache.ReplacementStrategy;
using ClientApp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mocks;

namespace Tests.Cache.CacheControllerTest
{
    [TestClass]
    public class CacheController_Read_LRU_MockDB_Test
    {
        CacheController_Read_LRU_DB_Test dbTest = new CacheController_Read_LRU_DB_Test();
        IStorage<int> databaseStorage_ = null;
        const int kNumberOfWays = 4;
        const int kLinesDegree = 4;
        const int kWordsInLine = 4;
        const int kWordSize = 8;

        const int kMinSequentialUserID = 1; // database contains continuous sequence (without gaps)
        const int kMaxSequentialUserID = 290;


        ICacheController<int> CreateController()
        {
            databaseStorage_ = new DatabaseStorageMock<int, string>();
            FillDatabaseTest();
            CacheGeometry cacheGeometry = new CacheGeometry(numberOfWays: kNumberOfWays
                                                            , linesDegree: kLinesDegree
                                                            , wordsInLine: kWordsInLine
                                                            , wordSize: kWordSize
                                                            , 2);
            IReplacementStrategy<int> replacementStrategy = new LRUStrategy<int>(cacheGeometry);
            ICacheController<int> cacheController =
                CacheFactory<int>.Create(cacheGeometry, databaseStorage_, replacementStrategy);

            return cacheController;
        }
        [TestMethod]
        public void ReadWordTest1_SelectedTags()
        {
            dbTest.ReadWordTest1_SelectedTags();
        }
        [TestMethod]
        public void ReadWordTest2_Sequential()
        {
            dbTest.ReadWordTest2_Sequential();
        }
        [TestMethod]
        public void ReadWordTest3_Sequential()
        {
            dbTest.ReadWordTest3_Sequential();
        }
        [TestMethod]
        public void ReadWordTest4_Sequential()
        {
            dbTest.ReadWordTest4_Sequential();
        }
        [TestMethod]
        public void ReadWordTest5_FirstRepresentativeZero()
        {
            dbTest.ReadWordTest5_FirstRepresentativeZero();
        }
        [TestMethod]
        public void ReadWordTest6_AllRepresentatives()
        {
            dbTest.ReadWordTest6_AllRepresentatives();
        }
        public void FillDatabaseTest()
        {
            for (int i = kMinSequentialUserID; i <= kMaxSequentialUserID; ++i)
            {
                string s = string.Format("test{0:0000}", i);
                databaseStorage_.WriteWord(i, Convert(s));
            }

            databaseStorage_.WriteWord(999, Convert("test0999"));
        }
        static byte[] Convert(string inValue)
        {
            return Encoding.ASCII.GetBytes(inValue);
        }
    }
}
