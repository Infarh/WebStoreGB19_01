using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;

namespace WebStore.Infrastucture
{
    internal static class DbSetExtensions
    {
        private class DbSetIdentityInsert<T> : IDisposable where T : class
        {
            private readonly string _TableName;
            private readonly DbContext _Context;

            public DbSetIdentityInsert(DbSet<T> Set)
            {
                _Context = Set.GetContext();
                var model = _Context.Model;
                var entity_types = model.GetEntityTypes();
                var entity_type = entity_types.First(type => type.ClrType == typeof(T));
                var table_name_annotation = entity_type.GetAnnotation("Relational:TableName");
                _TableName = table_name_annotation.Value.ToString();
                var cmd = $"SET IDENTITY_INSERT [dbo].[{_TableName}] ON";
                _Context.Database.ExecuteSqlCommand(cmd);
            }

            public void Dispose()
            {
                var cmd = $"SET IDENTITY_INSERT [dbo].[{_TableName}] OFF";
                _Context.Database.ExecuteSqlCommand(cmd);
            }
        }


        public static IDisposable IdentityInsert<T>(this DbSet<T> Set) where T : class => new DbSetIdentityInsert<T>(Set);

        public static DbContext GetContext<T>(this DbSet<T> Set)
            where T : class
        {
            var infrastructure = (IInfrastructure<IServiceProvider>) Set;
            var service_provider = infrastructure.Instance;
            var context_service = (ICurrentDbContext) service_provider.GetService(typeof(ICurrentDbContext));
            return context_service.Context;
        }
    }
}
