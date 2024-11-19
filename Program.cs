using FoodApi.Config;
using FoodApi.Extensions;
using FoodApi.Features.Foods.Endpoints;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();
builder.Services.AddAppServices();
builder.Services.Configure<FatSecretSettings>(builder.Configuration.GetSection("FatSecretsSettings"));
builder.Services.AddCors((config) =>
{
    config.AddDefaultPolicy((pb) =>
    {
        pb.WithOrigins("http://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors();

app.AddFoodsEndpoints();


app.Run();

