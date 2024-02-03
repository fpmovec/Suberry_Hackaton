using Syberry.Web;

using System.Text.Json.Serialization;
using Syberry.Web.Services.Abstractions;
using Syberry.Web.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("CommonFactory", _ => { })
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = (_, _, _, _) => true
    });

builder.Services.AddScoped<IBelarusBankService, BelarusBankService>();

builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection(AppSettings.SectionName));
builder.Services.AddStackExchangeRedisCache(config =>
{
    config.Configuration = "localhost:8001";
    config.InstanceName = "local";
});

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
