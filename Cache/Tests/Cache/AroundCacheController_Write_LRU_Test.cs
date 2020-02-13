using System;
using System.Linq;
using System.Text;
using System.Configuration;
using Cache;
using Cache.CacheController;
using Cache.ReplacementStrategy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mocks;

namespace Tests.Cache.CacheControllerTest
{
    [TestClass]
    public class AroundCacheController_Write_LRU_Test
    {
        DatabaseStorageMock<int, string> databaseStorage_ = new DatabaseStorageMock<int, string>();
        readonly int kNumberOfWays; //= 4;
        readonly int kLinesDegree;  //= 4;
        readonly int kWordsInLine;  //= 4;
        readonly int kWordSize;     //= 8;

        public AroundCacheController_Write_LRU_Test()
        {
            kNumberOfWays   = int.Parse(ConfigurationManager.AppSettings["NumberOfWays"]);
            kLinesDegree    = int.Parse(ConfigurationManager.AppSettings["LinesDegree"]);
            kWordsInLine    = int.Parse(ConfigurationManager.AppSettings["WordsInLine"]);
            kWordSize       = int.Parse(ConfigurationManager.AppSettings["WordSize"]);
        }

        ICacheController<int> CreateController()
        {
            databaseStorage_.database_.Clear();
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
        public void WriteWordTest1_SelectedTags()
        {
            ICacheController<int> cacheController = CreateController();

            // write -> read
            byte[] bytes42 = Encoding.ASCII.GetBytes("first42");
            byte[] bytes43 = Encoding.ASCII.GetBytes("first43");
            Word word42 = new Word(42, bytes42);
            Word word43 = new Word(43, bytes43);

            cacheController.WriteWord(42, word42);
            Word wordBack42 = cacheController.ReadWord(42);
            Assert.AreEqual(word42.Tag, wordBack42.Tag);

            cacheController.WriteWord(43, word43);
            Word wordBack43 = cacheController.ReadWord(43);
            Assert.AreEqual(word43.Tag, wordBack43.Tag);

            // update -> read
            byte[] byteSecond42 = Encoding.ASCII.GetBytes("second42");
            byte[] byteSecond43 = Encoding.ASCII.GetBytes("second43");
            Word wordSecond42 = new Word(42, bytes42);
            Word wordSecond43 = new Word(43, bytes43);

            cacheController.WriteWord(42, word42);
            Word wordBackSecond42 = cacheController.ReadWord(42);
            Assert.AreEqual(word42.Tag, wordBackSecond42.Tag);

            cacheController.WriteWord(43, word43);
            Word wordBackSecond43 = cacheController.ReadWord(43);
            Assert.AreEqual(word43.Tag, wordBackSecond43.Tag);
        }
        [TestMethod]
        public void WriteWordTest2_Sequential()
        {
            
            ICacheController<int> cacheController = CreateController();
            int i = 0;
            do
            {
                // write -> read
                string s = string.Format("1st{0:000}", i);
                Word word = new Word(i, Convert(s));

                cacheController.WriteWord(i, word);
                Word wordBack = cacheController.ReadWord(i);
                Assert.AreEqual(word.Tag, wordBack.Tag);
                Assert.IsTrue(word.Buffer.SequenceEqual(wordBack.Buffer));

                // update -> read
                s = string.Format("2nd{0:000}", i);
                Word wordSecond = new Word(i, Convert(s));

                cacheController.WriteWord(i, wordSecond);
                Word wordBackSecond = cacheController.ReadWord(i);
                Assert.AreEqual(wordSecond.Tag, wordBackSecond.Tag);

            } while (!databaseStorage_.EOF(++i));
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
            for (int i = 0; i < Math.Pow(2, 8); ++i)
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
