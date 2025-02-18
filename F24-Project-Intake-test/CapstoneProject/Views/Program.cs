using CapstoneProject.Models.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddSingleton<WebService>();
builder.Services.AddSingleton<EncryptionHelper>();



var app = builder.Build();
string connectionString = builder.Configuration.GetConnectionString("Connection_Database");
app.UsePathBase("/cis4396-f04");
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
// Enable session middleware first
app.UseSession();

app.UseMiddleware<RoleAuthorizationMiddleware>();



app.UseRouting();

app.UseAuthorization();

string routePrefix = app.Environment.IsProduction() ? "cis4396-f04" : "";
app.MapControllerRoute(
  name: "default",
pattern: "{controller=Login}/{action=Login}/{id?}");

app.Run();

//string routePrefix = app.Environment.IsProduction() ? "cis4396-f04" : "";
//app.MapControllerRoute(
//  name: "default",
//pattern: "{controller=Login}/{action=Login}/{id?}");