using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SimpleSnakeGame
{
    public class GameField
    {
        public GameField()
        {
        }
        /// <summary>
        /// Square size in pixels
        /// </summary>
        public int SquareSize { get; set; }
        public Canvas Field { get; set; }

    }
}
