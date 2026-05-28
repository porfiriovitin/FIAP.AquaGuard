namespace FIAP.AquaGuard.API.Infra.Authentication;

public interface IAuthCookieService
{
    void SetAccessToken(string token);
}
