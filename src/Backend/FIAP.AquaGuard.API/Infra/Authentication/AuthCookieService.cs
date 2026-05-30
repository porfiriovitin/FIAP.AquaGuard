namespace FIAP.AquaGuard.API.Infra.Authentication
{
    public class AuthCookieService : IAuthCookieService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthCookieService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Sets the access token in an HTTP-only cookie with appropriate security settings, such as Secure and SameSite attributes, and an expiration time of 15 minutes.
        /// </summary>
        public void SetAccessToken(string token)
        {
            var response = _httpContextAccessor.HttpContext?.Response;

            if (response is null)
                throw new InvalidOperationException("HTTP context is not available.");

            response.Cookies.Append("access_token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddMinutes(15),
                Path = "/"
            });
        }
    }
}
