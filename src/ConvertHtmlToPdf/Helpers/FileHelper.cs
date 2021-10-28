using System;
using System.IO;
using ConvertHtmlToPdf.Extensions;
using Microsoft.AspNetCore.Http;

namespace ConvertHtmlToPdf.Helpers
{
    public static class FileHelper
    {
        public static string GetFileAsString(string filePath, string fileNameWithExtension)
        {
            filePath = Path.Combine(filePath, fileNameWithExtension);

            if (!File.Exists(filePath))
                throw new ArgumentException($"Invalid argument: {nameof(filePath)} and/or {nameof(fileNameWithExtension)}, file doesn't exist in this location: {filePath}");

            using StreamReader streamReader = new(filePath);
            var fileContent = streamReader.ReadToEnd();

            return fileContent;
        }

        public static IFormFile? GetFileAsIFormFile(string filePath, string fileNameWithExtension)
        {
            var fullFilePath = Path.Combine(filePath, fileNameWithExtension);

            return GetFileAsIFormFile(fullFilePath);
        }

        public static IFormFile? GetFileAsIFormFile(string fullFilePath)
        {
            if (!File.Exists(fullFilePath)) return null;

            var fileContentAsByteArray = GetFileAsByteArray(fullFilePath);
            var fileName = Path.GetFileName(fullFilePath);

            return fileContentAsByteArray.ToIFormFile(fileName);
        }

        public static byte[] GetFileAsByteArray(string fullFilePath)
        {
            byte[] fileContent = Array.Empty<byte>();

            if (!File.Exists(fullFilePath)) return fileContent;

            using FileStream fileStream = File.OpenRead(fullFilePath);
            using BinaryReader binaryReader = new(fileStream);

            return binaryReader.ReadBytes((int)fileStream.Length);
        }
    }
}
