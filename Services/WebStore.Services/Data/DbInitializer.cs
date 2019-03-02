using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebStore.DAL.Context;
using WebStore.Domain.Entities;

namespace WebStore.Services.Data
{
    public static class DbInitializer
    {
        public static void Initialize(this WebStoreContext context)
        {
            context.Database.EnsureCreated();

            if (context.Products.Any()) return;

            using (var transaction = context.Database.BeginTransaction())
            {
                foreach (var section in TestData.Sections)
                    context.Sections.Add(section);

                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Sections] ON");
                context.SaveChanges();
                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Sections] OFF");
                transaction.Commit();
            }

            using (var transaction = context.Database.BeginTransaction())
            {
                foreach (var brand in TestData.Brands)
                    context.Brands.Add(brand);

                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Brands] ON");
                context.SaveChanges();
                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Brands] OFF");
                transaction.Commit();
            }

            using (var transaction = context.Database.BeginTransaction())
            {
                foreach (var product in TestData.Products)
                    context.Products.Add(product);

                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Products] ON");
                context.SaveChanges();
                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [dbo].[Products] OFF");
                
//                using (context.Products.IdentityInsert()) context.SaveChanges();
                transaction.Commit();
            }
        }

        public static async Task InitializeIdentityAsync(this IServiceProvider services)
        {
            var role_manager = services.GetService<RoleManager<IdentityRole>>();
            if (!await role_manager.RoleExistsAsync(User.UserRole))
                await role_manager.CreateAsync(new IdentityRole(User.UserRole));

            if (!await role_manager.RoleExistsAsync(User.AdminRole))
                await role_manager.CreateAsync(new IdentityRole(User.AdminRole));

            var user_manager = services.GetService<UserManager<User>>();
            var user_store = services.GetService<IUserStore<User>>();

            if (await user_store.FindByNameAsync(User.AdminUser, CancellationToken.None) == null)
            {
                var admin = new User
                {
                      UserName = User.AdminUser,
                      Email = $"{User.AdminUser}@server.ru"
                };

                if ((await user_manager.CreateAsync(admin, "AdminPassword123@")).Succeeded)
                    await user_manager.AddToRoleAsync(admin, User.AdminRole);
            }

            if (await user_store.FindByNameAsync(User.TestUser, CancellationToken.None) == null)
            {
                var user = new User
                {
                    UserName = User.TestUser,
                    Email = $"{User.TestUser}@server.ru"
                };

                if ((await user_manager.CreateAsync(user, "TestUser123@")).Succeeded)
                    await user_manager.AddToRoleAsync(user, User.UserRole);
            }

        }
    }
}
