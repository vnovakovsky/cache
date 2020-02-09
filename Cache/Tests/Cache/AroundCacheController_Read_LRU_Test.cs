using System;
using System.Text;
using Cache;
using Cache.CacheController;
using Cache.ReplacementStrategy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mocks;

namespace Tests.Cache.CacheControllerTest
{
    [TestClass]
    public class CacheController_Read_LRU_Test
    {
        DatabaseStorageMock<int, string> databaseStorage_ = new DatabaseStorageMock<int, string>();
        const int kNumberOfWays = 4;
        const int kLinesDegree = 4;
        const int kWordsInLine = 4;
        const int kWordSize = 8;

        ICacheController<int> CreateController()
        {
            databaseStorage_.database_.Clear();
            FillDatabaseTest();
            CacheGeometry cacheGeometry = new CacheGeometry(numberOfWays: kNumberOfWays
                                                            , linesDegree: kLinesDegree
                                                            , wordsInLine: kWordsInLine
                                                            , wordSize: kWordSize);
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

            Word word = cacheController.ReadWord(43);
            Word wordHit = cacheController.ReadWord(43); // read hit
            Assert.AreEqual(word.Tag, wordHit.Tag);
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
                Assert.AreEqual(word.Tag, wordHit.Tag);
            } while (!databaseStorage_.EOF(++i));

        }
        [TestMethod]
        public void ReadWordTest3_Sequential()
        {
            ICacheController<int> cacheController = CreateController();
            const int wordsInLine = 4;
            // if line consists from n words then word0...wordn-1 will be not chached when word0 requested first time
            // but subsequent reading of word1..wordn-1 will hit
            int i = 0;
            do
            {
                Word word = cacheController.ReadWord(i);
                if (i % wordsInLine == 0)
                {
                    Assert.AreEqual(word.isCached, false); // miss for word0
                }
                else
                {
                    Assert.AreEqual(word.isCached, true); // hit for word1..wordn-1
                }
            } while (!databaseStorage_.EOF(i++));
        }
        [TestMethod]
        public void ReadWordTest4_Sequential()
        {
            ICacheController<int> cacheController = CreateController();
            const int wordsInLine = 4;

            int i = 0;
            do
            {
                Word word = cacheController.ReadWord(i);
                if (i % wordsInLine == 0)
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
            int rep = 0;
            for (int c = 0, tag = rep + linesPerSet_Mod * c;
                    tag < databaseStorage_.MaxKey();
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
            for (int rep = 0; rep < linesPerSet_Mod; ++rep)
            {
                // iterate throug all members of class [a] = Z linesPerSet_Mod = Z mod linesPerSet_Mod
                // [a] = {x: z mod linesPerSet_Mod == a, z E Z}
                for (int c = 0, tag = rep + linesPerSet_Mod * c;
                    tag < databaseStorage_.MaxKey();
                    ++c, tag = rep + linesPerSet_Mod * c)
                {
                    Word word = cacheController.ReadWord(tag);
                    Assert.AreEqual(word.SetIndex, c % kNumberOfWays);
                }
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
