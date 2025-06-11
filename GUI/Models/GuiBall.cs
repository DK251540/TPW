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
        private Brush _ballColor;

        public GuiBall(IBall ball)
        {
            _ball = ball;
            UpdateColor();
        }

        public double ScaledX => _ball.X * _scaleX;
        public double ScaledY => _ball.Y * _scaleY;
        public double ScaledRadius => _ball.Radius * ((_scaleX + _scaleY) / 2) * VisualScaleFactor;
        public Brush BallColor
        {
            get => _ballColor;
            private set
            {
                _ballColor = value;
                OnPropertyChanged();
            }
        }

        private static int _colorSchemeIndex = 0;
        private static readonly Brush[][] _colorSchemes =
        {
    // Schemat 0 - oryginalny
    new[] { Brushes.LightBlue, Brushes.Blue, Brushes.Red, Brushes.DarkRed },
    // Schemat 1 - pastelowy
    new[] { Brushes.LightGreen, Brushes.LightSkyBlue, Brushes.LightPink, Brushes.LightSalmon },
    // Schemat 2 - ciemny
    new[] { Brushes.DarkGreen, Brushes.DarkBlue, Brushes.DarkViolet, Brushes.DarkRed },
    // Schemat 3 - neonowy
    new[] { Brushes.Lime, Brushes.Cyan, Brushes.Magenta, Brushes.Yellow }
};

        public void UpdateColor()
        {
            var scheme = _colorSchemes[_colorSchemeIndex];
            BallColor = _ball.Mass switch
            {
                < 1 => scheme[0],
                < 1.5 => scheme[1],
                < 2 => scheme[2],
                _ => scheme[3]
            };

            // Przełącz na następny schemat
            _colorSchemeIndex = (_colorSchemeIndex + 1) % _colorSchemes.Length;
        }

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
