using System.Security.Claims;

namespace Ecommerce.V1.Commons.TokenGenerator.Interface
{
    public interface ITokenService
    {
        public string WriteToken(IEnumerable<Claim> claim);
    }
}
