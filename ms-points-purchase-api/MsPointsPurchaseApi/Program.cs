

// Add services to the container.
using Microsoft.IdentityModel.Tokens;
using MsPointsPurchaseApi;
using MsPointsPurchaseApi.Contracts.Data;
using MsPointsPurchaseApi.Contracts.Migrations;
using MsPointsPurchaseApi.Contracts.Repositories;
using MsPointsPurchaseApi.Data;
using MsPointsPurchaseApi.Repositories;

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
    services.AddTransient<IPlatformConfigurationContext, PlatformConfigurationContext>();
    services.AddTransient<IPointsRepository, PointsRepository>();
    services.AddTransient<IPointsPurchaseRepository, PointsPurchaseRepository>();
    services.AddTransient<IDbVersionRepository, DbVersionRepository>();

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
        options.AddPolicy("AdminPolicy", policy => policy.RequireClaim("client_id", "plataform-admin"));
    });
}
async Task Configure(IApplicationBuilder app)
{
    var logger = app.ApplicationServices.GetService<ILogger<Program>>();

    const string version = "1.0.1";
    var configuration = app.ApplicationServices.GetRequiredService<IConfiguration>();
    var dbVersionRepository = app.ApplicationServices.GetRequiredService<IDbVersionRepository>();
    var pointsRepository = app.ApplicationServices.GetRequiredService<IPointsRepository>();
    var pointsPurchaseRepository = app.ApplicationServices.GetRequiredService<IPointsPurchaseRepository>();
    var currentVersion = await dbVersionRepository.GetCurrentVersion();
    logger.LogInformation($"Current version: {currentVersion?.Name}, desired version: {version}");

    switch (currentVersion?.Name)
    {
        case null:
        case "1.0.0":
            await Migration_1_0_0.Apply(dbVersionRepository, pointsRepository);
            break;
        default:
            break;
    }

    if (currentVersion?.Name != version)
        await Configure(app);
}
