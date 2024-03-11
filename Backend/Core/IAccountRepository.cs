using Backend.Utils.Request;
using Backend.Utils.Response;

namespace Backend.Core;

public interface IAccountRepository
{
    AuthenticateResponse Authenticate(AuthenticateRequest model, string ipAddress);
    void Register(RegisterRequest model, string origin);
}