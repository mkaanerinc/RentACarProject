using Core.Utilities.Helpers.FileHelper.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Helpers.FileHelper.Concrete
{
    public class ImageFileManager : IFileHelper
    {
        public IResult Upload(IFormFile file, string root)
        {
            IResult fileExists = CheckIfFileExists(file);
            if (!fileExists.Success)
                return fileExists;

            string fileExtension = Path.GetExtension(file.FileName);
            IResult fileExtensionValid = CheckIfFileExtensionValid(fileExtension);
            if (!fileExtensionValid.Success)
                return fileExtensionValid;

            string guild = Guid.NewGuid().ToString();
            string fileName = guild + fileExtension;

            CheckIfDirectoryExists(root);
            CreateImageFile(file, root + fileName);

            return new SuccessResult(fileName);
        }

        public IResult Delete(string filePath)
        {
            DeleteOldImageFile(filePath);

            return new SuccessResult();
        }

        public IResult Update(IFormFile file, string filePath, string root)
        {
            Delete(filePath);

            return Upload(file, root);
        }

        private static IResult CheckIfFileExists(IFormFile file)
        {
            if(file is null || file.Length == 0)
            {
                return new ErrorResult("File is not found.");
            }

            return new SuccessResult();
        }

        private static IResult CheckIfFileExtensionValid(string fileExtension)
        {
            if(fileExtension != ".jpeg" && fileExtension != ".jpg" && fileExtension != ".png")
            {
                return new ErrorResult("File extension is invalid.");
            }

            return new SuccessResult();
        }

        private static void CheckIfDirectoryExists(string root)
        {
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }
        }

        private static void CreateImageFile(IFormFile file, string directory)
        {
            using (FileStream fileStream = File.Create(directory))
            {
                file.CopyTo(fileStream);
                fileStream.Flush();
            }
        }

        private static void DeleteOldImageFile(string filePath)
        {
            if(Directory.Exists(filePath))
                Directory.Delete(filePath);
        }
    }
}
