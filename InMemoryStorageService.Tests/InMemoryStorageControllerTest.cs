using NUnit.Framework;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using InMemoryStorageService.Controllers;
using InMemoryStorageService.Storage;

namespace InMemoryStorageService.Tests
{
    public class InMemoryStorageControllerTest
    {
        private InMemoryStorageController controller;
        private InMemoryStorage storage;

        [SetUp]
        public void Setup()
        {
            storage = new InMemoryStorage();
            controller = new InMemoryStorageController(storage);
        }

        [TestCase("key", "112233")]
        [TestCase("key with spaces", "hello world")]
        public async Task setValue(string key, string value)
        {
            await controller.SetValueByKey(key, value);

            Assert.AreEqual(value, storage[key]);
        }


        [TestCase(null, "key-null")]
        public async Task setByNullKey(string key, string value)
        {
            var response = await controller.SetValueByKey(key, value);

            Assert.IsInstanceOf<BadRequestObjectResult>(response);
            Assert.AreEqual($"Invalid key <{key}>", response.Value);
        }

        [Test]
        public async Task setAndGetValue()
        {
            string key = "test key";
            string value = "test value";

            await controller.SetValueByKey(key, value);
            var response = await controller.GetValueByKey(key);
            string resultValue = response?.GetType().GetProperty("value")?.GetValue(response);

            Assert.AreEqual(resultValue, value);
        }


        [Test]
        public async Task getByNotExistsKey()
        {
            string notExistsKey = "not exists key";

            var response = await controller.GetValueByKey(notExistsKey);

            Assert.IsInstanceOf<BadRequestObjectResult>(response);
            Assert.AreEqual($"Invalid key <{notExistsKey}>", response.Value);
        }

        [Test]
        public async Task removeValue()
        {
            string key = "test key";
            string value = "test value";

            await controller.SetValueByKey(key, value);
            await controller.RemoveValueByKey(key);

            var response = await controller.GetValueByKey(key);
            string actualValue = response?.GetType().GetProperty("value")?.GetValue(response);
            Assert.IsNull(actualValue);
        }

        [Test]
        public async Task removeNotExistsValue()
        {
            string notExistsKey = "not exists key";

            var response = await controller.RemoveValueByKey(notExistsKey);
            Assert.IsInstanceOf<BadRequestObjectResult>(response);
            Assert.AreEqual($"Invalid key <{notExistsKey}>", response.Value);
        }

        [Test]
        public async Task getAllValues()
        {
            Dictionary<string, string> rows = new Dictionary<string, string>
            {
                ["1"] = "test value1",
                ["2"] = "test value2",
                ["3"] = "test value3",
                ["4"] = null
            };


            IEnumerable<string> expectedKeys = from row in rows select row.Key;
            IEnumerable<string> expectedValues = from row in rows select row.Value;

            foreach (var row in rows)
            {
                await controller.SetValueByKey(row.Key, row.Value);
            }

            Dictionary<string, string> actualRows = await controller.GetAll();
            IEnumerable<string> actualKeys = from row in actualRows select row.Key;
            IEnumerable<string> actualValues = from row in actualRows select row.Value;

            Assert.That(actualKeys, Is.EqualTo(expectedKeys));
            Assert.That(actualValues, Is.EqualTo(expectedValues));
        }

        [Test]
        public async Task getKeys()
        {
            Dictionary<string, string> rows = new Dictionary<string, string>
            {
                ["1"] = "test value1",
                ["2"] = "test value2",
                ["3"] = "test value3"
            };

            IEnumerable<string> expectedKeys = from row in rows select row.Key;

            foreach (var row in rows)
            {
                await controller.SetValueByKey(row.Key, row.Value);
            }

            await controller.SetValueByKey("4", null);

            var response = await controller.GetKeys();
            IEnumerable<string> actualKeys = response?.GetType().GetProperty("value")?.GetValue(response);

            Assert.That(actualKeys, Is.EqualTo(expectedKeys));
        }
    }
}
