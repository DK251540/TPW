//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2024, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//_____________________________________________________________________________________________________________________________________

namespace TP.ConcurrentProgramming.Data
{

    internal class Ball : IBall
    {
        private readonly double BallDiameter; // <== Nowe pole

        internal Ball(Vector initialPosition, Vector initialVelocity, double ballDiameters)
        {
            Position = initialPosition;
            Velocity = initialVelocity;
            BallDiameter = ballDiameters;
        }
        #region ctor

        internal Ball(Vector initialPosition, Vector initialVelocity)
        {
            Position = initialPosition;
            Velocity = initialVelocity;
        }

        #endregion ctor

        #region IBall

        public event EventHandler<IVector>? NewPositionNotification;

        public IVector Velocity { get; set; }

        #endregion IBall

        #region private

        public Vector Position;

        private void RaiseNewPositionChangeNotification()
        {
            NewPositionNotification?.Invoke(this, Position);
        }

        internal void Move(Vector delta)
        {
            double newX = Position.x + delta.x;
            double newY = Position.y + delta.y;
            double radius = BallDiameter / 2.0;

            // Odbicia od ścian z uwzględnieniem promienia
            if (newX - radius < 0)
                newX = radius;
            else if (newX + radius > 400)
                newX = 400 - radius;

            if (newY - radius < 0)
                newY = radius;
            else if (newY + radius > 400)
                newY = 400 - radius;

            Position = new Vector(newX, newY);
            RaiseNewPositionChangeNotification();
        }

        #endregion private
    }


}