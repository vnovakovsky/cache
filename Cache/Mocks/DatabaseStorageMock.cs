using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Globalization;
using Cache;

namespace Mocks
{
    class DatabaseStorageMock<Key, Word> : IStorage<Key>
    {
        public Dictionary<Key, Word> database_;
        public DatabaseStorageMock()
        {
            database_ = new Dictionary<Key, Word>();
        }
        public byte[] ReadWord(Key key)
        {
            Word w = database_[key];
            return Convert(w);
        }

        public void WriteWord(Key key, byte[] binWord)
        {
            Word word = Convert(binWord);
            database_.Add(key, word);
        }

        public static Word Convert(byte[] binWord)
        {
            string s = Encoding.ASCII.GetString(binWord);
            return Convert(s);
        }
        public static Word Convert(string word)
        {
            TypeConverter converter =
                TypeDescriptor.GetConverter(typeof(Word));

            return (Word)converter.ConvertFromString(null,
                CultureInfo.InvariantCulture, word);
        }
        public static byte[] Convert(Word word)
        {
            TypeConverter converter =
                TypeDescriptor.GetConverter(typeof(Word));

            string s = (string)converter.ConvertFrom(null,
                CultureInfo.InvariantCulture, word);
            return Encoding.ASCII.GetBytes(s);
        }
    }
}
