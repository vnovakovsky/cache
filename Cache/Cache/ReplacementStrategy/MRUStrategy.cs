

namespace Cache.ReplacementStrategy
{
    public class MRUStrategy<Tag> : RecentlyUsedStrategy<Tag> where Tag : unmanaged
    {
        public MRUStrategy(CacheGeometry cacheGeometry)
            :base(cacheGeometry, Functor.Less<long>())
        {
        }
    }
}
