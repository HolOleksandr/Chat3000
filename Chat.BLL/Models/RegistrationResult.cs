using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.BLL.Models
{
    public class RegistrationResult
    {
        public bool Success { get; set; }
        public IEnumerable<string>? Errors { get; set; }

    }
}
