namespace Cache.ReplacementStrategy
{
    public interface IReplacementStrategy<Tag> where Tag : unmanaged
    {
        int SelectVictim(Tag tag);

        void SetRecentLine(Tag tag, int setIndex);
    }
}
