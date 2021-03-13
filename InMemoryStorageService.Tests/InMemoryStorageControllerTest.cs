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
            Assert.AreEqual($"Неверное значение ключа <{key}>", response.Value);
        }

        [Test]
        public async Task setAndGetValue()
        {
            string key = "test key";
            string value = "test value";

            await controller.SetValueByKey(key, value);
            string resultValue = await controller.GetValueByKey(key);

            Assert.AreEqual(resultValue, value);
        }


        [Test]
        public async Task getByNotExistsKey()
        {
            string notExistsKey = "not exists key";

            var response = await controller.GetValueByKey(notExistsKey);

            Assert.IsInstanceOf<BadRequestObjectResult>(response);
            Assert.AreEqual($"Неверное значение ключа <{notExistsKey}>", response.Value);
        }

        [Test]
        public async Task removeValue()
        {
            string key = "test key";
            string value = "test value";

            await controller.SetValueByKey(key, value);
            await controller.RemoveValueByKey(key);

            string actualValue = await controller.GetValueByKey(key);
            Assert.IsNull(actualValue);
        }

        [Test]
        public async Task removeNotExistsValue()
        {
            string notExistsKey = "not exists key";

            var response = await controller.RemoveValueByKey(notExistsKey);
            Assert.IsInstanceOf<BadRequestObjectResult>(response);
            Assert.AreEqual($"Неверное значение ключа <{notExistsKey}>", response.Value);
        }

        [Test]
        public async Task getAllValues()
        {
            List<RowModel> expectedRows = new List<RowModel>()
            {
                 new RowModel("1", "test value1"),
                 new RowModel("2", "test value2"),
                 new RowModel("3", "test value3")
            };

            IEnumerable<string> expectedKeys = from row in expectedRows select row.key;
            IEnumerable<string> expectedValues = from row in expectedRows select row.value;


            foreach (RowModel row in expectedRows)
            {
                await controller.SetValueByKey(row.key, row.value);
            }

            await controller.SetValueByKey("4", null);

            IEnumerable<RowModel> actualRows = await controller.GetAll();
            IEnumerable<string> actualKeys = from row in actualRows select row.key;
            IEnumerable<string> actualValues = from row in actualRows select row.value;

            Assert.That(actualKeys, Is.EqualTo(expectedKeys));
            Assert.That(actualValues, Is.EqualTo(expectedValues));
        }
    }
}
