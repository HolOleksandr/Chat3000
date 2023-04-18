﻿using Chat.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.DAL.Repositories.Interfaces
{
    public interface IMessageRepository : IBaseRepository<Message>
    {
        Message CreateMessageTest(Message message);
    }
}