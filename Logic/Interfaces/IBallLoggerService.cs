using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BilardApp.Data.Interfaces;

namespace BilardApp.Logic.Interfaces
{
    public interface IBallLoggerService
    {
        Task LogAsync(IEnumerable<IBall> balls, DateTime timestamp);
    }
}
