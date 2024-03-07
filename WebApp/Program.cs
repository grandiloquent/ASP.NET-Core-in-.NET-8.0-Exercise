using Microsoft.Extensions.FileProviders;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

var wwwroot=Path.Combine(builder.Environment.ContentRootPath,"wwwroot");
app.UseStaticFiles(new StaticFileOptions
{
 FileProvider = new PhysicalFileProvider(wwwroot),
});
app.MapGet("/", () =>
{

    return Results.File(Path.Combine(wwwroot,"files.html"),
     "",null,null,null);
});

app.Run("http://192.168.8.189:8080");
