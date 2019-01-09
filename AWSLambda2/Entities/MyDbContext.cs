using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AWSLambda2.Entities
{
    public class MyDbContext: DbContext
    {
        public const string DefaultConnectionStringName = nameof(MyDbContext);

        public MyDbContext(DbContextOptions<MyDbContext> options): base(options)
        { }

        public DbSet<User> Users { get; set; }
    }

    public class DesignTimeMyDbContext : IDesignTimeDbContextFactory<MyDbContext>
    {
        public MyDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("connectionStrings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<MyDbContext>()
                .UseMySql(configuration.GetConnectionString(MyDbContext.DefaultConnectionStringName)
            );
            return new MyDbContext(builder.Options);
        }
    }
}
