using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BilardApp.Data.Models;
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
            ball.Move(1000, 750);

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
            ball.Move(1000, 750);

            // Assert
            Assert.True(ball.VelocityX > 0);
            Assert.True(ball.VelocityY > 0);
        }
    }
}