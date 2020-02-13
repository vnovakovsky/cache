using System;
using System.Configuration;
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
    public class Controller_Read_LRU_MockDB_Test : Controller_Read_LRU_DB_Test
    {
        readonly int kNumberOfWays; //= 4;
        readonly int kLinesDegree;  //= 4;
        readonly int kWordsInLine;  //= 4;
        readonly int kWordSize;     //= 8;

        const int kMinSequentialUserID = 1; // database contains continuous sequence (without gaps)
        const int kMaxSequentialUserID = 290;

        public Controller_Read_LRU_MockDB_Test()
        {
            kNumberOfWays   = int.Parse(ConfigurationManager.AppSettings["NumberOfWays"]);
            kLinesDegree    = int.Parse(ConfigurationManager.AppSettings["LinesDegree"]);
            kWordsInLine    = int.Parse(ConfigurationManager.AppSettings["WordsInLine"]);
            kWordSize       = int.Parse(ConfigurationManager.AppSettings["WordSize"]);
        }

        protected override ICacheController<int> CreateController()
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
        public new void ReadWordTest1_SelectedTags()
        {
            base.ReadWordTest1_SelectedTags();
        }
        [TestMethod]
        public new void ReadWordTest2_Sequential()
        {
            base.ReadWordTest2_Sequential();
        }
        [TestMethod]
        public new void ReadWordTest3_Sequential()
        {
            base.ReadWordTest3_Sequential();
        }
        [TestMethod]
        public new void ReadWordTest4_Sequential()
        {
            base.ReadWordTest4_Sequential();
        }
        [TestMethod]
        public new void ReadWordTest5_FirstRepresentativeZero()
        {
            base.ReadWordTest5_FirstRepresentativeZero();
        }
        [TestMethod]
        public new void ReadWordTest6_AllRepresentatives()
        {
            base.ReadWordTest6_AllRepresentatives();
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
