using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Globalization;
using Cache;
using System.Linq;

namespace Mocks
{
    class DatabaseStorageMock<Key, Value> : IStorage<Key> where Key : IComparable// where W : new() 
    {
        public Dictionary<Key, Value> database_;
        public DatabaseStorageMock()
        {
            database_ = new Dictionary<Key, Value>();
        }
        public byte[] ReadWord(Key key)
        {
            Value word = database_[key];
            return Convert(word);
        }

        public List<Word> ReadLine(Key tag, int wordsInLine)
        {
            List<Word> words = new List<Word>();
            int iTag = Util.ConvertToInt(tag);
            bool eof = false;
            for (int i = iTag, n = 0; !eof && n < wordsInLine; ++i)
            {
                Key currentTag = (Key)System.Convert.ChangeType(i, typeof(Key));
                eof = EOF(currentTag);
                byte[] storageBytes = ReadWord(currentTag);
                if (storageBytes != null)
                {
                    Word storageWord = new Word(i, storageBytes, -1);
                    words.Add(storageWord);
                    ++n;
                }
            }
            return words;
        }

        public void WriteWord(Key key, byte[] binWord)
        {
            Value word = Convert(binWord);
            database_.Add(key, word);
        }

        public static Value Convert(byte[] binWord)
        {
            string s = Encoding.ASCII.GetString(binWord);
            return Convert(s);
        }
        public static Value Convert(string word)
        {
            TypeConverter converter =
                TypeDescriptor.GetConverter(typeof(Value));

            return (Value)converter.ConvertFromString(null,
                CultureInfo.InvariantCulture, word);
        }
        public static byte[] Convert(Value word)
        {
            TypeConverter converter =
                TypeDescriptor.GetConverter(typeof(Value));

            string s = (string)converter.ConvertFrom(null,
                CultureInfo.InvariantCulture, word);
            return Encoding.ASCII.GetBytes(s);
        }
        public bool EOF(Key key)
        {
            return 0 == key.CompareTo(database_.Keys.Max());
        }
        public Key MaxKey()
        {
            return database_.Keys.Max();
        }
    }
}
