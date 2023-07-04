using Blazored.LocalStorage;
using Chat.Blazor.Server.Helpers.Interfaces;
using Chat.Blazor.Server.Helpers.Implementation;
using Chat.Blazor.Server.Helpers.StateContainers;
using Chat.Blazor.Server.Services.Interfaces;
using Chat.Blazor.Server.Services.Implementation;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using MudBlazor.Services;
using Chat.Blazor.Server.Helpers.StateContainers.Hubs;
using Syncfusion.Blazor;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

var key = builder.Configuration.GetSection("SyncfussionKey").Value;
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(key);
builder.Services.AddSyncfusionBlazor();

builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(option => { option.DetailedErrors = true; })
    .AddHubOptions(x => x.MaximumReceiveMessageSize = 102400000); 
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddAuthorizationCore();
builder.Services.AddMudServices(c => { c.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight; });
builder.Services.AddHttpClient();

builder.Services.AddSingleton<UserStateContainer>();
builder.Services.AddSingleton<GroupInfoStateContainer>();
builder.Services.AddSingleton<AvatarStateContainer>();
builder.Services.AddSingleton<PeerIdStateContainer>();
builder.Services.AddSingleton<VideoCallsSubStateContainer>();
builder.Services.AddSingleton<CallParamsStateContainer>();
builder.Services.AddSingleton<VideoCallHubStateContainer>();

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddSingleton<IImageService, ImageService>();
builder.Services.AddScoped<ICustomHttpClient, CustomHttpClient>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITestService, TestService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<IFIleService, FileService>();
builder.Services.AddScoped<IPdfContractService, PdfContractService>();
builder.Services.AddScoped<IHubConnectionService, HubConnectionService>();
builder.Services.AddScoped<IChatMessageService, ChatMessageService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
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
});
app.Run();
