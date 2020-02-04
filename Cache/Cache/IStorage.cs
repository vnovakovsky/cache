using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cache
{
    public interface IStorage <Key>
    {
        Byte[] ReadWord(Key key);
        void WriteWord(Key key, Byte[] array);
        List<Word> ReadLine(Key tag, int wordsInLine);
        bool EOF(Key key);
    }
}
