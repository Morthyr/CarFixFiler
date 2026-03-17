using CarFixFiler.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CarFixFiler.Data;
using CarFixFiler.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<CarFixFilerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CarFixFilerContext") ?? throw new InvalidOperationException("Connection string 'CarFixFilerContext' not found.")));

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<IServiceService, ServiceService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
