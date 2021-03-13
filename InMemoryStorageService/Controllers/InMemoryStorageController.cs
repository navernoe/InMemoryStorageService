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
        public async Task<IEnumerable<RowModel>> GetAll()
        {
            return await Task.Run(
                () => from row in _storage
                        where !string.IsNullOrEmpty(row.value)
                        select row
            );
        }

        [HttpGet]
        public async Task<dynamic> GetValueByKey([FromQuery] string key)
        {
            try
            {
                string value = await Task.Run(() => _storage[key]); 

                return value;
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

                return Ok($"Для ключа <{key}> установлено значение <{value}>");
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

                return Ok($"Значение ключа <{key}> успешно удалено");
            }
            catch (InvalidStorageKeyException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
