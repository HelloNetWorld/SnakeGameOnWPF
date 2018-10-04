using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using SnakeGameLibrary;

namespace SimpleSnakeGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += SnakeGameLoader;
            KeyDown += new KeyEventHandler(CloseOnEsc);
        }

        private void SnakeGameLoader(object sender, RoutedEventArgs e)
        {
            var snakeGame = new SnakeGame
            {
                Snake = new Snake(
                    length: 3,
                    startPosition: StartPosition.Center,
                    bodyColor: Brushes.Red,
                    headColor: Brushes.Black),
                GameField = new GameField(
                    pixelSize: 50,
                    playingField: playingField,
                    pixelType: PixelType.Square),
                Difficulty = Difficulty.Hard,
                FoodColor = Brushes.GreenYellow,
                AmountOfFood = 6
            };
            snakeGame.Start();
        }

        private void CloseOnEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) Close();
        }
    }
}
