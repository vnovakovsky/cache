using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Cache
{
    public static class Util
    {
        public static int ConvertToInt<Tag>(Tag tag)
        {
            return (int)System.Convert.ChangeType(tag, typeof(int));
        }
        public static byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
        // Convert a byte array to an Object
        public static Object ByteArrayToObject(byte[] arrBytes)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            ms.Write(arrBytes, 0, arrBytes.Length);
            ms.Seek(0, SeekOrigin.Begin);
            Object obj = (Object)binForm.Deserialize(ms);

            return obj;
        }
    }
    public static class Functor
    {
        public static Func<T, T, bool> Greater<T>()
            where T : IComparable<T>
        {
            return delegate (T lhs, T rhs) { return lhs.CompareTo(rhs) > 0; };
        }

        public static Func<T, T, bool> Less<T>()
            where T : IComparable<T>
        {
            return delegate (T lhs, T rhs) { return lhs.CompareTo(rhs) < 0; };
        }
    }
}
