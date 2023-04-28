using Blazored.LocalStorage;
using Chat.Blazor.Server.Configurations;
using Chat.Blazor.Server.Data;
using Chat.Blazor.Server.Helpers.Interfaces;
using Chat.Blazor.Server.Helpers.Realization;
using Chat.Blazor.Server.Helpers.StateContainers;
using Chat.Blazor.Server.Hubs;
using Chat.Blazor.Server.Services.Interfaces;
using Chat.Blazor.Server.Services.Realization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddMudServices(c => { c.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight; });
builder.Services.AddHttpClient();
builder.Services.AddSignalR();
builder.Services.AddSingleton<UserStateContainer>();
builder.Services.AddSingleton<GroupInfoStateContainer>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddSingleton<IImageService, ImageService>();
builder.Services.AddScoped<ICustomHttpClient, CustomHttpClient>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITestService, TestService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<IChatMessageService, ChatMessageService>();

builder.Services.AddSingleton<WeatherForecastService>();

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


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<ChatHub>("/chatHub");
});
app.Run();
