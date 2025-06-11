using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BilardApp.Logic.Interfaces;
using System.Windows.Input;
using BilardApp.GUI.Models;
using System.Timers;
using System.Windows.Threading;
using System.Windows.Media;

namespace BilardApp.GUI.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly IBallService _ballService;
        private readonly DispatcherTimer _animationTimer;
        private readonly DispatcherTimer _colorChangeTimer;
        private readonly Random _random = new();

        private int _ballCount = 10;
        public int BallCount
        {
            get => _ballCount;
            set
            {
                if (_ballCount != value)
                {
                    _ballCount = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<GuiBall> Balls { get; set; } = new();

        public ICommand GenerateBallsCommand { get; }

        private void ChangeBallColors()
        {
            foreach (var ball in Balls)
            {
                ball.UpdateColor();
            }
        }

        public MainViewModel(IBallService ballService)
        {
            _ballService = ballService;

            GenerateBallsCommand = new RelayCommand(_ => GenerateBallsAsync());

            _animationTimer = new DispatcherTimer();
            _animationTimer.Interval = TimeSpan.FromMilliseconds(8); // ~60 FPS
            _animationTimer.Tick += async (s, e) => await UpdateBallsAsync();
            _animationTimer.Start();

            _colorChangeTimer = new DispatcherTimer();
            _colorChangeTimer.Interval = TimeSpan.FromSeconds(5);
            _colorChangeTimer.Tick += (s, e) => ChangeBallColors();
            _colorChangeTimer.Start();
        }

        private const double LogicalWidth = 1000;
        private const double LogicalHeight = 750;

        private double _canvasWidth = 400;
        private double _canvasHeight = 300;

        public void UpdateCanvasSize(double width, double height)
        {
            _canvasWidth = width;
            _canvasHeight = height;

            foreach (var ball in Balls)
            {
                ball.SetScale(_canvasWidth / LogicalWidth, _canvasHeight / LogicalHeight);
            }
        }

        private async Task GenerateBallsAsync()
        {
            _ballService.CreateBalls(BallCount, LogicalWidth, LogicalHeight);
            Balls.Clear();

            foreach (var ball in _ballService.GetBalls())
            {
                var guiBall = new GuiBall(ball);
                guiBall.SetScale(_canvasWidth / LogicalWidth, _canvasHeight / LogicalHeight);
                Balls.Add(guiBall);
            }
        }

        private async Task UpdateBallsAsync()
        {
            await _ballService.UpdateBallPositionsAsync(LogicalWidth, LogicalHeight);
            foreach (var ball in Balls)
            {
                ball.Refresh();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}