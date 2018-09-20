using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSnakeGame
{
    public class SnakeGame
    {
        public Snake Snake { get; set; }
        public GameField GameField { get; set; }
        public Difficulty Difficulty { get; set; }
        public void ReadySteadyGo()
        {

        }
    }
    public enum Difficulty
    {
        Easy,
        Normal,
        Hard,
        VeryHard,
        Impossible
    }
}
