using DevExpress.ExpressApp.EFCore.Updating;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.ExpressApp.Design;
using DevExpress.ExpressApp.EFCore.DesignTime;
using MySolution.Module.BusinessObjects;

namespace MyXafSolution.Module.BusinessObjects;

// This code allows our Model Editor to get relevant EF Core metadata at design time.
// For details, please refer to https://supportcenter.devexpress.com/ticket/details/t933891.
public class MyXafSolutionContextInitializer : DbContextTypesInfoInitializerBase {
	protected override DbContext CreateDbContext() {
		var optionsBuilder = new DbContextOptionsBuilder<MyXafSolutionEFCoreDbContext>()
            .UseSqlServer(";")
            .UseChangeTrackingProxies()
            .UseObjectSpaceLinkProxies();
        return new MyXafSolutionEFCoreDbContext(optionsBuilder.Options);
	}
}
//This factory creates DbContext for design-time services. For example, it is required for database migration.
public class MyXafSolutionDesignTimeDbContextFactory : IDesignTimeDbContextFactory<MyXafSolutionEFCoreDbContext> {
	public MyXafSolutionEFCoreDbContext CreateDbContext(string[] args) {
		//throw new InvalidOperationException("Make sure that the database connection string and connection provider are correct. After that, uncomment the code below and remove this exception.");
		var optionsBuilder = new DbContextOptionsBuilder<MyXafSolutionEFCoreDbContext>();
		optionsBuilder.UseSqlServer("Integrated Security=SSPI;Data Source=.;Initial Catalog=MyXafSolution;TrustServerCertificate=true");
		optionsBuilder.UseChangeTrackingProxies();
		optionsBuilder.UseObjectSpaceLinkProxies();
		return new MyXafSolutionEFCoreDbContext(optionsBuilder.Options);
	}
}
[TypesInfoInitializer(typeof(MyXafSolutionContextInitializer))]
public class MyXafSolutionEFCoreDbContext : DbContext {
	public MyXafSolutionEFCoreDbContext(DbContextOptions<MyXafSolutionEFCoreDbContext> options) : base(options) {
	}
    //public DbSet<ModuleInfo> ModulesInfo { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<DemoTask> DemoTasks { get; set; }
    public DbSet<ReportDataV2> ReportDataV2 { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasChangeTrackingStrategy(ChangeTrackingStrategy.ChangingAndChangedNotificationsWithOriginalValues);
        modelBuilder.UsePropertyAccessMode(PropertyAccessMode.PreferFieldDuringConstruction);
    }
}
