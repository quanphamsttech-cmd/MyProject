using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MyProject.Configuration;
using MyProject.Web;

namespace MyProject.EntityFrameworkCore
{
    public class MyProjectDbContextFactory : IDesignTimeDbContextFactory<MyProjectDbContext>
    {
        public MyProjectDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<MyProjectDbContext>();

            var configuration = AppConfigurations.Get(
                WebContentDirectoryFinder.CalculateContentRootFolder()
            );

            MyProjectDbContextConfigurer.Configure(
                builder,
                configuration.GetConnectionString(MyProjectConsts.ConnectionStringName)
            );

            return new MyProjectDbContext(builder.Options);
        }
    }
}