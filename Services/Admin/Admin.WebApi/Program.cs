using System.Text;
using Admin.Application.Abstractions;
using Admin.Application.Services;
using Admin.Infrastructure.Clients;
using Admin.Infrastructure.DependencyInjection;
using Admin.WebApi.Handlers;
using Hotels.Infrastructure;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Polly;
using Polly.Extensions.Http;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<CorrelationIdHandler>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Admin API", Version = "v1" });

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Bearer token"
    };

    options.AddSecurityDefinition("Bearer", securityScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, Array.Empty<string>() }
    });
});

var allowedOrigins = builder.Configuration.GetSection("Admin:AllowedOrigins").Get<string[]>() ?? [];
builder.Services.AddCors(options =>
{
    options.AddPolicy("admin-ui", policy =>
    {
        if (allowedOrigins.Length == 0)
        {
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        }
        else
        {
            policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod();
        }
    });
});

var signingKey = builder.Configuration["Admin:Jwt:SigningKey"] ?? "dev-signing-key-change-me-32chars-minimum";
var issuer = builder.Configuration["Admin:Jwt:Issuer"] ?? "Admin.WebApi";
var audience = builder.Configuration["Admin:Jwt:Audience"] ?? "Admin.WebApi";

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1)
        };
    });

builder.Services.AddAuthorization();

// MassTransit with RabbitMQ
var rabbitHost = builder.Configuration["RabbitMQ:Host"] ?? "localhost";
builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(rabbitHost, "/", h =>
        {
            h.Username(builder.Configuration["RabbitMQ:Username"] ?? "guest");
            h.Password(builder.Configuration["RabbitMQ:Password"] ?? "guest");
        });
        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddAdminPersistence(builder.Configuration);

builder.Services.AddScoped<SystemSettingsService>();

// Hotels database access for admin controllers
builder.Services.AddSingleton<PgDbContextOptions>();

var hotelsBaseUrl = builder.Configuration["Admin:Hotels:BaseUrl"] ?? "http://localhost:5001";
var retryPolicy = HttpPolicyExtensions
    .HandleTransientHttpError()
    .OrResult(msg => msg.StatusCode == HttpStatusCode.TooManyRequests)
    .WaitAndRetryAsync(3, attempt =>
        TimeSpan.FromMilliseconds(200 * Math.Pow(2, attempt)) + TimeSpan.FromMilliseconds(Random.Shared.Next(0, 100)));

var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(5));

var circuitBreakerPolicy = HttpPolicyExtensions
    .HandleTransientHttpError()
    .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));

builder.Services
    .AddHttpClient<IHotelsClient, HotelsClient>(client =>
    {
        client.BaseAddress = new Uri(hotelsBaseUrl);
        client.Timeout = TimeSpan.FromSeconds(10);
    })
    .AddHttpMessageHandler<CorrelationIdHandler>()
    .AddPolicyHandler(retryPolicy)
    .AddPolicyHandler(timeoutPolicy)
    .AddPolicyHandler(circuitBreakerPolicy);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseCors("admin-ui");

app.MapControllers();
app.MapGet("/live", () => Results.Ok("OK"));
app.MapGet("/ready", () => Results.Ok("OK"));

app.Run();
