using Cache;
using Mocks;

namespace ClientApp
{
    static class DatabaseStorageFactory<Tag>
    {
        public static IStorage<Tag> Create()
        {
            return new DatabaseStorageMock<Tag, string>();
        }
    }
}
