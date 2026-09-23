using Abp.Zero.EntityFrameworkCore;
using MyProject.Authorization.Roles;
using MyProject.Authorization.Users;
using MyProject.MultiTenancy;
using Microsoft.EntityFrameworkCore;
using MyProject.Admissions;

namespace MyProject.EntityFrameworkCore;

public class MyProjectDbContext : AbpZeroDbContext<Tenant, Role, User, MyProjectDbContext>
{
    /* Define a DbSet for each entity of the application */
    public DbSet<AdmissionPeriod> AdmissionPeriods { get; set; }
    public DbSet<AdmissionQuota> AdmissionQuotas { get; set; }
    public DbSet<AdmissionQuotaHistory> AdmissionQuotaHistories { get; set; }
    public MyProjectDbContext(DbContextOptions<MyProjectDbContext> options)
        : base(options)
    {
    }
}
