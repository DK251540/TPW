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
            VelocityX = _rand.NextDouble() * 4 - 2;
            VelocityY = _rand.NextDouble() * 4 - 2;
            Mass = _rand.NextDouble() * 2 + 0.5; // Inicjalizacja masy
        }

        public void Move(double maxWidth, double maxHeight)
        {
            X += VelocityX;
            Y += VelocityY;

            // Odbicia od ścian
            if (X - Radius < 0 || X + Radius > maxWidth)
                VelocityX *= -1;

            if (Y - Radius < 0 || Y + Radius > maxHeight)
                VelocityY *= -1;

            // Korekta pozycji
            X = Math.Max(Radius, Math.Min(maxWidth - Radius, X));
            Y = Math.Max(Radius, Math.Min(maxHeight - Radius, Y));
        }
    }
}