using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using BilardApp.Data.Interfaces;

namespace BilardApp.GUI.Models
{
    public class GuiBall : INotifyPropertyChanged
    {
        private readonly IBall _ball;
        private double _scaleX = 1;
        private double _scaleY = 1;
        private const double VisualScaleFactor = 1.5;

        public GuiBall(IBall ball)
        {
            _ball = ball;
            // Ustaw kolor na podstawie masy
            BallColor = ball.Mass switch
            {
                < 1 => Brushes.LightBlue,
                < 1.5 => Brushes.Blue,
                < 2 => Brushes.Red,
                _ => Brushes.DarkRed
            };
        }

        public double ScaledX => _ball.X * _scaleX;
        public double ScaledY => _ball.Y * _scaleY;
        public double ScaledRadius => _ball.Radius * ((_scaleX + _scaleY) / 2) * VisualScaleFactor;
        public Brush BallColor { get; }

        public void SetScale(double width, double height)
        {
            _scaleX = width;
            _scaleY = height;
            OnPropertyChanged(nameof(ScaledX));
            OnPropertyChanged(nameof(ScaledY));
            OnPropertyChanged(nameof(ScaledRadius));
        }

        public void Refresh()
        {
            OnPropertyChanged(nameof(ScaledX));
            OnPropertyChanged(nameof(ScaledY));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
