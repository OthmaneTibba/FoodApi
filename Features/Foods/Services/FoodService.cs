using FoodApi.Features.Foods.Services.Interfaces;
using FoodApi.Shared.Services.Interfaces;


namespace FoodApi.Features.Foods.Services
{
    public class FoodService(
        IHttpClientFactory clientFactory,
        ICacheService cacheService,
        ILogger<FoodService> logger
    ) : IFoodService
    {

        public async Task<Dictionary<string, dynamic>> SearchFoodByNameAsync(string foodName)
        {

            string searchKey = string.Empty;
            foreach (var key in cacheService.GetAllKeys())
            {
                if (key.Contains(foodName, StringComparison.OrdinalIgnoreCase))
                {
                    searchKey = key;
                    break;
                }
            }

            Dictionary<string, dynamic>? data = cacheService.GetData<Dictionary<string, dynamic>?>(searchKey);

            if (data is not null)
            {
                logger.LogInformation("data has been retrieved from the cache");
                return data;
            }

            using var foodClient = clientFactory.CreateClient("foodClient");
            var response = await foodClient
            .GetFromJsonAsync<Dictionary<string, dynamic>>($"foods/search/v3?search_expression={foodName}&format=json&include_sub_categories=false&flag_default_serving=true&max_results=1&language=en&region=US")
            ?? throw new Exception("food result is null");

            cacheService.SetData<Dictionary<string, dynamic>?>(foodName, response, DateTimeOffset.Now.AddMinutes(2));
            logger.LogInformation("data has been saved to the cache");
            return response;
        }
    }
}