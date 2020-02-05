

namespace Cache.ReplacementStrategy
{
    public class LRUStrategy<Tag> : RecentlyUsedStrategy<Tag> where Tag : unmanaged
    {
        public LRUStrategy(CacheGeometry cacheGeometry)
            : base(cacheGeometry, Functor.Less<long>())
        {
        }
    }
}
