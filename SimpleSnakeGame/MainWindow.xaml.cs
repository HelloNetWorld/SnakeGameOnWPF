using System.Windows;

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
            Loaded += SnakeGameLoaded;
        }

        private void SnakeGameLoaded(object sender, RoutedEventArgs e)
        {
            var snakeGame = new SnakeGame
            {
                Snake = new Snake
                {
                    Length = 3,
                    StartPosition = StartPosition.Center
                },
                GameField = new GameField
                {
                    SquareSize = 50,
                    Field = playingField
                },
                Difficulty = Difficulty.Hard
            };
            snakeGame.ReadySteadyGo();
        }
    }
}
