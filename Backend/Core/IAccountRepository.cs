using Backend.Utils.Request;
using Backend.Utils.Response;

namespace Backend.Core;

public interface IAccountRepository
{
    AuthenticateResponse Authenticate(AuthenticateRequest model, string ipAddress);
    AuthenticateResponse RefreshToken(string token, string ipAddress);
    void RevokeToken(string token, string ipAddress);
    void Register(RegisterRequest model, string origin);
}