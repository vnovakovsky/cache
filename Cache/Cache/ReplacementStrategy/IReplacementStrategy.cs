namespace Cache.ReplacementStrategy
{
    public interface IReplacementStrategy<Tag>
    {
        Set<Tag> SelectVictim(Tag tag);

        void SetRecentWord(Tag tag);
    }
}
