using Zropbox.Data;
using Zropbox.Exceptions;
using Zropbox.Logger;
using Zropbox.Middlewares;
using Zropbox.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Zropbox.Repositories;
using Zropbox.Models;

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

if (string.Equals("true", Environment.GetEnvironmentVariable("ENABLE_CORS")))
{
    builder.Services.AddCors(o => o.AddPolicy("DevCors", builder =>
    {
        builder.WithOrigins("http://127.0.0.1:5000")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    }));
}

var app = builder.Build();

app.UseMiddleware<HeaderMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<APIExceptionHandlingMiddleware>();

if (string.Equals("true", Environment.GetEnvironmentVariable("ENABLE_CORS")))
{
    Console.WriteLine("Activating CORS support");
    app.UseCors("DevCors");
}

using (var scope = app.Services.CreateScope())
{
    DataContext context = scope.ServiceProvider.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();

    await context.Database.OpenConnectionAsync();

    User existingUser = await context.Users.AsQueryable().OrderBy(x => x.Id).FirstOrDefaultAsync();
    if (existingUser == null)
    {
        Random random = new();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        string password = new string(Enumerable.Repeat(chars, 20).Select(s => s[random.Next(s.Length)]).ToArray());

        await UserRepository.CreateDefault(scope.ServiceProvider).RegisterUser("admin", password, true);

        Console.WriteLine($"==================================================================================");
        Console.WriteLine();
        Console.WriteLine($"No user found in database. Created 'admin' account with password '{password}'.");
        Console.WriteLine();
        Console.WriteLine($"==================================================================================");
    }
    await context.Database.CloseConnectionAsync();
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
