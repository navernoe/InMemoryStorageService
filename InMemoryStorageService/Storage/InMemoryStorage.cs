using System;
using System.Collections.Generic;
using InMemoryStorageService.Exceptions;
using System.Collections;
using System.Collections.Concurrent;
using InMemoryStorageService.Storage.Interface;

namespace InMemoryStorageService.Storage
{
    public class InMemoryStorage: IKeyValueStorage<string, string, KeyValuePair<string, string>>
    {
        private ConcurrentDictionary<string, string> data = new ConcurrentDictionary<string, string>();

        public string this[string key]
        {
            get
            {
                if (isKeyExists(key))
                {
                    return data[key];
                }
                else
                {
                    throw new NotExistsStorageKeyException($"Key <{key}> doesn't exists");
                }
            }

            set
            {
                validate(key);
                data[key] = value;
            }
        }

        public void remove(string key)
        {
            if (isKeyExists(key))
            {
                ((IDictionary)data).Remove(key);
            } else
            {
                throw new NotExistsStorageKeyException($"Key <{key}> doesn't exists");
            }
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            foreach (var pair in data)
            {
                yield return pair;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private bool isKeyExists(string key)
        {
            validate(key);

            return data.ContainsKey(key);
        }

        private void validate(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new InvalidStorageKeyException($"Invalid key <{key}>");
            }
        }

    }
}
