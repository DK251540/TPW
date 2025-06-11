using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BilardApp.GUI.ViewModels;
using BilardApp.Logic.Services;

namespace BilardApp.GUI.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel(new BallService());
        }

        private void GameCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (DataContext is MainViewModel vm)
            {
                vm.UpdateCanvasSize(GameCanvas.ActualWidth, GameCanvas.ActualHeight);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            if (DataContext is IDisposable disposable)
            {
                disposable.Dispose();
            }
            base.OnClosed(e);
        }
    }




}