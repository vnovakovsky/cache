using Cache.ReplacementStrategy;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cache.CacheController
{
    public abstract class CacheController<Tag> : ICacheController<Tag> where Tag : unmanaged, IComparable
    {
        protected Cache<Tag> cache_;
        protected IStorage<Tag> storage_;
        public IReplacementStrategy<Tag> ReplacementStrategy { get; set; }

        public CacheController(CacheGeometry cacheGeometry, IStorage<Tag> storage)
        {
            cache_ = new Cache<Tag>(cacheGeometry);
            storage_ = storage;
        }
        public virtual Word ReadWord(Tag tag)
        {
            Word word = cache_.ReadWord(tag);
            word.isCached = true;
            if (word.IsEmpty)
            {
                // read miss
                List<Word> words = storage_.ReadLine(tag, cache_.CacheGeometry.WordsInLine);
                if (words.Count != 0)
                {
                    int setIndex = ReplacementStrategy.SelectVictim(tag);
                    cache_.SaveLine(setIndex, tag, words);
                    word = words[0];
                    word.SetIndex = setIndex;
                    word.isCached = false;
                }
                else
                {
                    return Word.CreateEmpty(Util.ConvertToInt(tag));
                }
            }
            ReplacementStrategy.SetRecentLine(tag, word.SetIndex);
            return word;
        }

        public abstract void WriteWord(Tag tag, Word word);
    }
}

