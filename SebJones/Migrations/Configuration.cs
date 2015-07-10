namespace SebJones.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using SebJones.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<SebJones.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "SebJones.Models.ApplicationDbContext";
        }

        protected override void Seed(SebJones.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            // Create users
            AddAdminUserAndRole(context);
            AddStandardUser(context);

            // Create Receipt Categories
            context.Categories.AddOrUpdate(r => r.Description,
                new Category { Description = "Sundries", ClaimVAT = false },
                new Category { Description = "Fuel", ClaimVAT = true },
                new Category { Description = "Public Travel - Air", ClaimVAT = true },
                new Category { Description = "Public Travel - Rail", ClaimVAT = true },
                new Category { Description = "Public Travel - Taxi", ClaimVAT = false },
                new Category { Description = "Accommodation", ClaimVAT = true },
                new Category { Description = "Entertainment / Gifts", ClaimVAT = false }
                );

            // Create Claim Statuses
            context.ClaimStatus.AddOrUpdate(r => r.Description,
                new ClaimStatus { Description = "Draft" },
                new ClaimStatus { Description = "Submitted" },
                new ClaimStatus { Description = "Rejected" },
                new ClaimStatus { Description = "Approved" },
                new ClaimStatus { Description = "Archived" }
                );

        }

        bool AddAdminUserAndRole(ApplicationDbContext context)
        {
            // Create admin role in db
            IdentityResult ir;
            var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            ir = rm.Create(new IdentityRole("admin"));

            // Create an admin user
            var hasher = new PasswordHasher();
            var users = new List<ApplicationUser>()
            {
                new ApplicationUser {
                    PasswordHash = hasher.HashPassword("Pa$$w0rd"),
                    UserName = "admin@expensemanager.com"
                }
            };

            var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            foreach (var user in users)
            {
                if (um.FindByName(user.UserName) == null)
                {
                    context.Users.AddOrUpdate(user);
                    um.AddToRole(user.Id, "admin");
                }
            }

            return true;
        }

        bool AddStandardUser(ApplicationDbContext context)
        {
            var hasher = new PasswordHasher();
            var users = new List<ApplicationUser>()
            {
                new ApplicationUser {
                    PasswordHash = hasher.HashPassword("Pa$$w0rd"),
                    UserName = "user@expensemanager.com"
                }
            };

            var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            foreach (var user in users)
            {
                if (um.FindByName(user.UserName) == null)
                {
                    context.Users.AddOrUpdate(user);
                }
            }

            return true;
        }
    }
}
