using Dating.API.Extension;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers(option =>
{
    option.ReturnHttpNotAcceptable = true;
}).AddXmlDataContractSerializerFormatters().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyMethod();
        builder.AllowAnyOrigin();
        builder.AllowAnyHeader();
    });

});
builder.Services.ConfigureLibrary(builder.Configuration);
builder.Services.ConfigureDb(builder.Configuration);
builder.Services.ConfigureServices(builder.Configuration);
builder.Services.ConfigurePayment(config);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors();
//}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();

app.Run();