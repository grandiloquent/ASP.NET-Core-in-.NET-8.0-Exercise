using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

var wwwroot=Path.Combine(builder.Environment.ContentRootPath,"wwwroot");
var provider = new FileExtensionContentTypeProvider();

 string GetMimeTypeForFileExtension(string filePath)
{
 const string DefaultContentType = "application/octet-stream";

 if (!provider.TryGetContentType(filePath, out string contentType))
 {
  contentType = DefaultContentType;
 }

 return contentType;
}
app.UseStaticFiles(new StaticFileOptions
{
 FileProvider = new PhysicalFileProvider(wwwroot),
});
app.MapGet("/", () =>
{
 var f=Path.Combine(wwwroot,"files.html");
    return Results.File(f,
     GetMimeTypeForFileExtension(f));
});
app.MapGet("/files", ( [FromQuery(Name = "path")] string path) =>
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

app.MapGet("/file", ( [FromQuery(Name = "path")] string path) =>
{
 Console.WriteLine(path);
 if (File.Exists(path))
 {
  return Results.File(path,GetMimeTypeForFileExtension(path),null,null,null,true);
 }

 return Results.NotFound();
});

app.Run("http://192.168.8.189:8080");
