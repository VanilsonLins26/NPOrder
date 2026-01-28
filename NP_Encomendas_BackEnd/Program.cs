using MercadoPago.Client;
using MercadoPago.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NP_Encomendas_BackEnd.Client;
using NP_Encomendas_BackEnd.Context;
using NP_Encomendas_BackEnd.DTOs.Mapping;
using NP_Encomendas_BackEnd.Repositories;
using NP_Encomendas_BackEnd.Services;
using NP_Encomendas_BackEnd.Services.Background;
using System;
using Polly;
using Polly.Extensions.Http;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

MercadoPagoConfig.AccessToken = builder.Configuration["MercadoPago:AccessToken"];

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen(c =>
{
    //var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    //c.IncludeXmlComments(xmlPath);

    c.SwaggerDoc("v1", new OpenApiInfo { Title = "NP_Encomendas_BackEnd", Version = "v1" });

    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {

            AuthorizationCode = new OpenApiOAuthFlow
            {

                AuthorizationUrl = new Uri("https://localhost:5001/connect/authorize"),
                TokenUrl = new Uri("https://localhost:5001/connect/token"),
                Scopes = new Dictionary<string, string>
                {
                    { "nporder_api", "API do NPOrder" },
                    { "openid", "OpenID" },
                    { "profile", "Profile" }
                }
            }
        }
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "oauth2"
                },
                Scheme = "oauth2",
                Name = "oauth2",
                In = ParameterLocation.Header
            },
            new List<string> { "nporder_api" }
        }
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {

        options.Authority = "https://localhost:5001";

        options.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = "name",
        };

        options.Audience = "nporder_api";


        options.RequireHttpsMetadata = false;

    });


builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowIdentity",
        policy => policy
            .WithOrigins("http://localhost:5001", "http://localhost:4200", "https://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .WithExposedHeaders("X-Pagination"));

});


var mySqlConnection = builder.Configuration.GetConnectionString("AppContext");
builder.Services.AddDbContext<AppDbContext>(options =>
                                            options.UseMySql(mySqlConnection, ServerVersion
                                            .AutoDetect(mySqlConnection)));

builder.Services.AddHttpClient<WhatsAppService>((provider, client) =>
{

    var configuration = provider.GetRequiredService<IConfiguration>();


    var baseUrl = configuration["EvolutionApi:BaseUrl"];
    var apiKey = configuration["EvolutionApi:ApiKey"];

    if (!string.IsNullOrEmpty(baseUrl))
    {
        client.BaseAddress = new Uri(baseUrl);
    }

    if (!string.IsNullOrEmpty(apiKey))
    {
        client.DefaultRequestHeaders.Add("apikey", apiKey);
    }
})
.AddPolicyHandler(GetRetryPolicy()); 

static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError() 
        .WaitAndRetryAsync(3, retryAttempt =>
            TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
}


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddAutoMapper(typeof(DTOMapping));
builder.Services.AddScoped<MercadoPagoService>();
builder.Services.AddScoped<CreatePaymentPreferenceService>();
builder.Services.AddScoped<ProccessPaymentNotificationService>();
builder.Services.AddHostedService<OrderCancellationService>();
builder.Services.AddHostedService<WhatsAppSenderService>();
builder.Services.AddHttpClient<WhatsAppService>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
     app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "NPORDER API v1"));
}

app.UseCors("AllowIdentity");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
