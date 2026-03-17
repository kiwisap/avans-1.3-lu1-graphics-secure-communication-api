using lu1_graphics_secure_communication_api.Data;
using lu1_graphics_secure_communication_api.Services;
using lu1_graphics_secure_communication_api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using System.Reflection;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Register MVC controllers for handling HTTP requests.
builder.Services.AddControllers();

// Configure JSON serialization to use camelCase for property names.
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

// Register OpenAPI/Swagger for API documentation and testing.
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Lu1 Graphics Secure Communication API",
        Version = "v1"
    });
});

builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

// Register authorization services for securing endpoints.
builder.Services.AddAuthorization();

// Retrieve the SQL connection string from configuration.
var sqlConnectionString = builder.Configuration.GetValue<string>("SqlConnectionString");

// Register the EF database context with the specified SQL connection string.
builder.Services.AddDbContext<ITCureDbContext>(options =>
{
    options.UseSqlServer(sqlConnectionString);
});

// Register ASP.NET Core Identity with Dapper stores for user authentication and management.
// Configures password and user requirements.
builder.Services.AddIdentityApiEndpoints<IdentityUser>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequiredLength = 10;
})
.AddRoles<IdentityRole>()
.AddDapperStores(options => options.ConnectionString = sqlConnectionString);

// Register IHttpContextAccessor for accessing HTTP context in services (e.g., to get current user info).
builder.Services.AddHttpContextAccessor();

// Register services
builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();

var app = builder.Build();

// Register OpenAPI/Swagger endpoints.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Lu1 Graphics Secure Communication API v1");
        options.RoutePrefix = "swagger"; // Access at /swagger
        options.CacheLifetime = TimeSpan.Zero; // Disable caching for development

        // Inject a warning in the Swagger UI if the SQL connection string is missing
        if (string.IsNullOrWhiteSpace(sqlConnectionString))
        {
            options.HeadContent = "<h1 align=\"center\">❌ SqlConnectionString not found ❌</h1>";
        }
    });
}
else
{
    // Show the health message directly in non-development environments
    var buildTimeStamp = File.GetCreationTime(Assembly.GetExecutingAssembly().Location);
    var currentHealthMessage = $"The API is up 🚀 | Connection string found: {(string.IsNullOrWhiteSpace(sqlConnectionString) ? "✅" : "❌")} | Build timestamp: {buildTimeStamp}";

    app.MapGet("/", () => currentHealthMessage);
}

// Enforce HTTPS for all requests.
app.UseHttpsRedirection();

// Enable authentication middleware.
app.UseAuthentication();

// Enable authorization middleware.
app.UseAuthorization();

// Register Identity endpoints for account management (register, login, etc.) under /api/account.
app.MapGroup("/api/account").MapIdentityApi<IdentityUser>().WithTags("Account");

// Register all controller endpoints for the application.
app.MapControllers();

app.Run();
