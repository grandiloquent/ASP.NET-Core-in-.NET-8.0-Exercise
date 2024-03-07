var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/", () =>
{
    return "Hello, world!";
});

app.Run("http://192.168.8.189:8080");
