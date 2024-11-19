
using System.Net.Http.Headers;

namespace FoodApi.Shared.FatSecretAuth
{
    public class TokenHandler(
        TokenService tokenService,
        ILogger<TokenHandler> logger
    ) : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await tokenService.GetToken();
            logger.LogInformation("Token is {1}", token);

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }

    }
}