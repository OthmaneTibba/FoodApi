namespace FoodApi.Features.Foods.Services.Interfaces
{
    public interface IFoodService
    {
        public Task<Dictionary<string, dynamic>> SearchFoodByNameAsync(string foodName);
    }
}