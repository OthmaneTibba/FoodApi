using FoodApi.Features.Foods.Services;
using FoodApi.Features.Foods.Services.Interfaces;
using FoodApi.Shared.FatSecretAuth;
using FoodApi.Shared.Services;
using FoodApi.Shared.Services.Interfaces;

namespace FoodApi.Extensions
{
    public static class AppExtensions
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {

            services.AddTransient<TokenHandler>();
            services
            .AddHttpClient("foodClient", (sp) => sp.BaseAddress = new Uri("https://platform.fatsecret.com/rest/"))
            .AddHttpMessageHandler<TokenHandler>();
            services.AddSingleton<TokenService>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IFoodService, FoodService>();

            return services;
        }
    }
}