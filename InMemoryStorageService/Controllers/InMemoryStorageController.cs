using System.Linq;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
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
        public Dictionary<string, string> GetAll()
        {
            return _storage.ToDictionary(row => row.Key, row => row.Value);
        }

        [HttpGet]
        [Route("keys")]
        public dynamic GetKeys()
        {
            IEnumerable<string> keys = _storage
                        .Where(row => !string.IsNullOrEmpty(row.Value))
                        .Select(row => row.Key);

            return new { value = keys };
        }

        [HttpGet]
        [Route("{key}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public dynamic GetValueByKey(string key)
        {
            try
            {
                string value = _storage[key];

                return new { value };
            }
            catch (InvalidStorageKeyException e)
            {
                return BadRequest(e.Message);
            }
            catch (NotExistsStorageKeyException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost]
        [Route("set")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ObjectResult SetValueByKey([FromBody] SetParams parameters)
        {
            string key = parameters.key;
            string value = parameters.value;

            try
            {
                _storage[key] = value;

                return Created(
                    WebUtility.UrlEncode($"/storage/{key}"),
                    new Dictionary<string,string> { [key]  = _storage[key] }
                );
            }
            catch (InvalidStorageKeyException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        [Route("remove/{key}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ObjectResult RemoveValueByKey(string key)
        {
            try
            {
                _storage.remove(key);

                return Ok($"Value for <{key}> key was removed successfully");
            }
            catch (InvalidStorageKeyException e)
            {
                return BadRequest(e.Message);
            }
            catch (NotExistsStorageKeyException e)
            {
                return NotFound(e.Message);
            }
        }
    }
}
