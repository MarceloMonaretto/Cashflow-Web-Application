using AccountRepositoryLib.Connection;
using AccountRepositoryLib.Repositories;
using Microsoft.EntityFrameworkCore;
using RoleRepositoryLib.Connection;
using RoleRepositoryLib.Repositories;
using TransactionRepositoryLib.Connection;
using TransactionRepositoryLib.Data;
using AppContextLib.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<PopulateDatabaseLib.IDatabasePopulator, PopulateDatabaseLib.DatabasePopulator>();

builder.Services.AddScoped<ITransactionRepositoryConnection, TransactionRepositoryConnection>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

builder.Services.AddScoped<IRoleRepositoryConnection, RoleRepositoryConnection>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();

var contextBuilder = new AppContextLib.Data.DbContextOptionsFactory();

builder.Services.AddScoped<IAccountRepositoryConnection, AccountRepositoryConnection>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();

//builder.Services.AddDbContext<AppContextLib.Data.AppContext>(options =>
//options.UseSqlServer("server=localhost;database=CashFlowDb;Trusted_Connection=True;MultipleActiveResultSets=True"));

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
    await service.PopulateRolesAsync();
    await service.PopulateAccountsAsync();
}

app.MapControllers();

app.Run();
