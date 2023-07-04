using Chat.BLL.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Chat.BLL.Validators
{
    public static class FileTypesValidator
    {
        public static bool IsValidType(string type)
        {
            if (string.IsNullOrEmpty(type))
                return false;
            return typesList.Contains(type);
        }

        public static string GetAvailableTypes()
        {
            return string.Join(", ", typesList);
        }

        public static void IsValidFileType(IFormFile avatarFile)
        {
            var fileType = Path.GetExtension(avatarFile.FileName)[1..].ToLower();
            var isValidType = IsValidType(fileType);
            if (!isValidType)
                throw new ChatException($"File extention must be one of: {GetAvailableTypes()}");
        }

        private readonly static HashSet<string> typesList = new()
        {
            "jpg",
            "jpeg",
            "png",
            "jfif"
        };
    }
}
