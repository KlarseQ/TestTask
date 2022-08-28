using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using TestTask.DBStuff;
using TestTask.DBStuff.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddMvc();
builder.Services.AddMvc().AddControllersAsServices();
builder.Services.AddHttpContextAccessor();
string connectionString = @"server=localhost;user=root;database=Url;password=666;port=3306";
var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));
builder.Services.AddDbContext<MyDBContext>(options => options.UseMySql(connectionString, serverVersion));
builder.Services.AddScoped<UrlReporitory>(x =>
                new UrlReporitory(x.GetService<MyDBContext>()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<MyDBContext>();
    dataContext.Database.Migrate();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
    );
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.MapRazorPages();

app.Run();
