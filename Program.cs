using Microsoft.EntityFrameworkCore;
using TeleCare.Data;
using TeleCare.Repository.Implementation;
using TeleCare.Repository.Interface;
using TeleCare.Service.Implementation;
using TeleCare.Service.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;
using TeleCare.Exceptions;
 
var builder = WebApplication.CreateBuilder(args);
 
// ✅ CORS Policy — allows frontend (React/Angular/etc.) to communicate with the API

builder.Services.AddCors(options =>

{

    options.AddPolicy("AllowAll", policy =>

    {

        policy.AllowAnyOrigin()

              .AllowAnyMethod()

              .AllowAnyHeader();

    });

});
 
// ✅ Add services to the container

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>

{

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme

    {

        Description = "Enter your JWT token below. Do not add 'Bearer' prefix.",

        Name = "Authorization",

        In = ParameterLocation.Header,

        Type = SecuritySchemeType.Http,

        Scheme = "bearer",

        BearerFormat = "JWT"

    });
 
    options.AddSecurityRequirement(doc =>

    {

        var requirement = new OpenApiSecurityRequirement();

        var scheme = new OpenApiSecuritySchemeReference("Bearer", doc);

        requirement.Add(scheme, new List<string>());

        return requirement;

    });

});
 
// ✅ Database Connection (update connection string in appsettings.json)

builder.Services.AddDbContext<AppDbContext>(options =>

    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
 
// ✅ Dependency Injection — Existing

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IAuthService, AuthService>();
 
// ✅ Dependency Injection — Rules

builder.Services.AddScoped<IRuleRepository, RuleRepository>();

builder.Services.AddScoped<IRuleService, RuleService>();
 
// ✅ Dependency Injection — Payers (read-only lookup, used within Claims and Payments)

builder.Services.AddScoped<IPayerRepository, PayerRepository>();
 
// ✅ Dependency Injection — Claims

builder.Services.AddScoped<IClaimRepository, ClaimRepository>();

builder.Services.AddScoped<IClaimService, ClaimService>();
 
// ✅ Dependency Injection — Payments

builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

builder.Services.AddScoped<IPaymentService, PaymentService>();
 
// ✅ Dependency Injection — Charges

builder.Services.AddScoped<IChargeRepository, ChargeRepository>();

builder.Services.AddScoped<IChargeService, ChargeService>();

// ✅ Dependency Injection — Notifications

builder.Services.AddScoped<INotificationRepository, NotificationRepository>();

builder.Services.AddScoped<INotificationService, NotificationService>();
 
builder.Services.AddAuthentication(options =>

    {

        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

    })

    .AddJwtBearer(options =>

    {

        options.RequireHttpsMetadata = false;

        options.SaveToken = true;

        options.TokenValidationParameters = new TokenValidationParameters

        {

            ValidateIssuer = true,

            ValidateAudience = true,

            ValidateLifetime = true,

            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],

            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? string.Empty))

        };

    });
 
builder.Services.AddAuthorization();
 
var app = builder.Build();
 
// ✅ Use Global Exception Middleware (CRITICAL — must be first)

app.UseMiddleware<GlobalExceptionMiddleware>();
 
// ✅ Configure the HTTP request pipeline

if (app.Environment.IsDevelopment())

{

    app.UseSwagger();

    app.UseSwaggerUI(options =>

    {

        options.SwaggerEndpoint("/swagger/v1/swagger.json", "TeleCare v1");

    });

}
 
app.UseHttpsRedirection();
 
// ✅ Apply CORS — must be before Authentication and Authorization

app.UseCors("AllowAll");
 
app.UseAuthentication();
 
app.UseAuthorization();
 
app.MapControllers();
 
app.Run();
 