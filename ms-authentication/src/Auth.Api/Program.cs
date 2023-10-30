using Auth.Api.Contracts.Data;
using Auth.Api.Contracts.Migrations;
using Auth.Api.Contracts.Repositories;
using Auth.Api.Contracts.Services;
using Auth.Api.Contracts.UserCases;
using Auth.Api.Data;
using Auth.Api.Repositories;
using Auth.Api.Services;
using Auth.Api.UserCases;
using IdentityServer4.Services;
using IdentityServer4.Validation;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
ConfigureServices(builder.Services, builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
await Configure(app);

app.UseRouting();
app.Map("/authapi", api =>
{
    api.UseIdentityServer();
});
app.UseAuthorization();
app.MapControllers();

app.Run();

void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{

    services.AddTransient<IIdentityContext, IdentityContext>();
    services.AddTransient<IIdentityRepository, IdentityRepository>();
    services.AddTransient<IDbVersionRepository, DbVersionRepository>();
    services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
    services.AddTransient<IProfileService, ProfileService>();
    services.AddTransient<IUserAuthorizationService, UserAuthorizationService>();
    services.AddTransient<ICampaignConnectorLoginUseCase, CampaignConnectorLoginUseCase>();
    services.AddLogging(builder => builder.SetMinimumLevel(LogLevel.Trace));

    services.AddIdentityServer((options) =>
    {
        options.IssuerUri = configuration.GetValue<string>("IDENTITYSERVER_URL");
    })
        .AddClientStore<UserClientStore>()
        .AddResourceStore<ResourceStore>()
        .AddExtensionGrantValidator<TokenGrantValidator>()
        .AddDeveloperSigningCredential();

    services.AddControllers();

};

async Task Configure(IApplicationBuilder app)
{
    var logger = app.ApplicationServices.GetService<ILogger<Program>>();
    app.UseCors((g) => g.AllowAnyOrigin());

    const string version = "1.0.2";
    var configuration = app.ApplicationServices.GetRequiredService<IConfiguration>();
    var dbVersionRepository = app.ApplicationServices.GetRequiredService<IDbVersionRepository>();
    var clientRepository = app.ApplicationServices.GetRequiredService<IIdentityRepository>();
    var currentVersion = await dbVersionRepository.GetCurrentVersion();
    logger.LogInformation($"Current version: {currentVersion?.Name}, desired version: {version}");

    switch (currentVersion?.Name)
    {
        case null:
        case "1.0.0":
            await Migration_1_0_0.Apply(dbVersionRepository, clientRepository);
            break;
        case "1.0.1":
            await Migration_1_0_1.Apply(dbVersionRepository, clientRepository);
            break;
        default:
            break;
    }

    if (currentVersion?.Name != version)
        await Configure(app);
}
