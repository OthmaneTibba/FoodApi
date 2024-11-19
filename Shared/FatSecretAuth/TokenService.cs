

using System.Text;
using System.Text.Json;
using FoodApi.Config;
using Microsoft.Extensions.Options;

namespace FoodApi.Shared.FatSecretAuth
{
    public class TokenService(
        ILogger<TokenService> logger,
        IOptions<FatSecretSettings> options
    )
    {

        private readonly AuthToken _authToken = new();

        // Get the token, checking if it's expired
        public async Task<string> GetToken()
        {
            // Check if the token is expired
            if (IsTokenExpired())
            {
                // Fetch a new token if expired
                return await FetchNewToken();
            }

            // Check if access_token exists and is not empty
            if (!string.IsNullOrEmpty(_authToken.AccessToken))
            {

                return _authToken.AccessToken;
            }
            else
            {
                // If there's no valid token, fetch a new one
                return await FetchNewToken();

                // After fetching the new token, return it from session

            }

        }


        private async Task<string> FetchNewToken()
        {
            using var client = new HttpClient();
            var byteArray = Encoding.ASCII.GetBytes($"{options.Value.ClientId}:{options.Value.SecretId}");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            var values = new Dictionary<string, string>
                    {
                    { "grant_type", "client_credentials" }
                    };
            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync("https://oauth.fatsecret.com/connect/token", content);
            var responseString = await response.Content.ReadAsStringAsync();


            logger.LogInformation("response ==> {1}", responseString);
            Dictionary<string, dynamic>? res = JsonSerializer.Deserialize<Dictionary<string, dynamic>>(responseString)
            ?? throw new Exception("cannot seralize the respnse");




            object? accessToken = string.Empty;


            res.TryGetValue("access_token", out accessToken);


            if (accessToken is null)
                throw new Exception("token is null");

            _authToken.AccessToken = $"{accessToken}";
            _authToken.ExpiresAt = DateTime.UtcNow.AddSeconds(86400);

            return $"{accessToken}";

        }

        private bool IsTokenExpired()
        {
            if (_authToken.ExpiresAt is DateTime expiresAt)
            {
                // Check if the expiration time is not set to DateTime.MinValue
                if (expiresAt == DateTime.MinValue)
                {
                    return true; // Consider the token expired if the expiration is not set properly
                }

                // If the token is expired or within 5 minutes of expiration, return true
                return DateTime.UtcNow >= expiresAt.AddMinutes(-5);
            }
            return true; // If there's no valid expiration time, treat as expired
        }




    }
}