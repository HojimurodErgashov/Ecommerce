using Ecommerce.V1.Users.Models;

namespace Ecommerce.V1.Users.Interfaces
{
    public interface IAuthService
    {
        ValueTask<UserModel> Registration(RegisterModel user);
        ValueTask<string> Login(LoginModel model);
    }
}
