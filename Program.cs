var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<HipcallManagement.Models.HipcallSettings>(builder.Configuration.GetSection("HipcallSettings"));

builder.Services.AddHttpClient<HipcallManagement.Services.IHipcallApiService, HipcallManagement.Services.HipcallApiService>((serviceProvider, client) =>
{
    var settings = serviceProvider.GetRequiredService<Microsoft.Extensions.Options.IOptions<HipcallManagement.Models.HipcallSettings>>().Value;
    if (!string.IsNullOrEmpty(settings.BaseUrl))
    {
        client.BaseAddress = new Uri(settings.BaseUrl);
    }
    if (!string.IsNullOrEmpty(settings.ApiToken))
    {
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", settings.ApiToken);
    }
});
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
