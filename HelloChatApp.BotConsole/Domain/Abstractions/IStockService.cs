﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloChatApp.BotConsole.Domain.Abstractions
{
    public interface IStockService
    {
        Task<double> GetStockPrice(string stockCode);
    }
}
