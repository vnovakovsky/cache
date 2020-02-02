namespace Cache.ReplacementStrategy
{
    public interface IReplacementStrategy<Tag> where Tag : unmanaged
    {
        Set<Tag> SelectVictim(Tag tag);

        void SetRecentWord(Tag tag);
    }
}
