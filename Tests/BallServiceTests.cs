using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BilardApp.Data.Interfaces;
using BilardApp.Data.Models;
using BilardApp.Logic.Services;
using Xunit;
using Moq;

namespace BilardApp.Tests.Logic
{
    public class BallServiceTests
    {
        [Fact]
        public async Task UpdateBallPositionsAsync_ShouldHandleCollisions()
        {
            // Arrange
            var ball1 = new Mock<IBall>();
            var ball2 = new Mock<IBall>();

            // Konfiguracja mocków
            ball1.SetupAllProperties();
            ball2.SetupAllProperties();

            ball1.Object.X = 100;
            ball1.Object.Y = 100;
            ball1.Object.VelocityX = 2;
            ball1.Object.VelocityY = 0;

            ball2.Object.X = 110;
            ball2.Object.Y = 100;
            ball2.Object.VelocityX = -2;
            ball2.Object.VelocityY = 0;

            var service = new BallService();
            service.GetType().GetField("_balls", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(service, new List<IBall> { ball1.Object, ball2.Object });

            // Act
            await service.UpdateBallPositionsAsync(1000, 750);

            // Assert
            Assert.True(ball1.Object.VelocityX < 0);
            Assert.True(ball2.Object.VelocityX > 0);
        }

        [Fact]
        public void CreateBalls_ShouldGenerateCorrectAmount()
        {
            // Arrange
            var service = new BallService();

            // Act
            service.CreateBalls(5, 1000, 750);

            // Assert
            Assert.Equal(5, service.GetBalls().Count());
        }
    }
}