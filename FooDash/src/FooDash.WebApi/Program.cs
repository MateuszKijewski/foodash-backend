using FluentValidation.AspNetCore;
using FooDash.Application;
using FooDash.Domain;
using FooDash.Application.Auth.Providers;
using FooDash.Application.Security.Services;
using FooDash.Persistence;
using FooDash.WebApi.Helpers;
using FooDash.WebApi.Providers;
using FooDash.WebApi.Services;
using FooDash.WebApi.Common.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using FooDash.Application.Common.Interfaces.Auth;
using FooDash.Application.Auth.Dtos.Responses;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

JwtSecurityTokenHandler.DefaultInboundClaimFilter.Clear();
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();



services.AddControllers();
services.AddCors();
services.AddMvc();

services.AddFluentValidation(
    fv => fv.RegisterValidatorsFromAssemblyContaining<GetCurrentUserResponse>());

services.AddOptions();
services.Configure<EncryptionOptions>(builder.Configuration.GetSection(nameof(EncryptionOptions)));
services.Configure<TokenValidationOptions>(builder.Configuration.GetSection(nameof(TokenValidationOptions)));
services.AddScoped<ICurrentUserService, CurrentUserService>();

services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "FooDash", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              },
              Scheme = "Bearer",
              Name = "Bearer",
              In = ParameterLocation.Header,
            },
            new List<string>()
          }
        });
});

services.AddHttpContextAccessor();

services.AddDomain(builder.Configuration);
services.AddPersistence(builder.Configuration);
services.AddApplication(builder.Configuration);

services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration.GetSection(nameof(TokenValidationOptions))[nameof(TokenValidationOptions.Issuer)],
            ValidAudience = builder.Configuration.GetSection(nameof(TokenValidationOptions))[nameof(TokenValidationOptions.Audience)],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection(nameof(TokenValidationOptions))[nameof(TokenValidationOptions.IssuerSigningKey)]))
        };
    });

var authorizationOptionsProvider = new AuthorizationOptionsProvider(services);
services.AddAuthorization(options => authorizationOptionsProvider.GetAuthorizationOptions(options));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My service");
    c.RoutePrefix = string.Empty;
});
app.UseCors(options => options.WithOrigins(ConfigurationHelper.GetAllowedOrigins(builder.Configuration)).AllowAnyMethod().AllowAnyHeader());
app.UseCustomExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();
app.UseRouting();
app.UseAuthorization();
app.ConfigureSignalR();
app.MapControllers();

app.Run();