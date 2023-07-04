using Chat.BLL.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.AzureFunc.TextFileTransform.Helpers
{
    public static class FileTypesValidator
    {
        public static bool IsValidType(string fileName)
        {
            var fileType = Path.GetExtension(fileName)[1..].ToLower();
            return typesList.Contains(fileType);
        }

        public static string GetAvailableTypes()
        {
            return string.Join(", ", typesList);
        }

        private readonly static HashSet<string> typesList = new()
        {
            "doc",
            "docx"
        };
    }
}
