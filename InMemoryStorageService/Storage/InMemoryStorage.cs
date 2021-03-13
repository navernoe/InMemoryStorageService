using System.Collections.Generic;
using InMemoryStorageService.Exceptions;
using System.Collections;

namespace InMemoryStorageService.Storage
{
    public class InMemoryStorage: IEnumerable<RowModel>
    {
        public Dictionary<string, string> data = new Dictionary<string, string>();

        public dynamic this[string key]
        {
            get
            {
                if (isKeyExists(key))
                {
                    return data[key];
                }
                else
                {
                    throw new InvalidStorageKeyException($"Invalid key <{key}>");
                }
            }

            set
            {
                if (isKeyExists(key))
                {
                    data[key] = value;
                }
                else
                {
                    data.Add(key, value);
                }
            }
        }

        public void remove(string key)
        {
            if (isKeyExists(key))
            {
                data[key] = null;
            } else
            {
                throw new InvalidStorageKeyException($"Invalid key <{key}>");
            }
        }

        private void validate(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new InvalidStorageKeyException($"Invalid key <{key}>");
            }
        }

        private bool isKeyExists(string key)
        {
            validate(key);

            return data.ContainsKey(key);
        }

        public IEnumerator<RowModel> GetEnumerator()
        {
            foreach(KeyValuePair<string, string> pair in data)
            {
                yield return new RowModel(pair.Key, pair.Value);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
