using EmailPostOffice.MailQueues;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.IdentityServer.EntityFrameworkCore;
using Volo.Abp.LanguageManagement.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TextTemplateManagement.EntityFrameworkCore;
using Volo.Saas.EntityFrameworkCore;
using Volo.Saas.Editions;
using Volo.Saas.Tenants;
using Volo.Abp.Gdpr;

namespace EmailPostOffice.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityProDbContext))]
[ReplaceDbContext(typeof(ISaasDbContext))]
[ConnectionStringName("Default")]
public class EmailPostOfficeDbContext :
    AbpDbContext<EmailPostOfficeDbContext>,
    IIdentityProDbContext,
    ISaasDbContext
{
    public DbSet<MailQueue> MailQueues { get; set; }
    /* Add DbSet properties for your Aggregate Roots / Entities here. */

    #region Entities from the modules

    /* Notice: We only implemented IIdentityProDbContext and ISaasDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityProDbContext and ISaasDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    // Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }

    // SaaS
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<Edition> Editions { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion

    public EmailPostOfficeDbContext(DbContextOptions<EmailPostOfficeDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentityPro();
        builder.ConfigureIdentityServer();
        builder.ConfigureFeatureManagement();
        builder.ConfigureLanguageManagement();
        builder.ConfigureSaas();
        builder.ConfigureTextTemplateManagement();
        builder.ConfigureBlobStoring();
        builder.ConfigureGdpr();

        /* Configure your own tables/entities inside here */

        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(EmailPostOfficeConsts.DbTablePrefix + "YourEntities", EmailPostOfficeConsts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});
        builder.Entity<MailQueue>(b =>
    {
        b.ToTable(EmailPostOfficeConsts.DbTablePrefix + "MailQueues", EmailPostOfficeConsts.DbSchema);
        b.ConfigureByConvention();
        b.Property(x => x.TenantId).HasColumnName(nameof(MailQueue.TenantId));
        b.Property(x => x.Recipient).HasColumnName(nameof(MailQueue.Recipient)).IsRequired().HasMaxLength(MailQueueConsts.RecipientMaxLength);
        b.Property(x => x.RecipientName).HasColumnName(nameof(MailQueue.RecipientName)).HasMaxLength(MailQueueConsts.RecipientNameMaxLength);
        b.Property(x => x.Sender).HasColumnName(nameof(MailQueue.Sender)).IsRequired().HasMaxLength(MailQueueConsts.SenderMaxLength);
        b.Property(x => x.SenderName).HasColumnName(nameof(MailQueue.SenderName)).HasMaxLength(MailQueueConsts.SenderNameMaxLength);
        b.Property(x => x.Subject).HasColumnName(nameof(MailQueue.Subject)).IsRequired().HasMaxLength(MailQueueConsts.SubjectMaxLength);
        b.Property(x => x.Content).HasColumnName(nameof(MailQueue.Content)).IsRequired();
        b.Property(x => x.Retry).HasColumnName(nameof(MailQueue.Retry));
        b.Property(x => x.Success).HasColumnName(nameof(MailQueue.Success));
        b.Property(x => x.Suspend).HasColumnName(nameof(MailQueue.Suspend));
        b.Property(x => x.FreezeTime).HasColumnName(nameof(MailQueue.FreezeTime));
    });
    }
}