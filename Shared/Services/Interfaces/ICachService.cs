namespace FoodApi.Shared.Services.Interfaces
{
    public interface ICacheService
    {
        public T GetData<T>(string key);
        public bool SetData<T>(string key, T data, DateTimeOffset expirationTime);
        public object RemoveData(string key);
        public List<string> GetAllKeys();
    }
}