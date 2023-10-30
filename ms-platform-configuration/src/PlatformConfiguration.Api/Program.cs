using Microsoft.IdentityModel.Tokens;
using PlatformConfiguration;
using PlatformConfiguration.Api.Contracts.Data;
using PlatformConfiguration.Api.Contracts.Migrations;
using PlatformConfiguration.Api.Contracts.Repositories;
using PlatformConfiguration.Api.Contracts.UseCases;
using PlatformConfiguration.Api.Data;
using PlatformConfiguration.Api.Repositories;
using PlatformConfiguration.Api.UseCases;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
ConfigureServices(builder.Services, builder.Configuration);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
await Configure(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors((x) =>
    {
        x.AllowAnyOrigin();
        x.AllowAnyMethod();
        x.AllowAnyHeader();
    });
}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseRequestUserInfo();
app.Run();


void ConfigureServices(IServiceCollection services, IConfiguration config)
{
    services.AddTransient<IPlataformConfigurationContext, PlataformConfigurationContext>();
    services.AddTransient<ICampaignRepository, CampaignRepository>();
    services.AddTransient<IClientRepository, ClientRepository>();
    services.AddTransient<IDbVersionRepository, DbVersionRepository>();
    services.AddTransient<IPartnerRepository, PartnerRepository>();
    services.AddTransient<IStoreRepository, StoreRepository>();
    services.AddTransient<IGetPartnerConfigurationUseCase, GetPartnerConfigurationUseCase>();


    services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = config.GetValue<string>("IDENTITYSERVER_URL");
                options.IncludeErrorDetails = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false
                };
                options.RequireHttpsMetadata = false;
            });

    services.AddAuthorization(options =>
    {
        options.AddPolicy("ProfilePolicy", policy => policy.RequireClaim("client_id", "plataform-admin", "plataform-catalog"));
    });
}
async Task Configure(IApplicationBuilder app)
{
    var logger = app.ApplicationServices.GetService<ILogger<Program>>();

    const string version = "1.0.2";
    var configuration = app.ApplicationServices.GetRequiredService<IConfiguration>();
    var campaignRepository = app.ApplicationServices.GetRequiredService<ICampaignRepository>();
    var clientRepository = app.ApplicationServices.GetRequiredService<IClientRepository>();
    var partnerRepository = app.ApplicationServices.GetRequiredService<IPartnerRepository>();
    var storeRepository = app.ApplicationServices.GetRequiredService<IStoreRepository>();
    var dbVersionRepository = app.ApplicationServices.GetRequiredService<IDbVersionRepository>();
    var currentVersion = await dbVersionRepository.GetCurrentVersion();
    logger.LogInformation($"Current version: {currentVersion?.Name}, desired version: {version}");

    switch (currentVersion?.Name)
    {
        case null:
            await Migration_1_0_0.Apply(configuration, campaignRepository, clientRepository, partnerRepository, storeRepository, dbVersionRepository);
            break;
        case "1.0.0":
            await Migration_1_0_1.Apply(configuration, campaignRepository, clientRepository, partnerRepository, storeRepository, dbVersionRepository);
            break;
        case "1.0.1":
            await Migration_1_0_2.Apply(configuration, clientRepository, dbVersionRepository);
            break;
        default:
            break;
    }

    if (currentVersion?.Name != version)
        await Configure(app);
}