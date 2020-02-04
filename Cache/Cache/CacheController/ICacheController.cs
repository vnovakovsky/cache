using Cache.ReplacementStrategy;

namespace Cache.CacheController
{
    public interface ICacheController<Tag> where Tag : unmanaged
    {
        IReplacementStrategy<Tag> ReplacementStrategy { get; set; }

        Word ReadWord(Tag tag);
        void WriteWord(Tag tag, Word word);
    }
}