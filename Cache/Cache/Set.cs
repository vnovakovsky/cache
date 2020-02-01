using CacheSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cache
{
    public class Set <Tag>
    {
        public Set(int numberOfLines, int wordsInLine, int wordSize)
        {
            SetProxy setProxy = new SetProxy(4, 2, 16);
        }
    }
}
