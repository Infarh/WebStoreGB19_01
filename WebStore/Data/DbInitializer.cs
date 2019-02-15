using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Infrastucture;

namespace WebStore.Data
{
    internal static class DbInitializer
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

                using (context.Products.IdentityInsert()) context.SaveChanges();
                transaction.Commit();
            }
        }
    }
}
