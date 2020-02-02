using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cache.ReplacementStrategy
{
    public class LRUStrategy<Tag> : IReplacementStrategy<Tag> where Tag : unmanaged
    {
        readonly CacheGeometry cacheGeometry_;
        List<long[]> registry_;
        private long counter_ = 0;
        public LRUStrategy(CacheGeometry cacheGeometry)
        {
            cacheGeometry_ = cacheGeometry;
            registry_ = new List<long[]>();
            for(int i = 0; i < cacheGeometry_.NumberOfWays; ++i)
            {
                long[] arr = new long[cacheGeometry.LinesPerCache];
                registry_.Add(arr);
            }
        }
        /// <summary>
        ///   Returns set index with least recently used line.
        /// </summary>
        public int SelectVictim(Tag tag)
        {
            int iTag = Util.ConvertToInt(tag);
            int lineInSet = iTag % cacheGeometry_.LinesPerCache;
            long min = 0;
            int setIndex = 0;
            for (int s = 0; s < registry_.Count; ++s)
            {
                if(registry_[s][lineInSet] < min)
                {
                    min = registry_[s][lineInSet];
                    setIndex = s;
                }
            }
            return setIndex;
        }

        public void SetRecentWord(Tag tag, int setIndex)
        {
            int iTag = Util.ConvertToInt(tag);
            int lineInSet = iTag % cacheGeometry_.LinesPerCache;
            registry_[setIndex][lineInSet] = ++counter_;
        }
    }
}
