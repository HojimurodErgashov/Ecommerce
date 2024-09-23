using Microsoft.AspNetCore.Identity;

namespace Ecommerce.V1.Users.Entities
{
    public class User:IdentityUser<int>
    {
        public string FirstName {  get; set; }
        public string LastName { get; set; }
    }
}
