using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.AccessDeniedPath = "/accessdenied";
})
.AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
{
    options.Authority = "https://accounts.google.com";
    options.ClientId = "810781142200-dq66urmgsov8lfn695s9u2f9bmcp5e5r.apps.googleusercontent.com";
    options.ClientSecret = "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";
    options.ResponseType = "code";
    options.UsePkce = true;
    options.SaveTokens = true;

    options.Scope.Clear();
    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.Scope.Add("email");

    options.GetClaimsFromUserInfoEndpoint = true;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = ClaimTypes.Name, 
        RoleClaimType = ClaimTypes.Role  
    };

    options.Events = new OpenIdConnectEvents
    {
        OnTokenValidated = context =>
        {
            var identity = (ClaimsIdentity)context.Principal.Identity;
            var email = identity.FindFirst(ClaimTypes.Email)?.Value;

            Console.WriteLine($"User logged in: {email}");

            if (!string.IsNullOrEmpty(email))
            {
                if (email.Equals("eduarthajko1992@gmail.com", StringComparison.OrdinalIgnoreCase))
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, "Admin")); 
                }
            }

            return Task.CompletedTask;
        }
    };
});

// Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAuthenticated", policy =>
    {
        policy.RequireAuthenticatedUser();
    });

    options.AddPolicy("RequireAdmin", policy =>
    {
        policy.RequireRole("Admin"); 
    });
});

// Reverse Proxy
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// Rate Limiting
builder.Services.AddRateLimiter(rateLimiterOptions =>
{
    rateLimiterOptions.AddFixedWindowLimiter("fixed", options =>
    {
        options.Window = TimeSpan.FromSeconds(10);
        options.PermitLimit = 5;
    });
});

var app = builder.Build();

// Middleware
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.MapReverseProxy();
// Routes
app.MapGet("/", () => "Hello World! Go to /admin to test admin area.");

app.MapGet("/admin", (ClaimsPrincipal user) =>
{
    var email = user.FindFirst(ClaimTypes.Email)?.Value;
    return $"Welcome Admin! Your email is {email}";
}).RequireAuthorization("RequireAdmin");

app.MapGet("/accessdenied", () => Results.Content(
    "<h1>Access Denied</h1><p>You do not have permission to access this page.</p>",
    "text/html"
));

app.MapGet("/claims", (ClaimsPrincipal user) =>
{
    var claims = user.Claims.Select(c => $"{c.Type}: {c.Value}");
    return Results.Content(string.Join("<br>", claims), "text/html");
}).RequireAuthorization();

app.MapGet("/logout", async (HttpContext context) =>
{
    await context.SignOutAsync();
    return Results.Redirect("/");
});

app.Run();
