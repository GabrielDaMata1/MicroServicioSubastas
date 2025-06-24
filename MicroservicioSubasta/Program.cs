using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using System.Reflection;
using Application.Command;
using Application.External_Services.MassTransit;
using Application.Service;
using Domain.Interfaces;
using Infrastructure.Consumers;
using MassTransit;
using Infrastructure.Repositories.MongoDB;
using Infrastructure.Repositories.PostgreSQL;
using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Dto;
using Application.External_Services.Hangfire;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Mi API",
        Version = "v1",
        Description = "Documentaci�n de mi API usando Swagger"
    });
});


builder.Services.AddDbContext<SubastaDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection"),
        b => b.MigrationsAssembly("Infrastructure")));


var mongoClient = new MongoClient("mongodb://localhost:27017");
builder.Services.AddSingleton<IMongoClient>(mongoClient);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.AddScoped<ISubastaRepositoryMongo, SubastaMongoRepository>();
builder.Services.AddScoped<ISubastaRepositoryPostgreSQL, SubastaPostgreSQLRepository>();
builder.Services.AddScoped<IHistorialSubastaMongoRepository, HistorialSubastaMongoRepository>();
builder.Services.AddScoped<IHistorialSubastaPostgreSQLRepository, HistorialSubastaPostgreSQLRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<ISubastaService, SubastaService>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IPujaService, PujaService>();
builder.Services.AddScoped<ISubastaSchedule, SubastaSchedule>();


builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RegistrarSubastaCommand).Assembly));


builder.Services.AddHttpClient<UsuarioService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5001/api/usuarios/");
});
builder.Services.AddHttpClient<ProductoService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5001/api/Productos/");
});

builder.Services.AddHttpClient<PujaService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5004/api/Pujas/");
});




builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<SubastaRegistradaConsumer>();
    x.AddConsumer<SubastaModificadaConsumer>();
    x.AddConsumer<SubastaEliminadaConsumer>();
    x.AddConsumer<SubastaActivaConsumer>();
    x.AddConsumer<SubastaAcabadaConsumer>();

    x.AddSagaStateMachine<SubastaStateMachine, SubastaState>()
        .MongoDbRepository(r =>
        {
            r.Connection = "mongodb://localhost:27017"; 
            r.DatabaseName = "MassTransitBD";
            r.CollectionName = "MassTransit";
        });


    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });


        cfg.ReceiveEndpoint("subasta-registrada-queue", e =>
        {
            e.ConfigureConsumer<SubastaRegistradaConsumer>(context);
        });

        cfg.ReceiveEndpoint("subasta-modificada-queue", e =>
        {
            e.ConfigureConsumer<SubastaModificadaConsumer>(context);
        });

        cfg.ReceiveEndpoint("subasta-eliminada-queue", e =>
        {
            e.ConfigureConsumer<SubastaEliminadaConsumer>(context);
        });

        cfg.ReceiveEndpoint("subasta-state-saga", e =>
        {
            e.ConfigureSaga<SubastaState>(context);
        });

        cfg.ReceiveEndpoint("subasta-activa-queue", e =>
        {
            e.ConfigureConsumer<SubastaActivaConsumer>(context);
        });

        cfg.ReceiveEndpoint("subasta-acabada-queue", e =>
        {
            e.ConfigureConsumer<SubastaAcabadaConsumer>(context);
        });
    });

});

builder.Services.AddHangfire(config =>
{
    config.UseMongoStorage(
        "mongodb://localhost:27017",
        "HangFireBD",
        new MongoStorageOptions
        {
            MigrationOptions = new MongoMigrationOptions
            {
                MigrationStrategy = new MigrateMongoMigrationStrategy(),
                BackupStrategy = new CollectionMongoBackupStrategy()
            },
            CheckQueuedJobsStrategy = CheckQueuedJobsStrategy.TailNotificationsCollection
        });
});

builder.Services.AddHangfireServer();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mi API v1");
    });
}
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.UseHangfireDashboard("/hangfire");


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
