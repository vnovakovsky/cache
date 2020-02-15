using System.Collections.Generic;

namespace Cache.Set
{
    public interface ISet
    {
        byte[] FindWord(int tag, bool invalidate);
        void PutWord(int tag, List<Word> data);
    }
}