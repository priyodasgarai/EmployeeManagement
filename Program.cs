using Employee.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddInfrastructureService(builder.Configuration);
builder.Services.AddApplicationService(builder.Configuration);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserPolicy", policy => policy.RequireRole("User"));
    options.AddPolicy("StorePolicy", policy => policy.RequireRole("Store"));
});

//Add Serilog for Loging---------------------------------------
Log.Logger = new LoggerConfiguration()
.Enrich.FromLogContext()
.WriteTo.Console()
.WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
.CreateLogger();

builder.Host.UseSerilog();
Log.Logger.Information("Application is building.....");
//---------------------Serilog-----------------------------------
var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseInfrastructructureService();
app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
            Path.Combine(builder.Environment.ContentRootPath, "Uploads")),
    RequestPath = "/Resources"
});
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
