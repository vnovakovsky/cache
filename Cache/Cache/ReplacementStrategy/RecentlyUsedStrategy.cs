using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cache.ReplacementStrategy
{
    public class RecentlyUsedStrategy<Tag> : IReplacementStrategy<Tag> where Tag : unmanaged
    {
        readonly CacheGeometry cacheGeometry_;
        List<long[]> registry_;
        private long counter_ = 0;
        Func<long, long, bool> comparisonOperator_;
        public RecentlyUsedStrategy(CacheGeometry cacheGeometry, Func<long, long, bool> comparisonOperator)
        {
            comparisonOperator_ = comparisonOperator;
            cacheGeometry_ = cacheGeometry;
            registry_ = new List<long[]>();
            for (int i = 0; i < cacheGeometry_.NumberOfWays; ++i)
            {
                long[] arr = new long[cacheGeometry.LinesPerSet];
                registry_.Add(arr);
            }
        }
        /// <summary>
        ///   Returns set index with least/most recently used line.
        /// </summary>
        public int SelectVictim(Tag tag)
        {
            int iTag = Util.ConvertToInt(tag);
            int lineInSet = iTag % cacheGeometry_.LinesPerSet;
            long extremum = registry_[0][lineInSet];
            int setIndex = 0;
            for (int s = 0; s < registry_.Count; ++s)
            {
                if (comparisonOperator_(registry_[s][lineInSet], extremum))
                {
                    extremum = registry_[s][lineInSet];
                    setIndex = s;
                }
            }
            return setIndex;
        }

        public void SetRecentLine(Tag tag, int setIndex)
        {
            int iTag = Util.ConvertToInt(tag);
            int lineInSet = iTag % cacheGeometry_.LinesPerSet;
            registry_[setIndex][lineInSet] = ++counter_;
        }
    }
}
