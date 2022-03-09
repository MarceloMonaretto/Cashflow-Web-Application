using PopulateDatabaseLib;
using TransactionRepositoryLib.Connection;
using TransactionRepositoryLib.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IDatabasePopulator, DatabasePopulator>();

builder.Services.AddScoped<ITransactionRepositoryConnection, TransactionRepositoryConnection>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

var contextBuilder = new AppContextLib.Data.DbContextOptionsFactory();

builder.Services.AddScoped<AppContextLib.Data.IAppContext, AppContextLib.Data.AppContext>((IServiceProvider serviceprovider) =>
{
    return contextBuilder.CreateDbContext(new string[] { builder.Configuration.GetConnectionString("DatabaseConnection") });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

var scopeFactory = app.Services.GetService<IServiceScopeFactory>();

using (var scope = scopeFactory.CreateScope())
{
    var service = scope.ServiceProvider.GetService<PopulateDatabaseLib.IDatabasePopulator>();
    await service.PopulateTransitionsAsync();
}

app.MapControllers();

app.Run();
