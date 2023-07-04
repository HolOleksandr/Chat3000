using Blazored.LocalStorage;
using Chat.Blazor.WebAssembly;
using Chat.Blazor.WebAssembly.Helpers.Implementation;
using Chat.Blazor.WebAssembly.Helpers.Interfaces;
using Chat.Blazor.WebAssembly.Helpers.StateContainers;
using Chat.Blazor.WebAssembly.Helpers.StateContainers.Hubs;
using Chat.Blazor.WebAssembly.Services.Implementation;
using Chat.Blazor.WebAssembly.Services.Interfaces;
using IndexedDB.Blazor;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using Syncfusion.Blazor;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

var key = builder.Configuration.GetSection("SyncfussionKey").Value;
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(key);
builder.Services.AddSyncfusionBlazor();

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddAuthorizationCore();
builder.Services.AddMudServices(c => { c.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight; });
builder.Services.AddHttpClient();

builder.Services.AddSingleton<IIndexedDbFactory, IndexedDbFactory>();

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

await builder.Build().RunAsync();