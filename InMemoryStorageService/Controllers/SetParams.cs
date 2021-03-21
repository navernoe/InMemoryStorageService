using InMemoryStorageService.Controllers.Interface;

namespace InMemoryStorageService.Controllers
{
    public class SetParams : ISetParams
    {
        public string key { get; set; }
        public string value { get; set; }
    }
}
