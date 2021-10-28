using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;

namespace ConvertHtmlToPdf.Extensions
{
    //TODO: remove hardcoded strings
    public static class ByteExtensionMethods
    {
        public static IFormFile ToIFormFile(this byte[] value, string fileNameWithExtension)
        {
            if (string.IsNullOrEmpty(Path.GetExtension(fileNameWithExtension)))
            {
                throw new ArgumentException($"Invalid argument: {nameof(fileNameWithExtension)}, file name must have and extenstion");
            }

            new FileExtensionContentTypeProvider().TryGetContentType(fileNameWithExtension, out string contentType);

            return ToIFormFile(value, fileNameWithExtension, fileNameWithExtension, contentType);
        }

        public static IFormFile ToIFormFile(this byte[] value, string name = "", string fileName = "", string contentType = "")
        {
            if (value is null || value.Length == 0)
            {
                throw new ArgumentException($"Invalid argument: {nameof(value)}");
            }

            if (!string.IsNullOrEmpty(fileName) && string.IsNullOrEmpty(contentType))
            {
                new FileExtensionContentTypeProvider().TryGetContentType(fileName, out contentType);
            }

            if (string.IsNullOrEmpty(contentType))
            {
                contentType = "unknown/unknown";
            }

            if (string.IsNullOrEmpty(name))
            {
                name = Guid.NewGuid().ToString();
            }

            if (string.IsNullOrEmpty(fileName))
            {
                fileName = Guid.NewGuid().ToString();
            }

            var stream = new MemoryStream(value);
            var file = new FormFile(stream, 0, value.Length, name, fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };

            return file;
        }
    }
}
