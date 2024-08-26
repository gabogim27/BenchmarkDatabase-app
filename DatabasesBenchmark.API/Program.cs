using DatabasesBenchmark.API.Middlewares.Extensions;
using DatabasesBenchmark.Infrastructure.DI;
using DatabasesBenchmark.Services.DI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddInfrastructureServicesToDI(builder.Configuration);
builder.Services.AddServicesToDI(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

app.UseExceptionMiddleware();

app.UseAuthorization();

app.MapControllers();

app.Run();
