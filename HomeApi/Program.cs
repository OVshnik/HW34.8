using HomeApi.Configuration;
using HomeApi.Mapping;
using System.Reflection;
using FluentValidation;
using HomeApi.Contracts.Validation;
using HomeApi.Data.Repos;
using HomeApi.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Configuration.AddJsonFile("HomeOptions.json").AddJsonFile("appsettings.json").AddJsonFile("appsettings.Development.json");

        var assambly = Assembly.GetAssembly(typeof(MappingProfile));
        builder.Services.AddAutoMapper(assambly);

        builder.Services.AddSingleton<IDeviceRepository, DeviceRepository>();
        builder.Services.AddSingleton<IRoomRepository, RoomRepository>();

        string connection = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<HomeApiContext>(options => options.UseSqlServer(connection), ServiceLifetime.Singleton);

        builder.Services.AddValidatorsFromAssemblyContaining<AddDeviceRequestValidator>();

        builder.Services.Configure<HomeOptions>(builder.Configuration);

        builder.Services.Configure<Address>(builder.Configuration.GetSection("Address"));

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

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}