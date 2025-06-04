using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BilardApp.Data.Interfaces;
using BilardApp.Data.Models;
using BilardApp.Logic.Services;
using Xunit;

namespace BilardApp.Tests.Data
{
    public class BallTests
    {

        [Fact]
        public void Move_ShouldUpdatePosition()
        {
            // Arrange
            var ball = new Ball { X = 100, Y = 100, VelocityX = 5, VelocityY = 5 };

            // Act
            ball.Move(1000, 750, 1);

            // Assert
            Assert.Equal(105, ball.X);
            Assert.Equal(105, ball.Y);
        }

        [Fact]
        public void Move_ShouldBounceFromWalls()
        {
            // Arrange
            var ball = new Ball { X = 10, Y = 10, VelocityX = -5, VelocityY = -5 };

            // Act
            ball.Move(1000, 750, 1);

            // Assert
            Assert.True(ball.VelocityX > 0);
            Assert.True(ball.VelocityY > 0);
        }

        [Fact]
        public async Task LogAsync_WritesCorrectData()
        {
            var logger = new BallLoggerService();
            var balls = new List<IBall>
        {
            new Ball { X = 1, Y = 1, VelocityX = 0.1, VelocityY = 0.1, Mass = 1 }
        };

            await logger.LogAsync(balls, DateTime.Now);

            var lines = File.ReadAllLines("ball_log.txt");
            Assert.Contains(",", lines.Last());
        }
    }
}