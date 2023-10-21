using Business.Abstract;
using Business.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<DbContext, RentACarProjectContext>();

// Business IoC
builder.Services.AddScoped<IBrandService, BrandManager>();
builder.Services.AddScoped<ICarService, CarManager>();
builder.Services.AddScoped<IColorService, ColorManager>();
builder.Services.AddScoped<ICustomerService, CustomerManager>();
builder.Services.AddScoped<IRentalService, RentalManager>();
builder.Services.AddScoped<IUserService, UserManager>();

// DataAccess IoC
builder.Services.AddScoped<IBrandDal, EFBrandDal>();
builder.Services.AddScoped<ICarDal, EFCarDal>();
builder.Services.AddScoped<IColorDal, EFColorDal>();
builder.Services.AddScoped<ICustomerDal, EFCustomerDal>();
builder.Services.AddScoped<IRentalDal, EFRentalDal>();
builder.Services.AddScoped<IUserDal, EFUserDal>();

// Add services to the container.

builder.Services.AddControllers();
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

app.UseAuthorization();

app.MapControllers();

app.Run();
