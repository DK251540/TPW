using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BilardApp.Data.Interfaces;

namespace BilardApp.Data.Models
{
    public class Ball : IBall
    {
        private static readonly Random _rand = new();

        public double X { get; set; }
        public double Y { get; set; }
        public double Radius { get; } = 10;
        public double Mass { get; set; } // Zmieniamy na get/set
        public double VelocityX { get; set; }
        public double VelocityY { get; set; }

        public Ball()
        {
            VelocityX = (_rand.NextDouble() * 4 - 2)*50;
            VelocityY = (_rand.NextDouble() * 4 - 2)*50;
            Mass = _rand.NextDouble() * 2 + 0.5; // Inicjalizacja masy
        }

        public void Move(double maxWidth, double maxHeight, double deltaTime)
        {
            X += VelocityX * deltaTime;
            Y += VelocityY * deltaTime;

            if (X - Radius < 0 || X + Radius > maxWidth)
                VelocityX *= -1;

            if (Y - Radius < 0 || Y + Radius > maxHeight)
                VelocityY *= -1;

            X = Math.Max(Radius, Math.Min(maxWidth - Radius, X));
            Y = Math.Max(Radius, Math.Min(maxHeight - Radius, Y));
        }

    }
}