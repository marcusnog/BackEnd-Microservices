using Microsoft.IdentityModel.Tokens;
using MsPaymentIntegrationCelcoin.Contracts.Repository;
using MsPaymentIntegrationCelcoin.Repository;
using MsPaymentIntegrationTransfeera.Api.Contracts.Data;
using MsPaymentIntegrationTransfeera.Api.Contracts.Services;
using MsPaymentIntegrationTransfeera.Api.Contracts.UseCases;
using MsPaymentIntegrationTransfeera.Api.Data;
using MsPaymentIntegrationTransfeera.Api.Services;
using MsPaymentIntegrationTransfeera.Api.UseCases;
using MsPaymentIntegrationTransfeera.Contracts.UseCases;
using MsPaymentIntegrationTransfeera.Extensions;
using MsPaymentIntegrationTransfeera.UseCases;

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
    services.AddTransient<IIntegrationTransfeeraService, IntegrationTransfeeraService>();
    services.AddTransient<IBilletContext, BilletContext>();
    services.AddTransient<IBilletCreationUseCase, BilletCreationUseCase>();
    services.AddTransient<IBilletRepository, BilletRepository>();
    services.AddTransient<ICampaignConnectorUseCases, CampaignConnectorUseCases>();

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
        options.AddPolicy("ProfilePolicy", profile => profile.RequireClaim("client_id", "plataform-catalog"));
    });
}
async Task Configure(IApplicationBuilder app)
{
    var logger = app.ApplicationServices.GetService<ILogger<Program>>();

    var configuration = app.ApplicationServices.GetRequiredService<IConfiguration>();
}