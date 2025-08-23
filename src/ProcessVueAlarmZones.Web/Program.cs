using ProcessVueAlarmZones.Application.Config;
using ProcessVueAlarmZones.Application.Interface;
using ProcessVueAlarmZones.Application.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


/// Bind geometry from config
builder.Services.AddOptions<EemuaGeometry>()
    .BindConfiguration(nameof(EemuaGeometry))
    .Validate(g => g.X1 < g.X2 && g.X2 < g.X3 && g.YTop > 0, "Invalid EemuaGeometry")
    .ValidateOnStart();

// Classifier
builder.Services.AddSingleton<IEemuaZoneClassifier, EemuaZoneClassifier>();

// (Optional) quick check
var g = builder.Configuration.GetSection(nameof(EemuaGeometry)).Get<EemuaGeometry>();
Console.WriteLine($"EemuaGeometry: X={g?.X1},{g?.X2},{g?.X3} YTop={g?.YTop}");


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
