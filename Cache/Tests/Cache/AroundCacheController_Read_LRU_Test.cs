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
    public class Controller_Read_LRU_DB_Test
    {
        protected IStorage<int> databaseStorage_ = null;
        const int kNumberOfWays = 4;
        const int kLinesDegree = 4;
        const int kWordsInLine = 4;
        const int kWordSize = 8;

        const int kMinSequentialUserID = 1; // database contains continuous sequence (without gaps)
        const int kMaxSequentialUserID = 290;
        

        protected virtual ICacheController<int> CreateController()
        {
            databaseStorage_ = new DatabaseStorage<int, string>();


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
            ICacheController<int> cacheController = CreateController();
            Word word42     = cacheController.ReadWord(42);
            Word word42Hit  = cacheController.ReadWord(42); // read hit
            Assert.AreEqual(word42.Tag, word42Hit.Tag);
            Assert.AreEqual(word42Hit.isCached, true);

            Word word = cacheController.ReadWord(43);
            Word wordHit = cacheController.ReadWord(43); // read hit
            Assert.AreEqual(word.Tag, wordHit.Tag);
            Assert.AreEqual(wordHit.isCached, true);
        }
        [TestMethod]
        public void ReadWordTest2_Sequential()
        {
            ICacheController<int> cacheController = CreateController();
            int i = 0;
            do
            {
                Word word = cacheController.ReadWord(i);
                Word wordHit = cacheController.ReadWord(i); // read hit second time for the same word
                if (!wordHit.IsEmpty)
                {
                    Assert.AreEqual(word.Tag, wordHit.Tag);
                }
            } while (!databaseStorage_.EOF(++i));

        }
        [TestMethod]
        public void ReadWordTest3_Sequential()
        {
            ICacheController<int> cacheController = CreateController();
            // if line consists from n words then word0...wordn-1 will be not chached when word0 requested first time
            // but subsequent reading of word1..wordn-1 will hit
            for(int i = kMinSequentialUserID; i <= kMaxSequentialUserID; ++i)
            { 
                Word word = cacheController.ReadWord(i);
                if (i % kWordsInLine == 1)
                {
                    Assert.AreEqual(word.isCached, false); // miss for word0
                }
                else
                {
                    Assert.AreEqual(word.isCached, true); // hit for word1..wordn-1
                }
            }
            Word word291 = cacheController.ReadWord(291);
            Assert.IsTrue(word291.IsEmpty);

            Word word999 = cacheController.ReadWord(999);
            Assert.AreEqual(word999.isCached, true);
        }
        [TestMethod]
        public void ReadWordTest4_Sequential()
        {
            ICacheController<int> cacheController = CreateController();

            int i = 0;
            do
            {
                Word word = cacheController.ReadWord(i);
                if (word.IsEmpty)
                    continue;
                if (i % kWordsInLine == 1)
                {
                    Assert.AreEqual(word.isCached, false); // miss
                    word = cacheController.ReadWord(i);
                    Assert.AreEqual(word.isCached, true); // hit second time
                }
                else
                {
                    Assert.AreEqual(word.isCached, true); // hit
                }
            } while (!databaseStorage_.EOF(++i));
        }
        [TestMethod]
        public void ReadWordTest5_FirstRepresentativeZero()
        {
            ICacheController<int> cacheController = CreateController();
            int linesPerSet_Mod = (int)Math.Pow(2, kLinesDegree) / kNumberOfWays; ;

            // iterate throug all members of class [0] = Z linesPerSet_Mod = Z mod linesPerSet_Mod
            // rep = representative
            int rep = kMinSequentialUserID;
            for (int c = 0, tag = rep + linesPerSet_Mod * c;
                    tag <= kMaxSequentialUserID;
                    ++c, tag = rep + linesPerSet_Mod * c)
            {
                Word word = cacheController.ReadWord(tag);
                Assert.AreEqual(word.SetIndex, c % kNumberOfWays);
            }
        }
        [TestMethod]
        public void ReadWordTest6_AllRepresentatives()
        {
            ICacheController<int> cacheController = CreateController();
            int linesPerSet_Mod = (int)Math.Pow(2, kLinesDegree) / kNumberOfWays;

            // iterate throug all representatives of ring (Z mod linesPerSet_Mod): [0], [1], ..., [linesPerSet_Mod - 1]
            // rep = representative
            for (int rep = kMinSequentialUserID; rep < linesPerSet_Mod; ++rep)
            {
                // iterate throug all members of class [a] = Z linesPerSet_Mod = Z mod linesPerSet_Mod
                // [a] = {x: z mod linesPerSet_Mod == a, z E Z}
                for (int c = 0, tag = rep + linesPerSet_Mod * c;
                    tag <= kMaxSequentialUserID;
                    ++c, tag = rep + linesPerSet_Mod * c)
                {
                    Word word = cacheController.ReadWord(tag);
                    Assert.AreEqual(word.SetIndex, c % kNumberOfWays);
                }
            }
        }
    }
}
