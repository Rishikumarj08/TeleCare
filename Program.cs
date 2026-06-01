using Microsoft.EntityFrameworkCore;
using TeleCare.Data;
using TeleCare.Exceptions;
using TeleCare.Repository.Implementation;
using TeleCare.Repository.Interface;
using TeleCare.Service.Implementation;
using TeleCare.Service.Interface;
using TeleCare.Filters;

var builder = WebApplication.CreateBuilder(args);

//  Database Configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// Controllers & Swagger
builder.Services.AddControllers(options =>
{
    // Register global model validation filter to return structured errors
    options.Filters.Add<ValidateModelAttribute>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Program Module
builder.Services.AddScoped<IProgramRepository, ProgramRepository>();
builder.Services.AddScoped<IProgramService, ProgramService>();

// Medication Module
builder.Services.AddScoped<IMedicationRepository, MedicationRepository>();
builder.Services.AddScoped<IMedicationService, MedicationService>();

// // CarePlan Module
builder.Services.AddScoped<ICarePlanRepository, CarePlanRepository>();
builder.Services.AddScoped<ICarePlanService, CarePlanService>();



var app = builder.Build();

// Global Exception Middleware
app.UseMiddleware<GlobalExceptionMiddleware>();

//  Swagger (Development Only)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware Pipeline
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
