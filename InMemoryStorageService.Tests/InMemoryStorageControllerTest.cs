using NUnit.Framework;
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
        public void setValue(string key, string value)
        {
            SetParams parametes = new SetParams { key = key, value = value };
            controller.SetValueByKey(parametes);

            Assert.AreEqual(value, storage[key]);
        }


        [TestCase(null, "key-null")]
        public void setByNullKey(string key, string value)
        {
            SetParams parametes = new SetParams { key = key, value = value };
            var response = controller.SetValueByKey(parametes);

            Assert.IsInstanceOf<BadRequestObjectResult>(response);
            Assert.AreEqual($"Invalid key <{key}>", response.Value);
        }


        [Test]
        public void setAndGetValue()
        {
            string key = "test key";
            string value = "test value";
            SetParams parametes = new SetParams { key = key, value = value };

            controller.SetValueByKey(parametes);
            var response = controller.GetValueByKey(key);
            string resultValue = response?.GetType().GetProperty("value")?.GetValue(response);

            Assert.AreEqual(resultValue, value);
        }


        [Test]
        public void getByNotExistsKey()
        {
            string notExistsKey = "not exists key";

            var response = controller.GetValueByKey(notExistsKey);

            Assert.IsInstanceOf<NotFoundObjectResult>(response);
            Assert.AreEqual($"Key <{notExistsKey}> doesn't exists", response.Value);
        }


        [Test]
        public void removeValue()
        {
            string key = "test key";
            string value = "test value";
            SetParams parametes = new SetParams { key = key, value = value };

            controller.SetValueByKey(parametes);
            controller.RemoveValueByKey(key);

            var response = controller.GetValueByKey(key);
            string actualValue = response?.GetType().GetProperty("value")?.GetValue(response);
            Assert.IsNull(actualValue);
        }


        [Test]
        public void removeByNotExistsKey()
        {
            string notExistsKey = "not exists key";

            var response = controller.RemoveValueByKey(notExistsKey);

            Assert.IsInstanceOf<NotFoundObjectResult>(response);
            Assert.AreEqual($"Key <{notExistsKey}> doesn't exists", response.Value);
        }

        [Test]
        public void removeByNullKey()
        {
            string nullKey = null;

            var response = controller.RemoveValueByKey(nullKey);

            Assert.IsInstanceOf<BadRequestObjectResult>(response);
            Assert.AreEqual($"Invalid key <{nullKey}>", response.Value);
        }


        [Test]
        public void getAllValues()
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
                SetParams parametes = new SetParams { key = row.Key, value = row.Value };
                controller.SetValueByKey(parametes);
            }

            Dictionary<string, string> actualRows = controller.GetAll();
            Assert.AreEqual(actualRows, expectedRows);
        }


        [Test]
        public void getKeys()
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
                controller.SetValueByKey(new SetParams { key = row.Key, value = row.Value });
            }

            controller.SetValueByKey(new SetParams { key = "4", value = null });

            var response = controller.GetKeys();
            IEnumerable<string> actualKeys = response?.GetType().GetProperty("value")?.GetValue(response);

            Assert.That(actualKeys, Is.EquivalentTo(expectedKeys));
        }
    }
}
