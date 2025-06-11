using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BilardApp.Data.Interfaces;

namespace BilardApp.Logic.Interfaces
{
    public interface IBallService
    {
        void CreateBalls(int count, double width, double height);
        IEnumerable<IBall> GetBalls();
        Task UpdateBallPositionsAsync(double width, double height);
    }
}