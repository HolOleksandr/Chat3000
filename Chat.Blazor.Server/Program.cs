using Blazored.LocalStorage;
using Chat.Blazor.Server.Configurations;
using Chat.Blazor.Server.Data;
using Chat.Blazor.Server.Helpers.Interfaces;
using Chat.Blazor.Server.Helpers.Realization;
using Chat.Blazor.Server.Services.Interfaces;
using Chat.Blazor.Server.Services.Realization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddAuthorizationCore();
builder.Services.AddTransient<TokenMiddleware>();
builder.Services.AddHttpClient<ITestService, TestService>().AddHttpMessageHandler<TokenMiddleware>();

builder.Services.AddHttpClient();
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<IAuthService, AuthService>();
//builder.Services.AddScoped<ICustomHttpClient, CustomHttpClient>();
//builder.Services.AddScoped<ITestService, TestService>();
builder.Services.AddSingleton<WeatherForecastService>();

builder.Services.AddBlazoredLocalStorage();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
//app.UseMiddleware<TokenMiddleware>();
app.Run();
