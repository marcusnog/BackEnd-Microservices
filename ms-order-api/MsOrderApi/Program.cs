using MsOrderApi.Contracts.Data;
using MsOrderApi.Contracts.Repository;
using MsOrderApi.Contracts.Services;
using MsOrderApi.Data;
using MsOrderApi.Extensions;
using MsOrderApi.Repository;
using MsOrderApi.Services;



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
    services.AddTransient<INetShoesService, NetShoesService>();
    services.AddTransient<IViaVarejoService, ViaVarejoService>();
    services.AddTransient<IGifttyService, GifttyService>();
    services.AddTransient<IControlPointInternalService, ControlPointInternalService>();
    services.AddTransient<IControlPointExternalService, ControlPointExternalService>(); 
    services.AddTransient<IIntegrationRechargeService, IntegrationRechargeService>();
    services.AddTransient<IIntegrationTransfeeraService, IntegrationTransfeeraService>();
    services.AddTransient<IMagaluService, MagaluService>();
        

    services.AddTransient<IOrderRepository, OrderRepository>();   
    services.AddTransient<ICatalogContext, CatalogContext>();

    services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = config.GetValue<string>("CacheSettings:ConnectionString");
    });



    //services.AddAuthentication("Bearer")
    //        .AddJwtBearer("Bearer", options =>
    //        {
    //            options.Authority = config.GetValue<string>("IDENTITYSERVER_URL");
    //            options.IncludeErrorDetails = true;
    //            options.TokenValidationParameters = new TokenValidationParameters
    //            {
    //                ValidateAudience = false
    //            };
    //            options.RequireHttpsMetadata = false;
    //        });



    //services.AddAuthorization(options =>
    //{
    //    options.AddPolicy("ClientPolicy", policy => policy.RequireClaim("client_id", "ms-authentication"));
    //    options.AddPolicy("AdminPolicy", policy => policy.RequireClaim("client_id", "plataform-admin"));
    //});
}



async Task Configure(IApplicationBuilder app)
{
    var logger = app.ApplicationServices.GetService<ILogger<Program>>();



    var configuration = app.ApplicationServices.GetRequiredService<IConfiguration>();
}