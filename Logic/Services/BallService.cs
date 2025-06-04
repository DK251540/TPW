using BilardApp.Data.Interfaces;
using BilardApp.Data.Models;
using BilardApp.Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BilardApp.Logic.Services
{
    public class BallService : IBallService
    {
        private readonly List<IBall> _balls = new();
        private readonly object _lock = new();
        private readonly IBallLoggerService? _logger;

        public BallService(IBallLoggerService? logger = null)
        {
            _logger = logger;
        }

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
                    Mass = rand.NextDouble() * 2 + 0.5
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

        public async Task UpdateBallPositionsAsync(double width, double height, double deltaTime)
        {
            await Task.Run(() =>
            {
                lock (_lock)
                {
                    foreach (var ball in _balls)
                    {
                        if (ball is Ball concreteBall)
                        {
                            concreteBall.Move(width, height, deltaTime);
                        }
                    }

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

                    _ = _logger?.LogAsync(_balls.ToList(), DateTime.Now);
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
            double dx = ball2.X - ball1.X;
            double dy = ball2.Y - ball1.Y;
            double distance = Math.Sqrt(dx * dx + dy * dy);

            if (distance == 0) return; // zapobiegamy dzieleniu przez 0

            double nx = dx / distance;
            double ny = dy / distance;

            double relativeVelocityX = ball2.VelocityX - ball1.VelocityX;
            double relativeVelocityY = ball2.VelocityY - ball1.VelocityY;

            double velocityAlongNormal = relativeVelocityX * nx + relativeVelocityY * ny;

            if (velocityAlongNormal > 0) return;

            double restitution = 1.0;
            double j = -(1 + restitution) * velocityAlongNormal;
            j /= (1 / ball1.Mass) + (1 / ball2.Mass);

            double impulseX = j * nx;
            double impulseY = j * ny;

            ball1.VelocityX -= impulseX / ball1.Mass;
            ball1.VelocityY -= impulseY / ball1.Mass;
            ball2.VelocityX += impulseX / ball2.Mass;
            ball2.VelocityY += impulseY / ball2.Mass;

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
