using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using WebApp;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

var wwwroot = Path.Combine(builder.Environment.ContentRootPath, "wwwroot");


app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(wwwroot),
});
app.MapGet("/", () =>
{
    var f = Path.Combine(wwwroot, "files.html");
    return Results.File(f,
        Shared.GetMimeTypeForFileExtension(f));
});
app.MapGet("/files", ([FromQuery(Name = "path")] string path) =>
{
    if (Directory.Exists(path))
    {
        var files = new List<dynamic>();
        foreach (var fsi in Directory.GetFileSystemEntries(path).Select(v => new FileInfo(v)))
        {
            files.Add(new
            {
                IsDir = ((fsi.Attributes & FileAttributes.Directory) == FileAttributes.Directory),
                Name = fsi.Name,
                Parent = fsi.DirectoryName,
                LastModified = new DateTimeOffset(fsi.LastWriteTimeUtc).ToUnixTimeSeconds(),
                Length = ((fsi.Attributes & FileAttributes.Directory) == FileAttributes.Directory) ? 0 : fsi.Length
            });
        }

        return Task.FromResult(Results.Ok(files));
    }

    return Task.FromResult(Results.NotFound());
});

app.MapGet("/file", ([FromQuery(Name = "path")] string path) =>
{
    if (File.Exists(path))
    {
        return Results.File(path, Shared.GetMimeTypeForFileExtension(path), null, null, null, true);
    }

    return Results.NotFound();
});
app.MapPost("/file/delete", async (HttpRequest request) =>
{
    string body = "";
    using (StreamReader stream = new StreamReader(request.Body))
    {
        body = await stream.ReadToEndAsync();
    }

    var fileList = JsonSerializer.Deserialize<List<string>>(body);
    foreach (var s in fileList)
    {
        if (File.Exists(s))
            File.Delete(s);
        if (Directory.Exists(s))
            Directory.Delete(s, true);
    }
});
app.Run("http://192.168.8.189:8080");