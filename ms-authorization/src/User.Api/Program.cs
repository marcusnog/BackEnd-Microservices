using Microsoft.IdentityModel.Tokens;
using User.Api.Connector.Contracts.Repositories;
using User.Api.Connector.Repositories;
using User.Api.Contracts.Migrations;
using User.Api.Contracts.Repositories;
using User.Api.Contracts.UseCases;
using User.Api.Extensions;
using User.Api.Repositories;
using User.Api.UseCases;
using User.Contracts.Repositories;
using User.Repositories;

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
    services.AddTransient<IForgotPasswordUseCase, ForgotPasswordUseCase>();
    services.AddTransient<IAuthContext, AuthContext>();
    services.AddTransient<IUserAdministratorRepository, UserAdministratorRepository>();
    services.AddTransient<IUserParticipantRepository, UserParticipantRepository>();
    services.AddTransient<IProfileRepository, ProfileRepository>();
    services.AddTransient<ISystemRepository, SystemRepository>();
    services.AddTransient<IRoleRepository, RoleRepository>();
    services.AddTransient<IDbVersionRepository, DbVersionRepository>();
    services.AddTransient<IPasswordRecoveryRepository, PasswordRecoveryRepository>();
    services.AddTransient<IAddressRepository, AddressRepository>();
    services.AddTransient<IRedisRepository, RedisRepository>();
    services.AddTransient<IAccountUseCase, AccountUseCase>();
    services.AddTransient<IAccountMovimentUseCase, AccountMovimentUseCase>();

    services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = config.GetValue<string>("RedisSettings:ConnectionString");
    });

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
        options.AddPolicy("ProfilePolicy", policy => policy.RequireClaim("client_id", "plataform-catalog"));
        options.AddPolicy("ClientPolicy", policy => policy.RequireClaim("client_id", "ms-authentication", "ms-authorization", "plataform-catalog"));
        options.AddPolicy("AdminPolicy", policy => policy.RequireClaim("client_id", "plataform-admin"));
    });
}
async Task Configure(IApplicationBuilder app)
{
    var logger = app.ApplicationServices.GetService<ILogger<Program>>();

    const string version = "1.0.14";
    var configuration = app.ApplicationServices.GetRequiredService<IConfiguration>();
    var userRepository = app.ApplicationServices.GetRequiredService<IUserAdministratorRepository>();
    var userParticipantRepository = app.ApplicationServices.GetRequiredService<IUserParticipantRepository>();
    var systemRepository = app.ApplicationServices.GetRequiredService<ISystemRepository>();
    var profileRepository = app.ApplicationServices.GetRequiredService<IProfileRepository>();
    var dbVersionRepository = app.ApplicationServices.GetRequiredService<IDbVersionRepository>();
    var currentVersion = await dbVersionRepository.GetCurrentVersion();
    logger.LogInformation($"Current version: {currentVersion?.Name}, desired version: {version}");

    switch (currentVersion?.Name)
    {
        case null:
        case "1.0.0":
        case "1.0.1":
        case "1.0.2":
        case "1.0.3":
        case "1.0.4":
        case "1.0.5":
        case "1.0.6":
        case "1.0.7":
        case "1.0.8":
        case "1.0.9":
        case "1.0.10":
            await Migration_1_0_11.Apply(systemRepository, userRepository, profileRepository, dbVersionRepository, configuration);
            break;
        case "1.0.11":
            await Migration_1_0_12.Apply(systemRepository, userParticipantRepository, profileRepository, dbVersionRepository, configuration);
            break;
        case "1.0.12":
            await Migration_1_0_13.Apply(systemRepository, userRepository, profileRepository, dbVersionRepository, configuration);
            break;
        case "1.0.13":
            await Migration_1_0_14.Apply(systemRepository, userRepository, profileRepository, dbVersionRepository, configuration);
            break;
        default:
            break;
    }

    if (currentVersion?.Name != version)
        await Configure(app);
}