using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace SnakeGameLibrary
{
    public class Snake
    {
        #region Private members

        private GameField _gameField;
        private readonly DispatcherTimer _dispatcherTimer;

        #endregion

        #region Ctors

        public Snake(int length)
        {
            if (length <= 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(length)} must be positive");
            }
            Body = new List<Pixel>();
            MovingDirection = Direction.Right;
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Tick += Move;
            Length = length;

            MovesCount = 0;
            ScoreCount = 0;
        }

        /// <summary>
        /// Initializes a new instance of Snake
        /// </summary>
        /// <param name="length">Length (must be between 3 and 10)</param>
        /// <param name="startPosition">Start position</param>
        /// <param name="bodyColor">Body Color</param>
        /// <param name="headColor">Head Color</param>
        public Snake(int length, StartPosition startPosition, SolidColorBrush bodyColor,
            SolidColorBrush headColor) : this(length)
        {
            StartPosition = startPosition;
            BodyColor = bodyColor;
            HeadColor = headColor;
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when snake crashes
        /// </summary>
        public event EventHandler OnSnakeCrash;

        /// <summary>
        /// Occurs when there is no food
        /// </summary>
        public event EventHandler OnLackOfFood;

        #endregion

        #region Public methods

        /// <summary>
        /// Initializes a snake on game field
        /// </summary>
        /// <param name="gamefield">Game Field</param>
        /// <param name="difficulty">Difficulty</param>
        public void InitializeSnake(GameField gamefield, Difficulty difficulty)
        {
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, (int)difficulty);
            _gameField = gamefield ?? throw new ArgumentNullException(nameof(gamefield));
            var startingPoint = gamefield.GetStartingPoint(StartPosition);
            Head = gamefield[startingPoint.i, startingPoint.j];
            Head.Fill(HeadColor);

            for (int i = 1; i <= Length; i++)
            {
                var square = gamefield[startingPoint.i, startingPoint.j - i];
                square.Fill(BodyColor);
                Body.Add(square);
            }

            Pixel.Canvas.Focusable = true;
            Keyboard.Focus(Pixel.Canvas);
            Pixel.Canvas.KeyDown += OnKeyDown;
        }

        /// <summary>
        /// Starts moving in MovingDirection
        /// </summary>
        public void StartMoving() => _dispatcherTimer.Start();

        /// <summary>
        /// Stops moving
        /// </summary>
        public void StopMoving() => _dispatcherTimer.Stop();

        #endregion

        #region Properties

        public SolidColorBrush BodyColor { get; set; }

        public SolidColorBrush HeadColor { get; set; }

        public Direction MovingDirection { get; set; }

        public StartPosition StartPosition { get; set; }

        /// <summary>
        /// Length without Head
        /// </summary>
        public int Length { get; private set; }

        public List<Pixel> Body { get; }

        public Pixel Head { get; private set; }

        public int MovesCount { get; set; }

        public double ScoreCount { get; set; }

        #endregion

        #region Private methods

        private void Move(object sender, EventArgs e)
        {
            if (MovingDirection < Direction.Up || MovingDirection > Direction.Left)
            {
                throw new ArgumentException($"{nameof(MovingDirection)} illegal enum value");
            }

            // Speed changes every 100 moves
            if (++MovesCount % 100 == 0)
            {
                SpeedUp();
            }

            if (_gameField.IsLackOfFood)
            {
                OnLackOfFood?.Invoke(this, null);
            }

            Head.Fill(BodyColor);
            Body.Insert(0, Head); // Insert head into body - oh my Gosh

            try
            {
                switch (MovingDirection)
                {
                    //gets a brand new Head ;)
                    case Direction.Down:
                        Head = _gameField[Head.I + 1, Head.J];
                        break;
                    case Direction.Up:
                        Head = _gameField[Head.I - 1, Head.J];
                        break;
                    case Direction.Left:
                        Head = _gameField[Head.I, Head.J - 1];
                        break;
                    case Direction.Right:
                        Head = _gameField[Head.I, Head.J + 1];
                        break;
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                StopMoving();
                _dispatcherTimer.Tick -= Move;
                OnSnakeCrash?.Invoke(this, null);
            }

            Head.Fill(HeadColor);

            if (Head.IsFood)
            {
                Head.IsFood = false;
                ScoreCount = Math.Ceiling(10 + (0.5 * MovesCount));
                Length++;
            }
            else
            {
                Body.Last().Unfill();
                Body.RemoveAt(Length);
            }

            if (IsBodyRammedByHead())
            {
                StopMoving();
                _dispatcherTimer.Tick -= Move;
                OnSnakeCrash?.Invoke(this, null);
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs eventArgs)
        {
            switch (eventArgs.Key)
            {
                case Key.W:
                    if (MovingDirection != Direction.Down)
                    MovingDirection = Direction.Up;
                    break;
                case Key.S:
                    if (MovingDirection != Direction.Up)
                    MovingDirection = Direction.Down;
                    break;
                case Key.A:
                    if (MovingDirection != Direction.Right)
                    MovingDirection = Direction.Left;
                    break;
                case Key.D:
                    if (MovingDirection != Direction.Left)
                    MovingDirection = Direction.Right;
                    break;
                case Key.P:
                    if (_dispatcherTimer.IsEnabled)
                    {
                        StopMoving();
                    }
                    else
                    {
                        StartMoving();
                    }
                    break;
            }
        }

        /// <summary>
        /// Indicating whether Body was rammed by Head ;)
        /// </summary>
        /// <returns></returns>
        private bool IsBodyRammedByHead() => Body.Contains(Head);

        /// <summary>
        /// Accelerates snake movement
        /// </summary>
        private void SpeedUp()
        {
            int currentInterval = _dispatcherTimer.Interval.Milliseconds;
            int subtractor = currentInterval / 3;
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, currentInterval - subtractor);
        }

        #endregion
    }

    public enum Direction
    {
        Up,
        Down,
        Right,
        Left
    }

    public enum StartPosition
    {
        Center,
        /// <summary>
        /// Not supported
        /// </summary>
        LeftUpCorner,
        /// <summary>
        /// Not supported
        /// </summary>
        LeftDownCorner,
        /// <summary>
        /// Not supported
        /// </summary>
        RightUpCorner,
        /// <summary>
        /// Not supported
        /// </summary>
        RightDownCorner
    }
}
