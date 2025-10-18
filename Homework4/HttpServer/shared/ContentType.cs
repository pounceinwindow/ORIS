using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServer.Shared
{
    public static class ContentType
    {
        private static readonly Dictionary<string, string> FileTypes = new()
        {
            { ".html", "text/html" },
            { ".css", "text/css" },
            { ".js", "application/javascript" },
            { ".png", "image/png" },
            { ".jpeg", "image/jpeg" },
            { ".jpg", "image/jpeg" },
            { ".webp", "image/webp" },
            {".svg", "image/svg+xml" },
        };
        public static string GetContentType(string path)
        {
            
            string extension = Path.GetExtension(path).ToLower();

            return   FileTypes[extension];
        }
    }
}