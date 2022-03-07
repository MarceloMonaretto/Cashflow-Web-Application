using AccountRepositoryLib.Connection;
using AccountRepositoryLib.Repositories;
using RoleRepositoryLib.Connection;
using RoleRepositoryLib.Repositories;
using AppContextLibrary = AppContextLib.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


builder.Services.AddScoped<IRoleRepositoryConnection, RoleRepositoryConnection>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();


builder.Services.AddScoped<IAccountRepositoryConnection, AccountRepositoryConnection>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<AppContextLibrary.IAppContext, AppContextLibrary.AppContext>((IServiceProvider serviceProvider) =>
{
    return new AppContextLibrary.AppContext(AppContextLibrary.DbContextOptionsFactory.CreateDbContextOptions());
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

app.MapControllers();

app.Run();
