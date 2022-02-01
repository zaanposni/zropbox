using CDN.Data;
using CDN.Exceptions;
using CDN.Logger;
using CDN.Middlewares;
using CDN.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();

builder.Logging.AddProvider(new LoggerProvider());

string connectionString =
            $"Server={   Environment.GetEnvironmentVariable("MYSQL_HOST")};" +
            $"Port={     Environment.GetEnvironmentVariable("MYSQL_PORT")};" +
            $"Database={ Environment.GetEnvironmentVariable("MYSQL_DATABASE")};" +
            $"Uid={      Environment.GetEnvironmentVariable("MYSQL_USER")};" +
            $"Pwd={      Environment.GetEnvironmentVariable("MYSQL_PASSWORD")};";

builder.Services.AddDbContext<DataContext>(x => x.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddSingleton<InternalConfiguration>();

string jwtToken = Environment.GetEnvironmentVariable("JWT_KEY");
if (jwtToken == null) throw new InvalidConfigurationException("JWT_KEY not found.");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "cdn",
            ValidAudience = "cdn",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtToken))
        };
    });

var app = builder.Build();

app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<APIExceptionHandlingMiddleware>();

using (var scope = app.Services.CreateScope())
{
scope.ServiceProvider.GetRequiredService<DataContext>().Database.Migrate();
}

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<InternalConfiguration>().Init();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
