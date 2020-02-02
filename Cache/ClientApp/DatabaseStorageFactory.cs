using Cache;
using Mocks;

namespace ClientApp
{
    static class DatabaseStorageFactory<Tag> where Tag : unmanaged
    {
        public static IStorage<Tag> Create()
        {
            /* the purpose of this factory is to check 
                whether it's possible to convert Tag to int in run-time
            */
            Tag tag = default;
            int obj = Util.ConvertToInt(tag);

            return new DatabaseStorageMock<Tag, string>();
        }
    }
}
