using BTL_WEBBH.Models.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("sqlCon")));


builder.Services.AddDistributedMemoryCache();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


builder.Services.Configure<CookiePolicyOptions>(options => {
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddAuthentication("bkapschema").AddCookie("bkapschema", options => {
//đường dẫn cấm truy cập
//options.AccessDeniedPath = new PathString("/");
options.Cookie = new CookieBuilder
{
    HttpOnly = true,
    Name = "c2208g.cookie",
    Path = "/",
    SameSite = SameSiteMode.Lax,
    SecurePolicy = CookieSecurePolicy.SameAsRequest
};
    //phát sinh sự kiện
    //options.Events=new Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationEvents
    //{
    //    //OnSignedIn = context => { }
    //}

    //chỉ định đường dẫn login
    options.LoginPath = "/Admin/Account/Login";
    options.ReturnUrlParameter = "requestUrl";
    options.SlidingExpiration = true;
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.IsEssential = true;
});

var app = builder.Build();
app.UseHttpsRedirection();
/*app.UseStatusCodePagesWithRedirects("/Home/Error?statuscode={0}");*/

app.UseSession();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCookiePolicy();

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Product}/{action=Index}/{id?}"
    );
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
