using Ms.Campaign.Connector.Contracts.Data;
using Ms.Campaign.Connector.Contracts.Migrations;
using Ms.Campaign.Connector.Contracts.Repositories;
using Ms.Campaign.Connector.Data;
using Ms.Campaign.Connector.Repositories;

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
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{

    services.AddTransient<ICampaignConnectorContext, CampaignConnectorContext>();
    services.AddTransient<ICampaignConnectorRepository, CampaignConnectorRepository>();
    services.AddTransient<IDbVersionRepository, DbVersionRepository>();
    services.AddTransient<IRedisRepository, RedisRepository>();

    //services.AddSwaggerGen(c =>
    //{
    //    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    //});

    services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = configuration.GetValue<string>("RedisSettings:ConnectionString");
    });

};

async Task Configure(IApplicationBuilder app)
{
    var logger = app.ApplicationServices.GetService<ILogger<Program>>();

    const string version = "1.0.0";
    var configuration = app.ApplicationServices.GetRequiredService<IConfiguration>();
    var dbVersionRepository = app.ApplicationServices.GetRequiredService<IDbVersionRepository>();
    var campaignConnectorRepository = app.ApplicationServices.GetRequiredService<ICampaignConnectorRepository>();
    var currentVersion = await dbVersionRepository.GetCurrentVersion();
    logger.LogInformation($"Current version: {currentVersion?.Name}, desired version: {version}");

    switch (currentVersion?.Name)
    {
        case null:
            await Migration_1_0_0.Apply(dbVersionRepository, campaignConnectorRepository);
            break;
        default:
            break;
    }

    if (currentVersion?.Name != version)
        await Configure(app);
}
