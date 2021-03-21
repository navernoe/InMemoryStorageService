using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;
using InMemoryStorageService.Storage;
using InMemoryStorageService.Exceptions;

namespace InMemoryStorageService.Tests
{
    public class InMemoryStorageTest
    {
        private InMemoryStorage storage;

        [SetUp]
        public void Setup()
        {
            storage = new InMemoryStorage();
        }


        [TestCase("a", "22")]
        [TestCase("3", "sometext")]
        [TestCase("12xy", "value with spaces")]
        [TestCase("key with spaces", "hello world")]
        public void setAndGetValue(string key, string value)
        {
            storage[key] = value;

            Assert.AreEqual(value, storage[key]);
        }


        [TestCase(null, "set value without key")]
        public void setValueNegative(string key, string value)
        {
            Assert.Throws<InvalidStorageKeyException>(() => storage[key] = value);
        }


        [TestCase("not existed storage key")]
        public void getValueByNotExistsKey(string key)
        {
            var test = "";
            Assert.Throws<NotExistsStorageKeyException>(() => test = storage[key]);
        }

        [TestCase(null)]
        public void getValueByNullKey(string key)
        {
            var test = "";
            Assert.Throws<InvalidStorageKeyException>(() => test = storage[key]);
        }



        [TestCase("a")]
        [TestCase("12")]
        public void removeValue(string key)
        {
            storage[key] = "test value";
            storage.remove(key);

            var test = "";
            Assert.Throws<NotExistsStorageKeyException>(() => test = storage[key]);
        }


        [Test]
        public void removeValueNegative()
        {
            Assert.Throws<NotExistsStorageKeyException>(
                () => storage.remove("not existed key")
             );
        }


        [Test]
        public void getAllRows()
        {
            Dictionary<string, string> expectedRows = new Dictionary<string, string>
            {
                 ["1"] = "test value1",
                 ["2"] = "test value2",
                 ["3"] = "test value3",
                 ["4"] = null
            };

            foreach (var row in expectedRows)
            {
                storage[row.Key] = row.Value;
            }

            Assert.AreEqual(
                expectedRows,
                storage.ToDictionary(row => row.Key, row => row.Value)
            );
        }

    }
}