using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Globalization;

namespace ClientApp
{
    class DatabaseStorage<Key, Value> : Cache.IStorage<Key>
    {
        private Dictionary<Key, Value> database_;
        internal DatabaseStorage()
        {
            database_ = new Dictionary<Key, Value>();
        }
        public byte[] ReadWord(Key key)
        {
            throw new NotImplementedException();
        }

        public void WriteWord(Key key, byte[] binWord)
        {
            throw new NotImplementedException();
        }
        public List<Cache.Word> ReadLine(Key tag, int wordsInLine)
        {
            throw new NotImplementedException();
        }

        public bool EOF(Key key)
        {
            throw new NotImplementedException();
        }
    }
}
