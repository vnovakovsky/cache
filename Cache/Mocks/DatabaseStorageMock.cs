using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Globalization;
using Cache;

namespace Mocks
{
    class DatabaseStorageMock<Key, Value> : IStorage<Key>// where W : new() 
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
            for (int i = iTag; i < wordsInLine; ++i)
            {
                byte[] storageBytes = ReadWord(tag);
                Word storageWord = new Word(i, storageBytes, -1);
                words.Add(storageWord);
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
    }
}
