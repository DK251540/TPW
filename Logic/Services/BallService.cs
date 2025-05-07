using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BilardApp.Data.Interfaces;
using BilardApp.Data.Models;
using BilardApp.Logic.Interfaces;

namespace BilardApp.Logic.Services
{
    public class BallService : IBallService
    {
        private readonly List<IBall> _balls = new();
        private readonly object _lock = new();

        public void CreateBalls(int count, double width, double height)
        {
            var rand = new Random();
            _balls.Clear();

            for (int i = 0; i < count; i++)
            {
                var ball = new Ball
                {
                    X = rand.NextDouble() * (width - 20) + 10,
                    Y = rand.NextDouble() * (height - 20) + 10,
                    Mass = rand.NextDouble() * 2 + 0.5 // Teraz to zadziała
                };
                _balls.Add(ball);
            }
        }
       
        public IEnumerable<IBall> GetBalls()
        {
            lock (_lock)
            {
                return _balls.ToList();
            }
        }

        public async Task UpdateBallPositionsAsync(double width, double height)
        {
            await Task.Run(() =>
            {
                lock (_lock)
                {
                    // Aktualizacja pozycji
                    foreach (var ball in _balls)
                    {
                        ball.Move(width, height);
                    }

                    // Detekcja kolizji
                    for (int i = 0; i < _balls.Count; i++)
                    {
                        for (int j = i + 1; j < _balls.Count; j++)
                        {
                            if (AreBallsColliding(_balls[i], _balls[j]))
                            {
                                HandleCollision(_balls[i], _balls[j]);
                            }
                        }
                    }
                }
            });
        }

        private bool AreBallsColliding(IBall ball1, IBall ball2)
        {
            double dx = ball1.X - ball2.X;
            double dy = ball1.Y - ball2.Y;
            double distance = Math.Sqrt(dx * dx + dy * dy);
            return distance < (ball1.Radius + ball2.Radius) * 0.9; 
        }

        private void HandleCollision(IBall ball1, IBall ball2)
        {
            // Oblicz wektor normalny kolizji
            double dx = ball2.X - ball1.X;
            double dy = ball2.Y - ball1.Y;
            double distance = Math.Sqrt(dx * dx + dy * dy);

            // Normalizacja wektora
            double nx = dx / distance;
            double ny = dy / distance;

            // Oblicz względną prędkość
            double relativeVelocityX = ball2.VelocityX - ball1.VelocityX;
            double relativeVelocityY = ball2.VelocityY - ball1.VelocityY;

            // Oblicz prędkość wzdłuż normalnej
            double velocityAlongNormal = relativeVelocityX * nx + relativeVelocityY * ny;

            // Jeśli kule oddalają się od siebie, nie ma kolizji
            if (velocityAlongNormal > 0) return;

            // Współczynnik restytucji (1 dla zderzenia doskonale sprężystego)
            double restitution = 1.0;

            // Oblicz impuls
            double j = -(1 + restitution) * velocityAlongNormal;
            j /= (1 / ball1.Mass) + (1 / ball2.Mass);

            // Zastosuj impuls
            double impulseX = j * nx;
            double impulseY = j * ny;

            ball1.VelocityX -= impulseX / ball1.Mass;
            ball1.VelocityY -= impulseY / ball1.Mass;
            ball2.VelocityX += impulseX / ball2.Mass;
            ball2.VelocityY += impulseY / ball2.Mass;

            // Rozdziel kule, aby uniknąć "zlepienia"
            double overlap = (ball1.Radius + ball2.Radius) - distance;
            if (overlap > 0)
            {
                double moveX = overlap * nx * 0.5;
                double moveY = overlap * ny * 0.5;

                ball1.X -= moveX;
                ball1.Y -= moveY;
                ball2.X += moveX;
                ball2.Y += moveY;
            }
        }
    }
}