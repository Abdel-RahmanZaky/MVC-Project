using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace Project.PL.Helpers
{
    public class DocumentSettings
    {
        public static string UpLoadFile(IFormFile file, string folderName)
        {
            // 1. Get located Folder Path
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", folderName);

            // 2. Get File Name and Make It UNIQUE:
            string fileName = $"{Guid.NewGuid()}{file.FileName}";

            // 3. Get File Path :
            string filePath = Path.Combine(folderPath, fileName);

            // 4. Save File as Streems [Data Per Time]
            using var folderStreem = new FileStream(filePath, FileMode.Create);
            file.CopyTo(folderStreem);

            return fileName;

        }

        public static void DeleteFile(string folderName, string fileName) 
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files",folderName, fileName);

            if(File.Exists(filePath))
                File.Delete(filePath);
        }
    }
}
