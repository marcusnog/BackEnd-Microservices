using Catalog.Api.Contracts.Data;
using Catalog.Api.Contracts.Migrations;
using Catalog.Api.Contracts.Repositories;
using Catalog.Api.Data;
using Catalog.Api.Repositories;
using Microsoft.IdentityModel.Tokens;

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

if (builder.Configuration["DOTNET_RUNNING_IN_CONTAINER"] != "true")
    app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

void ConfigureServices(IServiceCollection services, IConfiguration config)
{
    services.AddTransient<ICatalogContext, CatalogContext>();
    services.AddTransient<ICategoryRepository, CategoryRepository>();
    services.AddTransient<IMainCategoryRepository, MainCategoryRepository>();
    services.AddTransient<IProductSkuRepository, ProductSkuRepository>();
    services.AddTransient<IProductSkuBillRepository, ProductSkuBillRepository>();
    services.AddTransient<IProductSkuPreDefinedPriceRepository, ProductSkuPreDefinedPriceRepository>();
    services.AddTransient<IProductSkuRangePriceRepository, ProductSkuRangePriceRepository>();
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
        options.AddPolicy("ProfilePolicy", policy => policy.RequireClaim("client_id", "plataform-catalog"));
    });
}
async Task Configure(IApplicationBuilder app)
{
    var logger = app.ApplicationServices.GetService<ILogger<Program>>();

    try
    {
        const string version = "1.1.4";
        var categoryRepository = app.ApplicationServices.GetRequiredService<ICategoryRepository>();
        var mainCategoryRepository = app.ApplicationServices.GetRequiredService<IMainCategoryRepository>();
        var productRepository = app.ApplicationServices.GetRequiredService<IProductSkuRepository>();
        var dbVersionRepository = app.ApplicationServices.GetRequiredService<IDbVersionRepository>();
        var currentVersion = await dbVersionRepository.GetCurrentVersion();
        logger.LogInformation($"Current version: {currentVersion?.Name}, desired version: {version}");

        switch (currentVersion?.Name)
        {
            case null:
                await V1Migration.Apply(categoryRepository, productRepository, dbVersionRepository);
                break;
            case "1.0.0":
            case "1.1.0":
                await V4Migration.Apply(categoryRepository, productRepository, dbVersionRepository);
                break;
            case "1.1.1":
            case "1.1.2":
                await V2Migration.Apply(categoryRepository, productRepository, dbVersionRepository);
                break;
            case "1.1.3":
                await V3Migration.Apply(categoryRepository, mainCategoryRepository, productRepository, dbVersionRepository);
                break;
            default:
                break;
        }

        if (currentVersion?.Name != version)
            await Configure(app);
    }
    catch (Exception ex)
    {
        logger.LogError($"Failed Bootstrap. Details: {ex.Message}");
        throw;
    }
}