using FluentAPIValidationDemo.Models;
using FluentAPIValidationDemo.Validators;
using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddControllers()
                // Optionally, configure JSON options or other formatter settings
                .AddJsonOptions(options =>
                {
                    // Configure JSON serializer settings to keep the original names in serialization and deserialization
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                });
// Register the ECommerceDbContext with dependency injection

builder.Services.AddDbContext<ECommerceDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("ECommerceDBConnection")));
//Enables integration between FluentValidation and ASP.NET Core automatic validation pipeline.
//builder.Services.AddFluentValidationAutoValidation();
// Register FluentValidation validator from the current assembly
builder.Services.AddValidatorsFromAssemblyContaining<ProductValidator>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddSwaggerUI();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI(c => {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V2");
});


app.Run();
