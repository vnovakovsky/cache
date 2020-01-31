using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Globalization;

namespace ClientApp
{
    class DatabaseStorage<Key, Word> : Cache.IStorage<Key>
    {
        private Dictionary<Key, Word> database_;
        internal DatabaseStorage()
        {
            database_ = new Dictionary<Key, Word>();
        }
        public byte[] ReadWord(Key key)
        {
            throw new NotImplementedException();
        }

        public void WriteWord(Key key, byte[] binWord)
        {
            throw new NotImplementedException();
        }
    }
}
