using System;
using System.Linq;
using System.Windows.Media;

namespace SnakeGameLibrary
{
    public class SnakeGame
    {
        #region Properties

        public Snake Snake { get; set; }

        public GameField GameField { get; set; }

        public Difficulty Difficulty { get; set; }

        public int AmountOfFood { get; set; }

        public SolidColorBrush FoodColor { get; set; }

        #endregion

        #region Public methods

        public void Start()
        {
            if (GameField == null)
            {
                throw new ArgumentNullException(nameof(GameField));
            }

            if (Snake == null)
            {
                throw new ArgumentNullException(nameof(Snake));
            }

            if (Difficulty < Difficulty.Impossible || Difficulty > Difficulty.Easy)
            {
                throw new ArgumentException($"{nameof(Difficulty)} illegal enum value");
            }

            Snake.InitializeSnake(GameField, Difficulty);
            Snake.OnLackOfFood += InitializeFood;
            Snake.OnSnakeCrash += GameOver;
            Snake.StartMoving();
        }

        #endregion

        #region Private methods

        private void InitializeFood(object sender, EventArgs e)
        {
            if (FoodColor == null)
            {
                throw new ArgumentNullException(nameof(FoodColor));
            }
            if (AmountOfFood <= 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(AmountOfFood)} must be positive");
            }
            var emptyPixels = GameField.GetPixels().Where(s => s.GetFillColor == null).ToArray();
            var random = new Random();
            int foodCount = 0;
            do
            {
                var randomIndex = random.Next(0, emptyPixels.Length);
                emptyPixels[randomIndex].Fill(FoodColor);
                emptyPixels[randomIndex].IsFood = true;
            }
            while (++foodCount < AmountOfFood);
        }

        private void GameOver(object sender, EventArgs e)
        {
            GameField.Clear();
            GameField.ResultBoard(Snake.MovesCount, Snake.ScoreCount);
            Snake.OnLackOfFood -= InitializeFood;
        }

        #endregion
    }

    public enum Difficulty
    {
        Easy = 300,
        Normal = 250,
        Hard = 200,
        VeryHard = 180,
        Impossible = 140
    }
}
