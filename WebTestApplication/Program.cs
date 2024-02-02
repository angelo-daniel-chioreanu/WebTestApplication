using Microsoft.EntityFrameworkCore;
using WebTestApplication.Data;
using WebTestApplication.Utilities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
});

builder.Services.AddDbContext<TestApplicationContext>(options =>
    options.UseInMemoryDatabase("TestDatabase"));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = scope.ServiceProvider.GetRequiredService<TestApplicationContext>();
    context.Database.EnsureCreated();
    DbInitializer.Initialize(context);
}

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action}/{id?}");

app.MapGet("/", () => "Test Web API");

app.Run();
