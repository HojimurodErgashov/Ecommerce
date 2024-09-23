using Ecommerce.V1.Users.Entities;
using User = Ecommerce.V1.Users.Entities.User;

namespace Ecommerce.V1.Users.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName {  get; set; }
        public string LastName { get; set; }
        public string PhoneNumber {  get; set; }

        public virtual UserModel MapFromEntity(User entity)
        {
            Id  = entity.Id;
            UserName = entity.UserName;
            Email = entity.Email;
            FirstName = entity.FirstName;
            LastName = entity.LastName;
            PhoneNumber = entity.PhoneNumber;
            return this;
        }
    }
}
