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
 
// ✅ CORS Policy

builder.Services.AddCors(options =>

{

    options.AddPolicy("AllowAll", policy =>

    {

        policy.AllowAnyOrigin()

              .AllowAnyMethod()

              .AllowAnyHeader();

    });

});
 
// ✅ Database Connection

builder.Services.AddDbContext<AppDbContext>(options =>

    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
 
// ✅ Dependency Injection — Teammates' modules

builder.Services.AddScoped<IPatientRepository, PatientRepository>();

builder.Services.AddScoped<IPatientService, PatientService>();
 
builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();

builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
 
builder.Services.AddScoped<ITelemetryRepository, TelemetryRepository>();

builder.Services.AddScoped<ITelemetryService, TelemetryService>();
 
builder.Services.AddScoped<IAdherenceRepository, AdherenceRepository>();

builder.Services.AddScoped<IAdherenceService, AdherenceService>();
 
builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();

builder.Services.AddScoped<IDeviceService, DeviceService>();
 
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();

builder.Services.AddScoped<IAppointmentService, AppointmentService>();
 
builder.Services.AddScoped<IVisitNoteRepository, VisitNoteRepository>();

builder.Services.AddScoped<IVisitNoteService, VisitNoteService>();
 
builder.Services.AddScoped<IAlertRepository, AlertRepository>();

builder.Services.AddScoped<IAlertService, AlertService>();
 
// ✅ Dependency Injection — Your modules

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IAuthService, AuthService>();
 
builder.Services.AddScoped<IRuleRepository, RuleRepository>();

builder.Services.AddScoped<IRuleService, RuleService>();
 
builder.Services.AddScoped<IPayerRepository, PayerRepository>();
 
builder.Services.AddScoped<IClaimRepository, ClaimRepository>();

builder.Services.AddScoped<IClaimService, ClaimService>();
 
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

builder.Services.AddScoped<IPaymentService, PaymentService>();
 
builder.Services.AddScoped<IChargeRepository, ChargeRepository>();

builder.Services.AddScoped<IChargeService, ChargeService>();
 
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();

builder.Services.AddScoped<INotificationService, NotificationService>();
 
builder.Services.AddControllers();

// CORS: allow browser requests during development. Restrict origins in production.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

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
 
// ✅ Global Exception Middleware — must be first

app.UseMiddleware<GlobalExceptionMiddleware>();
 
if (app.Environment.IsDevelopment())

{

    app.UseSwagger();

    app.UseSwaggerUI(options =>

    {

        options.SwaggerEndpoint("/swagger/v1/swagger.json", "TeleCare v1");

    });

}
 
app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
 