using HrSystem.Models;
using HrSystem.Repository;
using HrSystem.Seeds;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Security.Principal;
using System.Text;

namespace HrSystem
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var txt = "_myAllowSpecificOrigins";
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;
            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddControllers()
        .AddJsonOptions(options =>
            options.JsonSerializerOptions.Converters.Add(new TimeSpanConverter()));
            builder.Services.AddMvc().AddNewtonsoftJson();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(txt,
                builder =>
                {
                    builder.AllowAnyOrigin(); builder.AllowAnyHeader(); builder.AllowAnyMethod();
                });
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

	builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



//builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ITIContext>();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ITIContext>().AddDefaultTokenProviders();

//Authorize 
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    options.SaveToken = true;
  
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = false,
        ValidateIssuerSigningKey=true,
        //ValidIssuer = configuration["JWT:ValidIssuer"],
        ValidateAudience = false,
        //ValidAudience = configuration["JWT:ValidAudiance"],
        IssuerSigningKey = 
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:Token").Value))
    };
});


            builder.Services.AddDbContext<ITIContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("cs")));
            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            builder.Services.AddScoped<ILeaveAttendRepository, LeaveAttendRepository>();
            builder.Services.AddScoped<ISalaryReportRepository, SalaryReportRepository>();
            builder.Services.AddScoped<IGeneralSettingsRepository, GeneralSettingsRepository>();

	
	// Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo", Version = "v1" });
});
builder.Services.AddSwaggerGen(swagger =>
{
    //This is to generate the Default UI of Swagger Documentation    
    swagger.SwaggerDoc("v2", new OpenApiInfo
    {
        Version = "v1",
        Title = "ASP.NET 5 Web API",
        Description = " ITI Projrcy"
    });

    // To Enable authorization using Swagger (JWT)    
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
    });
    swagger.OperationFilter<SecurityRequirementsOperationFilter>();
    //swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
    //            {
    //                {
    //                new OpenApiSecurityScheme
    //                {
    //                Reference = new OpenApiReference
    //                {
    //                Type = ReferenceType.SecurityScheme,
    //                Id = "Bearer"
    //                }
    //                },
    //                new string[] {}
    //                }
    //            });
});


            var app = builder.Build();


	// Add Scope
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var looggerFactory = services.GetRequiredService<ILoggerFactory>();
var logger = looggerFactory.CreateLogger("app");
try
{
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
   ;
    await DefaultRoles.seedAsync(roleManager);
    await DefaultUser.SeedAdminUserAsync(userManager, roleManager);
    logger.LogInformation("Data seeded");
    logger.LogInformation("App started");

}
catch (Exception ex)
{

    logger.LogWarning(ex, "an error ecured while seeding data");
}

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
	app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors(txt);
            app.MapControllers();

            app.Run();
        }
    }
}