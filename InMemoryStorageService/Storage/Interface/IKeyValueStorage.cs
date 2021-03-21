using System.Collections.Generic;

namespace InMemoryStorageService.Storage.Interface
{
    public interface IKeyValueStorage<TKey, TValue, TRow> : IEnumerable<TRow>
    {
        public TValue this[TKey key]
        {
            get;
            set;
        }

        public void remove(TKey key);
    }
}
