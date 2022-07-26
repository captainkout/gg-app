using System.Numerics;
using Api.Services;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddTransient<IFibService<int>, IntFibService>();
builder.Services.AddTransient<IFibService<long>, LngFibService>();
builder.Services.AddTransient<IFibService<BigInteger>, BigIntFibService>();
builder.Services.AddSingleton<AppCacheService>();
builder.Services.AddSingleton<AppStateService>();
builder.Services.AddHostedService<AppMonitorService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(AppProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
