using Microsoft.IdentityModel.Tokens;
using MsPointsApi;
using MsPointsApi.Contracts.Data;
using MsPointsApi.Contracts.Repositories;
using MsPointsApi.Data;
using MsPointsApi.Repositories;

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
    services.AddTransient<IAuthContext, AuthContext>();
    services.AddTransient<IAccountRepository, AccountRepository>();
    services.AddTransient<IAccount_MovimentRepository, Account_MovimentRepository>();

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
        options.AddPolicy("ProfilePolicy", policy => policy.RequireClaim("client_id", "plataform-catalog", "plataform-admin"));
        options.AddPolicy("AdminPolicy", policy => policy.RequireClaim("client_id", "plataform-admin"));
    });
}
async Task Configure(IApplicationBuilder app)
{
    var logger = app.ApplicationServices.GetService<ILogger<Program>>();

    //const string version = "1.0.0";
    var configuration = app.ApplicationServices.GetRequiredService<IConfiguration>();

    //var accountRepository = app.ApplicationServices.GetRequiredService<IAccountRepository>();
    //var accountMovimentRepository = app.ApplicationServices.GetRequiredService<IAccount_MovimentRepository>();
    //var currentVersion = await dbVersionRepository.GetCurrentVersion();
    //logger.LogInformation($"Current version: {currentVersion?.Name}, desired version: {version}");

    //switch (currentVersion?.Name)
    //{
    //    case null:
    //        await Migration_1_0_0.Apply(configuration, campaignRepository, clientRepository, partnerRepository, storeRepository, dbVersionRepository);
    //        break;
    //    case "1.0.0":
    //        await Migration_1_0_1.Apply(configuration, campaignRepository, clientRepository, partnerRepository, storeRepository, dbVersionRepository);
    //        break;
    //    case "1.0.1":
    //        await Migration_1_0_2.Apply(configuration, clientRepository, dbVersionRepository);
    //        break;
    //    default:
    //        break;
    //}

    //if (currentVersion?.Name != version)
    //await Configure(app);
}