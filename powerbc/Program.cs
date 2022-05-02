using System.Net;
using System.Net.Sockets;

IPAddress[] ipList = Dns.GetHostAddresses(Dns.GetHostName())
    .Where(ip => ip.AddressFamily == AddressFamily.InterNetwork).ToArray();

List<string> origins = new() 
{
    "http://localhost:8080",
    "http://127.0.0.1:8080",
    "http://*:8080",
};

foreach (IPAddress ip in ipList)
{
    origins.Add($"http://{ip}:8080");
}

foreach (var origin in origins)
{
    Console.WriteLine(origin);
}




var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<powerbc.Services.UserService>();
builder.Services.AddSingleton<powerbc.Services.GroupService>();

builder.Services.AddSingleton<powerbc.Shares.JwtHelper>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(builder =>
{
    builder.WithOrigins(origins.ToArray())
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
});

app.UseAuthorization();

app.MapControllers();

app.Run();


