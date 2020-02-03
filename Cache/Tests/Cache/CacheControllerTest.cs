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

        CacheController<int> CreateController()
        {
            databaseStorage_.database_.Clear();
            FillDatabaseTest();
            CacheGeometry cacheGeometry = new CacheGeometry(numberOfWays: 4
                                                            , linesDegree: 5
                                                            , wordsInLine: 4
                                                            , wordSize: 8);
            IReplacementStrategy<int> replacementStrategy = new LRUStrategy<int>(cacheGeometry);
            CacheController<int> cacheController =
                CacheFactory<int>.Create(cacheGeometry, databaseStorage_, replacementStrategy);

            return cacheController;
        }
        [TestMethod]
        public void ReadWordTest1()
        {
            CacheController<int> cacheController = CreateController();
            Word word42     = cacheController.ReadWord(42);
            Word word42Hit  = cacheController.ReadWord(42); // read hit
            Assert.AreEqual(word42.Tag, word42Hit.Tag);

            Word word = cacheController.ReadWord(43);
            Word wordHit = cacheController.ReadWord(43); // read hit
            Assert.AreEqual(word.Tag, wordHit.Tag);
        }
        [TestMethod]
        public void ReadWordTest2()
        {
            CacheController<int> cacheController = CreateController();
            int i = 0;
            do
            {
                Word word = cacheController.ReadWord(i);
                Word wordHit = cacheController.ReadWord(i); // read hit
                Assert.AreEqual(word.Tag, wordHit.Tag);
            } while (!databaseStorage_.EOF(i++));

        }
        [TestMethod]
        public void ReadWordTest3()
        {
            CacheController<int> cacheController = CreateController();
            const int wordsInLine = 4;

            int i = 0;
            do
            {
                Word word = cacheController.ReadWord(i);
                if (i % wordsInLine == 0)
                {
                    Assert.AreEqual(word.isCached, false);
                }
                else
                {
                    Assert.AreEqual(word.isCached, true);
                }
            } while (!databaseStorage_.EOF(i++));
        }
        [TestMethod]
        public void ReadWordTest4()
        {
            CacheController<int> cacheController = CreateController();
            const int wordsInLine = 4;

            int i = 0;
            do
            {
                Word word = cacheController.ReadWord(i);
                if (i % wordsInLine == 0)
                {
                    Assert.AreEqual(word.isCached, false);
                    word = cacheController.ReadWord(i);
                    Assert.AreEqual(word.isCached, true);
                }
                else
                {
                    Assert.AreEqual(word.isCached, true);
                }
            } while (!databaseStorage_.EOF(i++));
        }
        [TestMethod]
        public void ReadWordTest5()
        {
            CacheController<int> cacheController = CreateController();
            const int linesPerSet = 8;

            int offset = 0;
            int tag = 0;
            for (int c = 1; tag < 256; ++c)
            {
                Word word = cacheController.ReadWord(tag);
                Assert.AreEqual(word.SetIndex, (c-1) % 4);
                tag = offset + linesPerSet * c;
            }
        }
        private void ReadWord(CacheController<int> cacheController, int tag)
        {
            Word word = cacheController.ReadWord(tag);
            Word wordHit = cacheController.ReadWord(tag); // read hit
            Assert.AreEqual(word.Tag, wordHit.Tag);
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
