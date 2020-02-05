using System;
using System.Collections.Generic;


namespace Cache.CacheController
{
    public class AllocateCacheController<Tag> : CacheController<Tag> where Tag : unmanaged
    {
        public AllocateCacheController(CacheGeometry cacheGeometry, IStorage<Tag> storage)
            : base(cacheGeometry, storage)
        {
        }

        /// <summary>
        ///   write-through / write-allocate approach.
        /// </summary>
        /** 
         *  In the case of a cache hit, we must certainly modify the contents of the cache
            line, because subsequent reads to that line must return the most up-to-date information.
            Regarding deffering of write there are two options: 1)writeback and 2) write-through

            write-through:  write both in the cache and in the next level of the hierarchy.
                            next level in hierarchy should be updated immediately after call write to cache.
                            With regard to whether write to cache in case of write miss there are two
                            options: 1) write-allocate and 2) write-around.

            
            write-allocate, in case of miss is to treat the write miss as a read miss followed by
                            a write hit. In contrary to write-around that is writes
                            ONLY in the next level of the memory hierarchy.
        */
        public override void WriteWord(Tag tag, Word word)
        {
            // for both hit and miss we read from lower level in memory hierarchy
            // because in any case we are going to save data in cache
            // for hit - in the same set as old word occupies (but the whole new line), 
            // for miss - in any set returned by replacement algorithm

            storage_.WriteWord(tag, word.Buffer); // if can add new record then words is not empty
            
            List<Word> words = storage_.ReadLine(tag, cache_.CacheGeometry.WordsInLine);
            if (words.Count != 0)
            {
                int setIndex = 0;
                bool isFirstInLine = Util.ConvertToInt(tag) == words[0].Tag;
                bool invalidate = !isFirstInLine;
                Word cachedWord = cache_.ReadWord(tag, invalidate);
                if (isFirstInLine  && !cachedWord.IsEmpty)
                {
                    // hit - rewrite old line in the same place
                    setIndex = cachedWord.SetIndex;
                }
                {
                    //miss or passed tag is not first in line and therefore tag maps not in old line
                    // ans replacement algorithm select new set index
                    setIndex = ReplacementStrategy.SelectVictim(tag);
                }
                cache_.SaveLine(setIndex, tag, words);
            }            
        }
    }
}

