using Microsoft.AspNetCore.StaticFiles;

namespace WebApp;

public static class Shared
{
    static readonly FileExtensionContentTypeProvider Provider = new();

    public static string GetMimeTypeForFileExtension(string filePath)
    {
        const string defaultContentType = "application/octet-stream";

        if (!Provider.TryGetContentType(filePath, out var contentType))
        {
            contentType = defaultContentType;
        }

        return contentType;
    }
}