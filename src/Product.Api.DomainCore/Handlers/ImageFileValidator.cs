using System;
using System.Collections.Generic;
using Product.Api.DomainCore.Exceptions;

namespace Product.Api.DomainCore.Handlers
{
    public class ImageFileValidator
    {
        public const string INVALID_IMAGE_EXTENSION = "Invalid image extension. Available extensions: jpg, jpeg, png";
        public const string INVALID_IMAGE_FILE_NAME = "Invalid image file name.";
        public const string NO_IMAGE_FILE_NAME = "Image file name should be provided.";
        public const string MAX_FILE_NAME_LENGH_REACHED = "Image file name should't be longer than 200 chars";
        public const string FILE_CONTENT_IVALID = "Image content is not valid base64string.";


        private static readonly List<string> WhiteListImageExtensions = new List<string>()
        {
            "jpg",
            "png",
            "jpeg"
        };

        public static void ValidateImageExtension(List<Fault> faults, string imageFileName)
        {
            if (string.IsNullOrEmpty(imageFileName)) return;

            string[] splitedParts = imageFileName.Split('.');
            if (splitedParts.Length <= 1)
            {
                faults.Add(new Fault() {Reason = "InvalidImageFile", Message = INVALID_IMAGE_FILE_NAME});
                return;
            }
            string imageExtension = splitedParts[splitedParts.Length - 1].ToLower();

            if (!WhiteListImageExtensions.Contains(imageExtension))
            {
                faults.Add(new Fault() {Reason = "InvalidImageExtension", Message = INVALID_IMAGE_EXTENSION});
            }
        }

        public static void ValidateFileContent(List<Fault> faults, string fileContent)
        {
            if (string.IsNullOrEmpty(fileContent)) return;

            try
            {
                Convert.FromBase64String(fileContent);
            }
            catch (Exception)
            {
                faults.Add(new Fault() { Reason = nameof(fileContent), Message = FILE_CONTENT_IVALID });
            }
        }


        public static void ValidateImageFileTitle(List<Fault> faults, string commandFileTitle, string commandFileContent)
        {
            if (string.IsNullOrEmpty(commandFileTitle) && string.IsNullOrEmpty(commandFileContent)) return;

            if (string.IsNullOrEmpty(commandFileTitle))
            {
                faults.Add(new Fault() { Reason = "NoFileName", Message = NO_IMAGE_FILE_NAME });
                return;
            }

            if (commandFileTitle.Length > 200)
            {
                faults.Add(new Fault() { Reason = "TooLongFileName", Message = MAX_FILE_NAME_LENGH_REACHED });
            }
        }
    }
}
;