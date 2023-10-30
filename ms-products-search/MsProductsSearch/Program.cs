using Microsoft.IdentityModel.Tokens;
using MsProductsSearch;
using MsProductsSearch.Contracts.Data;
using MsProductsSearch.Contracts.Repositories;
using MsProductsSearch.Data;
using MsProductsSearch.Repositories;

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
    services.AddTransient<ICatalogContext, CatalogContext>();
    services.AddTransient<IPlataformConfigurationContext, PlataformConfigurationContext>();
    services.AddTransient<IProductRepository, ProductRepository>();
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

    var configuration = app.ApplicationServices.GetRequiredService<IConfiguration>();

    //try
    //{
    //    const string version = "1.0.0";
    //    var dbVersionRepository = app.ApplicationServices.GetRequiredService<IDbVersionRepository>();
    //    var currentVersion = await dbVersionRepository.GetCurrentVersion();
    //    logger.LogInformation($"Current version: {currentVersion?.Name}, desired version: {version}");


    //    if (currentVersion?.Name != version)
    //        await Configure(app);
    //}
    //catch (Exception ex)
    //{
    //    logger.LogError($"Failed Bootstrap. Details: {ex.Message}");
    //    throw;
    //}
}