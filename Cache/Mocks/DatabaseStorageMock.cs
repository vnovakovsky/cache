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
            Value wordOut;
            if (database_.TryGetValue(key, out wordOut))
            {
                return Convert(wordOut);
            }

            return null;
        }

        public List<Word> ReadLine(Key tag, int wordsInLine)
        {
            List<Word> words = new List<Word>();
            int iTag = Util.ConvertToInt(tag);
            for (int i = iTag, n = 0; n < wordsInLine; ++i)
            {
                Key currentTag = (Key)System.Convert.ChangeType(i, typeof(Key));
                if (EOF(currentTag))
                    break;
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
            database_[key] = word;
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
            int result = key.CompareTo(database_.Keys.Max());
            if (result == 1)
                return true;
            else
                return false;
            //return 0 == key.CompareTo(database_.Keys.Max());
        }
        public Key MaxKey()
        {
            return database_.Keys.Max();
        }
    }
}
