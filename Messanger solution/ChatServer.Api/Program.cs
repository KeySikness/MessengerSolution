using ChatServer.Api.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddControllers();

var app = builder.Build();

app.UseRouting();
app.UseStaticFiles();

app.MapControllers();
app.MapHub<ChatHub>("/chat");

app.Run();
