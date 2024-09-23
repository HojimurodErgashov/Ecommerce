using Ecommerce.V1.Roles.Entities;
using Ecommerce.V1.Users.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.V1.Commons.EcommmerceContext
{
    public class EcommerceDbContext:IdentityDbContext<User ,Role ,  int>
    {
        public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options):base(options){ }

        public DbSet<User> Users {  get; set; }

        public DbSet<Role> Roles {  get; set; }
    }
}
