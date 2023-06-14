using PH.UberConnect.Webhook.AppSettings;
using PH.UberConnect.Core.Service;
using PH.UberConnect.Webhook.Extensions;

SerilogExtension.AddSerilog();

var builder = WebApplication.CreateBuilder(args);
string allowAll = "AllowAll";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowAll,
                      builder =>
                      {
                          builder
                          .SetIsOriginAllowed(origin => true)
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowAnyOrigin();
                      });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add a Singleton to use the signing keys
var signingKeys = builder.Configuration.GetSection("SigningKeys").Get<SigningKeys>();
builder.Services.AddSingleton(signingKeys);

// Add a Singleton to use the PH services
var services = new Services(builder.Configuration.GetConnectionString("PHCRSRVMCARIIS0"));
builder.Services.AddSingleton(services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(allowAll);

app.UseAuthorization();

app.MapControllers();

app.Run();
