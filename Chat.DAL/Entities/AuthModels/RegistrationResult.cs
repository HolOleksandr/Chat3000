using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.DAL.Entities.AuthModels
{
    public class RegistrationResult
    {
        public bool Success { get; set; }
        public IEnumerable<string>? Errors { get; set; }

    }
}
