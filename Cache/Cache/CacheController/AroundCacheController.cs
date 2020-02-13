using Cache.ReplacementStrategy;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cache.CacheController
{
    public class AroundCacheController<Tag> : CacheController<Tag> where Tag : unmanaged, IComparable
    {
        public AroundCacheController(CacheGeometry cacheGeometry, IStorage<Tag> storage) 
            : base(cacheGeometry, storage)
        {
        }

        /// <summary>
        ///   write-through / write-around approach.
        /// </summary>
        /** 
         *  In the case of a cache hit, we must certainly modify the contents of the cache
            line, because subsequent reads to that line must return the most up-to-date information.
            Regarding deffering of write there are two options: 1)writeback and 2) write-through

            write-through:  write both in the cache and in the next level of the hierarchy.
                            next level in hierarchy should be updated immediately after call write to cache.
                            With regard to whether write to cache in case of write miss there are two
                            options: 1) write-allocate and 2) write-around.

            write-around: is to write ONLY in the next level of the memory hierarchy.
        */
        public override void WriteWord(Tag tag, Word word)
        {
            storage_.WriteWord(tag, word.Buffer); // shared for miss or hit
            Word cachedWord = cache_.ReadWord(tag, Word.kInvalidate);
        }
    }
}
