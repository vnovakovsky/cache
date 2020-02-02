using Cache.ReplacementStrategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cache
{
    public class CacheController<Tag> where Tag : unmanaged
    {
        public CacheController(IStorage<Tag> storage)
        {
        }
        public Word ReadWord(Tag tag)
        {
            throw new NotImplementedException();
        }
        public void WriteWord(Tag tag, Word word)
        {
            throw new NotImplementedException();
        }
        
        public IReplacementStrategy<Tag> ReplacementStrategy { get; set; } //Todo: set LRU as default
    }
}
