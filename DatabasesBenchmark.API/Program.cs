using DatabasesBenchmark.Infrastructure.DbContext;
using DatabasesBenchmark.Infrastructure.Repositories.Implementations;
using DatabasesBenchmark.Infrastructure.Repositories.Interfaces;
<<<<<<< HEAD
=======
using DatabasesBenchmark.Services.Helpers.Implementations;
using DatabasesBenchmark.Services.Helpers.Interfaces;
>>>>>>> 4b81f786cd0f9757a173c02912df4cf971c60ab8
using DatabasesBenchmark.Services.Implementations;
using DatabasesBenchmark.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<MySqlBenchmarkDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("MySQLConnection"), new MySqlServerVersion(new Version(8, 0, 23))));
builder.Services.AddDbContext<PostgreBenchmarkDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQLConnection")));

builder.Services.AddScoped<IBenchmarkDbContextFactory, BenchmarkDbContextFactory>();
builder.Services.AddScoped<IBenchmarkService, BenchmarkService>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddScoped<MySqlBenchmarkDbContext>();
builder.Services.AddScoped<PostgreBenchmarkDbContext>();

<<<<<<< HEAD
=======
builder.Services.AddScoped<IStringHelper, StringHelper>();

>>>>>>> 4b81f786cd0f9757a173c02912df4cf971c60ab8
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

app.UseAuthorization();

app.MapControllers();

app.Run();
