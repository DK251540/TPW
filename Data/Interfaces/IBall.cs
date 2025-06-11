using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilardApp.Data.Interfaces
{
    public interface IBall
    {
        double X { get; set; }
        double Y { get; set; }
        double Radius { get; }
        double Mass { get; set; } 
        double VelocityX { get; set; }
        double VelocityY { get; set; }
        void Move(double maxWidth, double maxHeight);
    }
}
