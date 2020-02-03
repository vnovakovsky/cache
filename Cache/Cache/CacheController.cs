using Cache.ReplacementStrategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cache
{
    public class CacheController<Tag> where Tag : unmanaged
    {
        Cache<Tag> cache_;
        IStorage<Tag> storage_;
		public IReplacementStrategy<Tag> ReplacementStrategy { get; set; }

        public CacheController(CacheGeometry cacheGeometry, IStorage<Tag> storage)
        {
            cache_ = new Cache<Tag>(cacheGeometry);
            storage_ = storage;
        }
        public Word ReadWord(Tag tag)
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
            }
            ReplacementStrategy.SetRecentLine(tag, word.SetIndex);
            return word;
        }
        public void WriteWord(Tag tag, Word word)
        {
            List<Word> words = storage_.ReadLine(tag, cache_.CacheGeometry.WordsInLine);
            Word cachedWord = cache_.ReadWord(tag);
            int setIndex = -1;
            if (!cachedWord.IsEmpty)
            {
                // hit
                setIndex = cachedWord.SetIndex;
                cache_.SaveLine(setIndex, tag, words);
        }
            // miss
            //List<Word> words = storage_.ReadLine(tag, cache_.CacheGeometry.WordsInLine);
            setIndex = ReplacementStrategy.SelectVictim(tag);
            cache_.SaveLine(setIndex, tag, words);
        
        }
    }
}
