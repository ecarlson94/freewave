using EventsTakeTheWheel.BFF.Features.ChargeController;
using EventsTakeTheWheel.BFF.Services;
using MongoDB.Driver;
using MongoDB.Entities;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

ConfigureEndpoints(app);

app.Run();

void ConfigureServices(IServiceCollection services, ConfigurationManager configuration)
{
    // Configure MongoDb
    var dbName = configuration.GetValue<string>("MongoDbSettings:DatabaseName") ?? "default";
    var dbConnectionString = configuration.GetValue<string>("MongoDbSettings:ConnectionString");
    var dbSettings = MongoClientSettings.FromConnectionString(dbConnectionString);
    var dbContext = new DBContext(dbName, dbSettings);
    services.AddSingleton(x => dbContext);

    // Configure gRPC
    services.AddGrpc();
}

void ConfigureEndpoints(IEndpointRouteBuilder app)
{
    app.MapGrpcService<GreeterService>();
    app.MapGrpcService<ChargeControllersService>();

    // Configure the HTTP request pipeline.
    app.MapGet(
        "/",
        () =>
            "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909"
    );
}
