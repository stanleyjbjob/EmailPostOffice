using System.IO;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EmailPostOffice.EntityFrameworkCore;
using EmailPostOffice.Localization;
using EmailPostOffice.MultiTenancy;
using EmailPostOffice.Permissions;
using EmailPostOffice.Web.Menus;
using Microsoft.OpenApi.Models;
using Volo.Abp;
using Volo.Abp.Account.Admin.Web;
using Volo.Abp.Account.Public.Web;
using Volo.Abp.Account.Public.Web.ExternalProviders;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Commercial;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Lepton;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AuditLogging.Web;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity.Web;
using Volo.Abp.IdentityServer.Web;
using Volo.Abp.LanguageManagement;
using Volo.Abp.LeptonTheme.Management;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.Web;
using Volo.Abp.TextTemplateManagement.Web;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.UI;
using Volo.Abp.UI.Navigation;
using Volo.Abp.VirtualFileSystem;
using Volo.Saas.Host;
using System;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Authentication.Twitter;
using Microsoft.AspNetCore.Extensions.DependencyInjection;
using EmailPostOffice.Web.HealthChecks;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Lepton.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Identity;
using Volo.Abp.Swashbuckle;
using Volo.Abp.Gdpr.Web;
using Volo.Abp.Account;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AuditLogging;
using Volo.Abp.Gdpr;
using Volo.Abp.IdentityServer.ApiResource.Dtos;
using Volo.Abp.IdentityServer.ApiScope.Dtos;
using Volo.Abp.IdentityServer.Client.Dtos;
using Volo.Abp.IdentityServer.IdentityResource.Dtos;
using Volo.Abp.LanguageManagement.Dto;
using Volo.Abp.TextTemplateManagement.TextTemplates;
using Volo.Abp.Users;
using Volo.Saas.Host.Dtos;
using EmailPostOffice.MailQueues;
using Volo.Abp.Content;
using static EmailPostOffice.Controllers.TestJbhrController;
using Microsoft.Extensions.Logging.Console;
using Serilog;
using Autofac.Core;

namespace EmailPostOffice.Web;

[DependsOn(
    typeof(EmailPostOfficeHttpApiModule),
    typeof(EmailPostOfficeApplicationModule),
    typeof(EmailPostOfficeEntityFrameworkCoreModule),
    typeof(AbpAutofacModule),
    typeof(AbpIdentityWebModule),
    typeof(AbpAccountPublicWebIdentityServerModule),
    typeof(AbpAuditLoggingWebModule),
    typeof(LeptonThemeManagementWebModule),
    typeof(SaasHostWebModule),
    typeof(AbpAccountAdminWebModule),
    typeof(AbpIdentityServerWebModule),
    typeof(LanguageManagementWebModule),
    typeof(AbpAspNetCoreMvcUiLeptonThemeModule),
    typeof(TextTemplateManagementWebModule),
    typeof(AbpGdprWebModule),
    typeof(AbpSwashbuckleModule),
    typeof(AbpAspNetCoreSerilogModule)
    )]
