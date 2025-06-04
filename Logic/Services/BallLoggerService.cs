using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BilardApp.Data.Interfaces;
using BilardApp.Logic.Interfaces;

namespace BilardApp.Logic.Services
{
    public class BallLoggerService : IBallLoggerService
    {
        private readonly string _logFilePath = "ball_log.txt";
        private readonly SemaphoreSlim _semaphore = new(1, 1);

        public async Task LogAsync(IEnumerable<IBall> balls, DateTime timestamp)
        {
            var sb = new StringBuilder();
            foreach (var ball in balls)
            {
                sb.AppendLine($"{timestamp:HH:mm:ss.fff},{ball.X:F2},{ball.Y:F2},{ball.VelocityX:F2},{ball.VelocityY:F2},{ball.Mass:F2}");
            }

            try
            {
                await _semaphore.WaitAsync();
                await File.AppendAllTextAsync(_logFilePath, sb.ToString());
            }
            catch (IOException)
            {
                // obsługa braku przepustowości: pomiń cykl
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
