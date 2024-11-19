

namespace FoodApi
{
    public class AuthToken
    {
        private string _accessToken = string.Empty;
        private DateTime _expiresAt = DateTime.MinValue;
        public string AccessToken { get => _accessToken; set { _accessToken = value; } }

        public DateTime ExpiresAt { get => _expiresAt; set { _expiresAt = value; } }

    }
}