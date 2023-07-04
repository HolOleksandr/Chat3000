using ChatApp.API.Configurations;
using ChatApp.API.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureAuthorizationPolicies();
builder.Services.ConfigureCors();
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJWT(builder);
builder.Services.ConfigureSwagger();
builder.Services.ConfigureValidators();
builder.Services.RegisterDependencies();

//builder.SetUpKeyVaultConfig(); // restore when Azure account will be registered
builder.ConfigureDbContext();
builder.ConfigureSignalRConnection();

builder.Services.ConfigureMapping();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseCors("CorsDefault");
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<ChatHub>("/chathub");
    endpoints.MapHub<VideoCallsHub>("/videocallhub");
});
app.Run();
