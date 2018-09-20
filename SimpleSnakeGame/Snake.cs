using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSnakeGame
{
    public class Snake
    {
        public int Length { get; set; }
        public StartPosition StartPosition { get; set; }
    }
    public enum StartPosition
    {
        Center,
        LeftUpCorner,
        LeftDownCorner,
        RightUpCorner,
        RightDownCorner
    }
}
