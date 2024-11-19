

using FoodApi.Features.Foods.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FoodApi.Features.Foods.Endpoints
{
    public static class FoodEndpoints
    {
        public static IEndpointRouteBuilder AddFoodsEndpoints(this IEndpointRouteBuilder routeBuilder)
        {
            var foodsApi = routeBuilder
            .MapGroup("/api/foods");

            foodsApi.MapGet("query", async (IFoodService foodService,
            [FromQuery] string foodName) => Results.Ok(await foodService.SearchFoodByNameAsync(foodName)));
            return routeBuilder;

        }
    }
}