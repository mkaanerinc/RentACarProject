using Autofac;
using Autofac.Extensions.DependencyInjection;
using Business.DependencyResolvers.Autofac;
using Core.Utilities.Security.JWT;
using Microsoft.IdentityModel.Tokens;
using Core.DependencyResolvers;
using Core.Utilities.IoC;
using Core.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Core.Utilities.Security.Encryption;

var builder = WebApplication.CreateBuilder(args);

// Autofac Configuration
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterModule(new BusinessModule());
});

// Token Configuration
var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<TokenOptions>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
   .AddJwtBearer(options =>
   {
       options.TokenValidationParameters = new TokenValidationParameters
       {
           ValidateIssuer = true,
           ValidateAudience = true,
           ValidateLifetime = true,
           ValidIssuer = tokenOptions.Issuer,
           ValidAudience = tokenOptions.Audience,
           ValidateIssuerSigningKey = true,
           IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey)
       };
   });

builder.Services.AddDependencyResolvers(new ICoreModule[]
{
                new CoreModule()
});

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddCors();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder =>
{
    builder.WithOrigins("https://localhost:7004").AllowAnyHeader();
    builder.WithOrigins("http://localhost:5201").AllowAnyHeader();
});

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
