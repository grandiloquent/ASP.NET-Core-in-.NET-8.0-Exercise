using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using WebApp;
using System.Diagnostics;


var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

var wwwroot = Path.Combine(builder.Environment.ContentRootPath, "wwwroot");


app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(wwwroot),
});
app.MapGet("/", (HttpRequest request) =>
{
    // foreach (var header in request.Headers){
    // Console.WriteLine($"{header.Key} {header.Value}");
    // }
    var f = Path.Combine(wwwroot, "files.html");
    return Results.File(f,
        Shared.GetMimeTypeForFileExtension(f));
});
app.MapGet("/files", ([FromQuery(Name = "path")] string path, int? isSize = 0) =>
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
                LastModified = new DateTimeOffset(fsi.CreationTime).ToUnixTimeSeconds(),
                Length = ((fsi.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
                    ? (isSize == 0
                        ? 0
                        : new DirectoryInfo(fsi.FullName).EnumerateFiles("*.*", SearchOption.AllDirectories)
                            .Sum(f => f.Length))
                    : fsi.Length
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
app.MapGet("/open", ([FromQuery(Name = "path")] string path) =>
{
    if (File.Exists(path))
    {
       Process.Start(new System.Diagnostics. ProcessStartInfo(){
		FileName="explorer.exe",
		Arguments=$"\"{path}\""
		});
        return Results.Ok();
    }

    return Results.NotFound();
});
app.MapPost("/file/delete", async (HttpRequest request) =>
{
    var body = "";
    using (var stream = new StreamReader(request.Body))
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
app.MapPost("/file/move", async (HttpRequest request, string dst) =>
{
    var body = "";
    using (var stream = new StreamReader(request.Body))
    {
        body = await stream.ReadToEndAsync();
    }

    var fileList = JsonSerializer.Deserialize<List<string>>(body);
    foreach (var s in fileList)
    {
        var f = Path.Combine(dst, Path.GetFileName(s));
        if (File.Exists(s) && !File.Exists(f))
            File.Move(s, f);
        if (Directory.Exists(s) && !Directory.Exists(f))
            Directory.Move(s, f);
    }
});

 
app.MapGet("/movevideo", (string path) =>
{
    var dst=Path.Combine("D:\\Blender",Path.GetFileName(path));
    File.Copy(path,dst);
    if(File.Exists(dst)){
        File.Delete(path);
    }
    return File.Exists(dst) ? Results.Ok() : Results.NotFound();
});
app.MapGet("/file/rename", (string path, string dst) =>
{
    if (File.Exists(path) && !File.Exists(dst))
    {
        File.Move(path, dst);
    }

    if (Directory.Exists(path) && !Directory.Exists(dst))
    {
        Directory.Move(path, dst);
    }
});
app.MapGet("/file/new_dir", (string path) =>
{
    if (!Directory.Exists(path))
        Directory.CreateDirectory(path);
    return Directory.Exists(path) ? Results.Ok() : Results.NotFound();
});
app.MapGet("/file/new_file", (string path,string? uri) =>
{
    if(uri!=null){
         Process.Start(new System.Diagnostics. ProcessStartInfo(){
		FileName="aria2c.exe",
		Arguments=$"-c \"{uri}\"",WorkingDirectory=path
		});
        return Results.Ok();
    }
    if (!File.Exists(path))
        File.Create(path).Dispose();
    return File.Exists(path) ? Results.Ok() : Results.NotFound();
});


app.MapGet("/tidy", (string path) =>
{
    if (!Directory.Exists(path))
    {
        return Results.NotFound();
    }

    var files = Directory.GetFiles(path);
    foreach (var element in files)
    {
        var f = Path.Combine(path, Path.GetExtension(element).ToUpper());
        if (!Directory.Exists(f))
            Directory.CreateDirectory(f);
        f = Path.Combine(f, Path.GetFileName(element));
        if (!File.Exists(f))
            File.Move(element, f);
    }

    return Results.Ok();
});

string GetLocalIPAddress()
{
    var host = Dns.GetHostEntry(Dns.GetHostName());
    foreach (var ip in host.AddressList)
    {
        if (ip.AddressFamily == AddressFamily.InterNetwork)
        {
            return ip.ToString();
        }
    }

    throw new Exception("No network adapters with an IPv4 address in the system!");
}
// {GetLocalIPAddress()}
app.Run($"http://0.0.0.0:8080");