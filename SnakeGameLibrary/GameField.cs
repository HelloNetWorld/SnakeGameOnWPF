using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SnakeGameLibrary
{
    public class GameField
    {
        #region Private members

        private readonly Pixel[,] _pixels;

        #endregion

        #region Public methods

        /// <summary>
        /// Initializes a new instance of GameField
        /// </summary>
        /// <param name="pixelSize">Size of squares in pixels (must be between 10 and 100)</param>
        /// <param name="playingField">Playing field</param>
        /// <param name="pixelType">Pixels type</param>
        public GameField(int pixelSize, Canvas playingField, PixelType pixelType)
        {
            if (pixelType < PixelType.Circle || pixelType > PixelType.Square)
            {
                throw new ArgumentException($"{nameof(pixelType)} illegal enum value");
            }
            Pixel.PixelType = pixelType;

            if (pixelSize < 10 || pixelSize > 100)
            {
                throw new ArgumentOutOfRangeException($"{nameof(pixelSize)} must be between '5' and '100'");
            }
            Pixel.Canvas = playingField ?? throw new ArgumentNullException(nameof(playingField));
            Pixel.Size = pixelSize;

            Rows = (int)Math.Floor(playingField.ActualHeight / pixelSize);
            Columns = (int)Math.Floor(playingField.ActualWidth / pixelSize);

            _pixels = new Pixel[Rows, Columns];
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    _pixels[i, j] = new Pixel(i, j);
                }
            }
        }

        public void ResultBoard(int movesCount, double scoreCount)
        {
            var grid = new Grid
            {
                Height = Pixel.Canvas.ActualHeight,
                Width = Pixel.Canvas.ActualWidth
            };
            var label = new Label
            {
                Content = $"GAME OVER\nMoves: {movesCount}, Score: {scoreCount: 0}",
                Foreground = Brushes.GreenYellow,
                FontSize = 50,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            grid.Children.Add(label);

            Pixel.Canvas.Children.Add(grid);
        }

        public IEnumerable<Pixel> GetPixels()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    yield return _pixels[i, j];
                }
            }
        }

        public void Clear() => Pixel.Canvas.Children.Clear();

        public (int i, int j) GetStartingPoint(StartPosition startPosition)
        {
            switch (startPosition)
            {
                case StartPosition.Center:
                    return (Rows / 2, Columns / 2);
                case StartPosition.LeftDownCorner:
                    return (Rows - 2, 1);
                case StartPosition.LeftUpCorner:
                    return (1, 1);
                case StartPosition.RightDownCorner:
                    return (Rows - 2, Columns - 2);
                case StartPosition.RightUpCorner:
                    return (1, Columns - 2);
                default: return (Rows / 2, Columns / 2);
            }
        }

        public bool IsLackOfFood => !GetPixels().Any(p => p.IsFood);

        #endregion

        #region Indexer

        public Pixel this[int i, int j]
        {
            get
            {
                if (i < 0 || i >= Rows)
                {
                    throw new ArgumentOutOfRangeException($"{nameof(i)} must be between '0' and '{Rows}'");
                }
                if (j < 0 || j >= Columns)
                {
                    throw new ArgumentOutOfRangeException($"{nameof(j)} must be between '0' and '{Columns}'");
                }
                return _pixels[i, j];
            }
        }

        #endregion

        #region Properties

        public int Rows { get; }
        public int Columns { get; }

        #endregion
    }

    public class Pixel
    {
        #region Private members

        private readonly Shape _pixel;

        #endregion

        #region Ctor

        public Pixel(int i, int j)
        {
            I = i;
            J = j;
            int yCoordinate = i * Size;
            int xCoordinate = j * Size;

            if (PixelType == PixelType.Circle)
            {
                _pixel = new Ellipse();
            }
            else
            {
                _pixel = new Rectangle();
            }

            IsFood = false;
            _pixel.Height = Size;
            _pixel.Width = Size;

            Canvas.Children.Add(_pixel);
            Canvas.SetLeft(_pixel, xCoordinate);
            Canvas.SetTop(_pixel, yCoordinate);
        }

        #endregion

        #region Public methods

        public void Fill(SolidColorBrush brush) => _pixel.Fill = brush;

        public void Unfill() => _pixel.Fill = null;

        #endregion

        #region Properties

        public static Canvas Canvas { get; set; }

        public static PixelType PixelType { get; set; }

        public static int Size { get; set; }

        public Brush GetFillColor => _pixel.Fill;

        public int I { get; set; }

        public int J { get; set; }

        public bool IsFood { get; set; }

        #endregion

    }

    public enum PixelType
    {
        Circle,
        Square
    }
}
