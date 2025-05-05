using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using YarpApiGateway.Telemetry;

var builder = WebApplication.CreateBuilder(args);


// Reverse Proxy
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
// Authentication
builder.Services.AddAuthentication(BearerTokenDefaults.AuthenticationScheme).AddBearerToken();



builder.Services.AddAuthorization(options =>
{
    // default policy
    options.AddPolicy("access", p =>
        p.RequireAuthenticatedUser());

    // user policy
    options.AddPolicy("user-access", p =>
        p.RequireAuthenticatedUser()
         .RequireClaim("role", "user"));

    // admin policy
    options.AddPolicy("admin-access", p =>
        p.RequireAuthenticatedUser()
         .RequireClaim("role", "admin"));
});
// Rate Limiting
builder.Services.AddRateLimiter(rateLimiterOptions =>
{
    rateLimiterOptions.AddFixedWindowLimiter("fixed", options =>
    {
        options.Window = TimeSpan.FromSeconds(10);
        options.PermitLimit = 5;
    });
});

// Add OpenTelemetry services
builder.Services.AddOpenTelemetryServices(builder.Configuration);
var app = builder.Build();
// Login endpoint - issues a bearer token for local testing
app.MapPost("/login", (string username, string password, string role="user") =>
{
    if (username == "admin" && password == "pass")
    {
        // Authenticate and issue bearer token
        return Results.SignIn(
            new ClaimsPrincipal(
                new ClaimsIdentity(new[]
                {
                    new Claim("id", Guid.NewGuid().ToString()),
                    new Claim("ts", DateTime.UtcNow.ToString("O")),
                    new Claim("sub", Guid.NewGuid().ToString()),
                    new Claim("username", username),
                    new Claim("role", role) 
                }, authenticationType: BearerTokenDefaults.AuthenticationScheme)
            ),
            authenticationScheme: BearerTokenDefaults.AuthenticationScheme
        );
    }

    return Results.Unauthorized();
});

// Middleware
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.MapReverseProxy();


app.Run();
