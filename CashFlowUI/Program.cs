using CashFlowUI.Helpers;
using CashFlowUI.HttpClients;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IAccountClient, AccountClient>();
builder.Services.AddScoped<IRoleClient, RoleClient>();
builder.Services.AddScoped<ITransactionClient, TransactionClient>();

builder.Services.AddHttpClient<IAccountClient, AccountClient>(client => {
    client.BaseAddress = new System.Uri("http://localhost:5000/cashflowapi/");
});
builder.Services.AddHttpClient<IRoleClient, RoleClient>(client =>
{
    client.BaseAddress = new System.Uri("http://localhost:5000/cashflowapi/");
});
builder.Services.AddHttpClient<ITransactionClient, TransactionClient>(client =>
{
    client.BaseAddress = new System.Uri("http://localhost:5000/cashflowapi/");
});

builder.Services.AddAuthentication(LoginManager.LoginCookieString).AddCookie(LoginManager.LoginCookieString, options =>
{
    options.Cookie.Name = LoginManager.LoginCookieString;
    options.LoginPath = "/Account/Login";
});

builder.Services.AddScoped<ILoginManager, LoginManager>();
builder.Services.AddScoped<IRolesManager, RolesManager>();
builder.Services.AddScoped<IAccountManager, AccountManager>();
builder.Services.AddScoped<ITransactionManager, TransactionManager>();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
