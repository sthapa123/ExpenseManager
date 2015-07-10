using Microsoft.AspNet.Identity.EntityFramework;

namespace SebJones.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public System.Data.Entity.DbSet<SebJones.Models.Category> Categories { get; set; }

        public System.Data.Entity.DbSet<SebJones.Models.ClaimStatus> ClaimStatus { get; set; }

        public System.Data.Entity.DbSet<SebJones.Models.ClaimComments> ClaimComments { get; set; }

        public System.Data.Entity.DbSet<SebJones.Models.Receipt> Receipts { get; set; }

        public System.Data.Entity.DbSet<SebJones.Models.Claim> Claims { get; set; }
    }
}