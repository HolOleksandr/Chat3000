using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Message : IEntity
    {
        public Guid Id { get; set; }
    }
}
