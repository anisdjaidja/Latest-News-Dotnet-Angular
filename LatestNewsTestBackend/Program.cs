using CacheCow.Server;
using CacheCow.Server.Core.Mvc;
using LatestNewsTestBackend.DataAcess;
using LatestNewsTestBackend.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
/// Configuration for the background fetching thread 
/// to enable smooth silent data fetching
builder.Services.Configure<HostOptions>(builder =>
{
    builder.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
    builder.ServicesStartConcurrently = true;
    builder.ServicesStopConcurrently = false;
    
});

/// Configure CORS policy to only allow 
/// our client app to hit our enpoints
builder.Services.AddCors(options =>
{
    options.AddPolicy("CORSPolicy",
        builder =>
        {
            builder
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithOrigins("http://localhost:4200");
        });
});
builder.Services.AddHttpCachingMvc();
// Add services to the container.
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
/// Adding a DBContext factory instead of DbContext 
/// ensures a managed 'on demand' accecibility for other services
builder.Services.AddDbContextFactory<NewsContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLServer"));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
    options.LogTo(Console.WriteLine, LogLevel.Warning);
});
builder.Services.AddHostedService<NewsFetchService>();
builder.Services.AddSingleton<NewsService>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("CORSPolicy");

//app.UseAuthorization();

app.MapControllers();

app.Run();
