using makefriends_web_api.Database;

var builder = WebApplication.CreateBuilder(args);
var CorsPolicy = "_allowOrigins";

// Add services to the container.

builder.Services.Configure<UserDatabaseSettings>(
    builder.Configuration.GetSection("UserDatabase")
);

builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicy, policy =>
    {
        policy.WithOrigins("http://localhost:3000");
        policy.AllowAnyHeader();
    });
});

builder.Services.AddSingleton<UserService>();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseCors(CorsPolicy);

app.UseAuthorization();

app.MapControllers();

app.Run();
