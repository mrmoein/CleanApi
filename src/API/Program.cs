using CleanApi.API;
using CleanApi.Infrastructure.Persistence;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApiServices();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();

    // Initialise and seed database
    using IServiceScope scope = app.Services.CreateScope();
    ApplicationDbContextInitialiser initialiser =
        scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
    await initialiser.InitialiseAsync();
    await initialiser.SeedAsync();

    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHsts();
}

app.UseHealthChecks("/health");
app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    "default",
    "{controller}/{action=Index}/{id?}");

app.Run();