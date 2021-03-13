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

        [TestCase(null)]
        [TestCase("not existed storage key")]
        public void getValueNegative(string key)
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

            Assert.IsNull(storage[key]);
        }

        [Test]
        public void removeValueNegative()
        {
            Assert.Throws<InvalidStorageKeyException>(
                () => storage.remove("not existed key")
             );
        }

        [Test]
        public void getAllRows()
        {
            RowModel[] expectedRows = new RowModel[]
            {
                 new RowModel("1", "test value1"),
                 new RowModel("2", "test value2"),
                 new RowModel("3", "test value3"),
                 new RowModel("4", null),
            };

            IEnumerable<string> expectedKeys = from row in expectedRows select row.key;
            IEnumerable<string> expectedValues = from row in expectedRows select row.value;

            foreach (RowModel row in expectedRows)
            {
                storage[row.key] = row.value;
            }

            IEnumerable<string> actualKeys = from row in storage select row.key;
            IEnumerable<string> actualValues = from row in storage select row.value;

            Assert.That(actualKeys, Is.EqualTo(expectedKeys));
            Assert.That(actualValues, Is.EqualTo(expectedValues));
        }

    }
}