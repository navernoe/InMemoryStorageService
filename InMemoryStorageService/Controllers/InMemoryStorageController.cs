using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InMemoryStorageService.Exceptions;
using InMemoryStorageService.Storage;

namespace InMemoryStorageService.Controllers
{
    [ApiController]
    [Route("storage")]
    public class InMemoryStorageController : ControllerBase
    {
        private readonly InMemoryStorage _storage;

        public InMemoryStorageController(
            InMemoryStorage storage
        )
        {
            _storage = storage;
        }

        [HttpGet]
        [Route("all")]
        public async Task<Dictionary<string, string>> GetAll()
        {
            return await Task.Run(
                () => _storage
                        .ToDictionary(row => row.Key, row => row.Value)
            );
        }

        [HttpGet]
        [Route("keys")]
        public async Task<dynamic> GetKeys()
        {
            IEnumerable<string> keys = await Task.Run(
                () => _storage
                        .Where(row => !string.IsNullOrEmpty(row.Value))
                        .Select(row => row.Key)
            );

            return new { value = keys };
        }

        [HttpGet]
        public async Task<dynamic> GetValueByKey([FromQuery] string key)
        {
            try
            {
                string value = await Task.Run(() =>  _storage[key]);

                return new { value };
            }
            catch(InvalidStorageKeyException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("set")]
        public async Task<ObjectResult> SetValueByKey(string key, string value)
        {
            try
            {
                await Task.Run(() => _storage[key] = value);

                return Ok($"Key <{key}> was set to <{value}> value successfully");
            }
            catch (InvalidStorageKeyException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("remove")]
        public async Task<ObjectResult> RemoveValueByKey(string key)
        {
            try
            {
                await Task.Run(() => _storage.remove(key));

                return Ok($"Value for <{key}> key was removed successfully");
            }
            catch (InvalidStorageKeyException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
