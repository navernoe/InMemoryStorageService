namespace InMemoryStorageService.Storage
{
    public class RowModel
    {
        public string key { get; set; }
        public string value { get; set; }

        public RowModel(string key, string value)
        {
            this.key = key;
            this.value = value;
        }
    }
}
