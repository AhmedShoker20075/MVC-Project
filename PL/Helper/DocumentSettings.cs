using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace PL.Helper
{
    public class DocumentSettings
    {
        public static string UploadFile(IFormFile file , string folderName)
        {
            
             //1.Get Location Folder Path

             //static
             //string folderPath = "D:\\Career\\Route\\02C#\\MVC\\Assignment05\\Assignment21\\PL\\wwwroot\\Files\\" + folderName;

             //dynamic
             //string folderPath = Directory.GetCurrentDirectory() + "wwwroot\\Files\\" +folderName;

             string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files\\", folderName);

             //2.Get File Name And Make It Unique

             string fileName = $"{Guid.NewGuid()}{file.FileName}";

             //3.File Path => FolderPath + FileName

             string filePath = Path.Combine(folderPath, fileName);

             //4.Save File As Stream : Data Per Time

             using var fileStream = new FileStream(filePath, FileMode.Create);
             file.CopyTo(fileStream);

             return fileName;
           

        }

        public static void DeleteFile (string fileName , string folderName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files\\", folderName, fileName);

            if (File.Exists(filePath))
                File.Delete(filePath);

        }
    }
}