public class EmailPostOfficeWebModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(
                typeof(EmailPostOfficeResource),
                typeof(EmailPostOfficeDomainModule).Assembly,
                typeof(EmailPostOfficeDomainSharedModule).Assembly,
                typeof(EmailPostOfficeApplicationModule).Assembly,
                typeof(EmailPostOfficeApplicationContractsModule).Assembly,
                typeof(EmailPostOfficeWebModule).Assembly
            );
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();

        ConfigureBundles();
        ConfigureUrls(configuration);
        ConfigurePages(configuration);
        ConfigureAuthentication(context, configuration);
        ConfigureImpersonation(context, configuration);
        ConfigureAutoMapper();
        ConfigureVirtualFileSystem(hostingEnvironment);
        ConfigureNavigationServices();
        ConfigureAutoApiControllers();
        ConfigureSwaggerServices(context.Services);
        ConfigureExternalProviders(context);
        ConfigureHealthChecks(context);
    }

    private void ConfigureHealthChecks(ServiceConfigurationContext context)
    {
        context.Services.AddEmailPostOfficeHealthChecks();
    }

    private void ConfigureBundles()
    {
        Configure<AbpBundlingOptions>(options =>
        {
            options.StyleBundles.Configure(
                LeptonThemeBundles.Styles.Global,
                bundle =>
                {
                    bundle.AddFiles("/global-styles.css");
                }
            );
        });
    }

    private void ConfigurePages(IConfiguration configuration)
    {
        Configure<RazorPagesOptions>(options =>
        {
            options.Conventions.AuthorizePage("/HostDashboard", EmailPostOfficePermissions.Dashboard.Host);
            options.Conventions.AuthorizePage("/TenantDashboard", EmailPostOfficePermissions.Dashboard.Tenant);
            options.Conventions.AuthorizePage("/MailQueues/Index", EmailPostOfficePermissions.MailQueues.Default);
        });
    }

    private void ConfigureUrls(IConfiguration configuration)
    {
        Configure<AppUrlOptions>(options =>
        {
            options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
        });
    }

    private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.AddAuthentication()
            .AddJwtBearer(options =>
            {
                options.Authority = configuration["AuthServer:Authority"];
                options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]); ;
                options.Audience = "EmailPostOffice";
            });

        context.Services.ForwardIdentityAuthenticationForBearer();
    }

    private void ConfigureImpersonation(ServiceConfigurationContext context, IConfiguration configuration)
    {
        context.Services.Configure<AbpSaasHostWebOptions>(options =>
        {
            options.EnableTenantImpersonation = true;
        });
        context.Services.Configure<AbpIdentityWebOptions>(options =>
        {
            options.EnableUserImpersonation = true;
        });
        context.Services.Configure<AbpAccountOptions>(options =>
        {
            options.TenantAdminUserName = "admin";
            options.ImpersonationTenantPermission = SaasHostPermissions.Tenants.Impersonation;
            options.ImpersonationUserPermission = IdentityPermissions.Users.Impersonation;
        });
    }

    private void ConfigureAutoMapper()
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<EmailPostOfficeWebModule>();
        });
    }

    private void ConfigureVirtualFileSystem(IWebHostEnvironment hostingEnvironment)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<EmailPostOfficeWebModule>();

            if (hostingEnvironment.IsDevelopment())
            {
                options.FileSets.ReplaceEmbeddedByPhysical<EmailPostOfficeDomainSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}EmailPostOffice.Domain.Shared", Path.DirectorySeparatorChar)));
                options.FileSets.ReplaceEmbeddedByPhysical<EmailPostOfficeDomainModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}EmailPostOffice.Domain", Path.DirectorySeparatorChar)));
                options.FileSets.ReplaceEmbeddedByPhysical<EmailPostOfficeApplicationContractsModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}EmailPostOffice.Application.Contracts", Path.DirectorySeparatorChar)));
                options.FileSets.ReplaceEmbeddedByPhysical<EmailPostOfficeApplicationModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}EmailPostOffice.Application", Path.DirectorySeparatorChar)));
                options.FileSets.ReplaceEmbeddedByPhysical<EmailPostOfficeHttpApiModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}src{0}EmailPostOffice.HttpApi", Path.DirectorySeparatorChar)));
                options.FileSets.ReplaceEmbeddedByPhysical<EmailPostOfficeWebModule>(hostingEnvironment.ContentRootPath);
            }
        });
    }

    private void ConfigureNavigationServices()
    {
        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new EmailPostOfficeMenuContributor());
        });

        Configure<AbpToolbarOptions>(options =>
        {
            options.Contributors.Add(new EmailPostOfficeToolbarContributor());
        });
    }

    private void ConfigureAutoApiControllers()
    {
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers.Create(typeof(EmailPostOfficeApplicationModule).Assembly);
        });
    }

    private void ConfigureSwaggerServices(IServiceCollection services)
    {
        services.AddAbpSwaggerGen(
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "EmailPostOffice API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
                options.DocumentFilter<HideIdentityUserFilter>();

                options.MapType<ListResultDto<IdentityRoleDto>>(() => new OpenApiSchema { Type = "IdentityRoleDtoResultList" });
                options.MapType<ListResultDto<ClaimTypeDto>>(() => new OpenApiSchema { Type = "ClaimTypeDtoResultList" });
                options.MapType<PagedResultDto<IdentityRoleDto>>(() => new OpenApiSchema { Type = "IdentityRoleDtoPageResultList" });
                options.MapType<PagedResultDto<ClaimTypeDto>>(() => new OpenApiSchema { Type = "ClaimTypeDtoPageResultList" });
                options.MapType<PagedResultDto<IdentitySecurityLogDto>>(() => new OpenApiSchema { Type = "IdentitySecurityLogDtoPageResultList" });

                options.MapType<PagedResultDto<ClientWithDetailsDto>>(() => new OpenApiSchema { Type = "ClientWithDetailsDtoPageResultList" });
                options.MapType<PagedResultDto<ApiScopeWithDetailsDto>>(() => new OpenApiSchema { Type = "ApiScopeWithDetailsDtoPageResultList" });
                options.MapType<PagedResultDto<ApiResourceWithDetailsDto>>(() => new OpenApiSchema { Type = "ApiResourceWithDetailsDtoPageResultList" });


                options.MapType<PagedResultDto<IdentityUserDto>>(() => new OpenApiSchema { Type = "IdentityUserDtoPageResultList" });
                options.MapType<PagedResultDto<EntityChangeDto>>(() => new OpenApiSchema { Type = "EntityChangeDtoPageResultList" });
                options.MapType<PagedResultDto<AuditLogDto>>(() => new OpenApiSchema { Type = "AuditLogDtoPageResultList" });
                options.MapType<PagedResultDto<EditionDto>>(() => new OpenApiSchema { Type = "EditionDtoPageResultList" });

                options.MapType<PagedResultDto<GdprRequestDto>>(() => new OpenApiSchema { Type = "GdprRequestDtoPageResultList" });
                options.MapType<PagedResultDto<LanguageTextDto>>(() => new OpenApiSchema { Type = "LanguageTextDtoPageResultList" });
                options.MapType<PagedResultDto<LanguageDto>>(() => new OpenApiSchema { Type = "LanguageDtoPageResultList" });
                options.MapType<ListResultDto<LanguageDto>>(() => new OpenApiSchema { Type = "LanguageDtoResultList" });
                options.MapType<PagedResultDto<OrganizationUnitWithDetailsDto>>(() => new OpenApiSchema { Type = "OrganizationUnitWithDetailsDtoPageResultList" });
                options.MapType<ListResultDto<OrganizationUnitWithDetailsDto>>(() => new OpenApiSchema { Type = "OrganizationUnitWithDetailsDtoResultList" });
                options.MapType<PagedResultDto<IdentityResourceWithDetailsDto>>(() => new OpenApiSchema { Type = "IdentityResourceWithDetailsDtoPageResultList" });

                options.MapType<PagedResultDto<SaasTenantDto>>(() => new OpenApiSchema { Type = "SaasTenantDtoPageResultList" });
                options.MapType<PagedResultDto<TemplateDefinitionDto>>(() => new OpenApiSchema { Type = "TemplateDefinitionDtoPageResultList" });
                options.MapType<PagedResultDto<LinkUserDto>>(() => new OpenApiSchema { Type = "LinkUserDtoPageResultList" });
                options.MapType<PagedResultDto<UserData>>(() => new OpenApiSchema { Type = "UserDataPageResultList" });
                options.MapType<ListResultDto<LinkUserDto>>(() => new OpenApiSchema { Type = "LinkUserDtoResultList" });
                options.MapType<ListResultDto<UserData>>(() => new OpenApiSchema { Type = "UserDataResultList" });

                options.MapType<ListResultDto<MailQueueDto>>(() => new OpenApiSchema { Type = "MailQueueDtoResultList" });
                options.MapType<PagedResultDto<MailQueueDto>>(() => new OpenApiSchema { Type = "MailQueueDtoPageResultList" });                
                //options.MapType<TestResult>(() => new OpenApiSchema { Type = "JBHR.TestResult" });
                //options.MapType<TestResultDto>(() => new OpenApiSchema { Type = "TestResultDto" });
            }
        );
    }

    private void ConfigureExternalProviders(ServiceConfigurationContext context)
    {
        context.Services.AddAuthentication()
            .AddGoogle(GoogleDefaults.AuthenticationScheme, _ => { })
            .WithDynamicOptions<GoogleOptions, GoogleHandler>(
                GoogleDefaults.AuthenticationScheme,
                options =>
                {
                    options.WithProperty(x => x.ClientId);
                    options.WithProperty(x => x.ClientSecret, isSecret: true);
                }
            )
            .AddMicrosoftAccount(MicrosoftAccountDefaults.AuthenticationScheme, options =>
            {
                //Personal Microsoft accounts as an example.
                options.AuthorizationEndpoint = "https://login.microsoftonline.com/consumers/oauth2/v2.0/authorize";
                options.TokenEndpoint = "https://login.microsoftonline.com/consumers/oauth2/v2.0/token";
            })
            .WithDynamicOptions<MicrosoftAccountOptions, MicrosoftAccountHandler>(
                MicrosoftAccountDefaults.AuthenticationScheme,
                options =>
                {
                    options.WithProperty(x => x.ClientId);
                    options.WithProperty(x => x.ClientSecret, isSecret: true);
                }
            )
            .AddTwitter(TwitterDefaults.AuthenticationScheme, options => options.RetrieveUserDetails = true)
            .WithDynamicOptions<TwitterOptions, TwitterHandler>(
                TwitterDefaults.AuthenticationScheme,
                options =>
                {
                    options.WithProperty(x => x.ConsumerKey);
                    options.WithProperty(x => x.ConsumerSecret, isSecret: true);
                }
            );
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseAbpRequestLocalization();

        if (!env.IsDevelopment())
        {
            app.UseErrorPage();
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseCorrelationId();
        app.UseAbpSecurityHeaders();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseJwtTokenMiddleware();

        if (MultiTenancyConsts.IsEnabled)
        {
            app.UseMultiTenancy();
        }

        app.UseUnitOfWork();
        app.UseIdentityServer();
        app.UseAuthorization();
        app.UseSwagger();
        app.UseAbpSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "EmailPostOffice API");
        });
        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();
    }
}