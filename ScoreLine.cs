using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    public class ScoreLine : IComparable
    {
        private int _Score;
        private string _Name;

        public int Score { get => _Score; set => _Score = value; }
        public string Name { get => _Name; set => _Name = value; }

        public ScoreLine(int score, string name)
        {
            Score = score;
            Name = name;
        }

        public int CompareTo(object obj)
        {
            if (obj is ScoreLine sl)
            {
                return Score.CompareTo(sl.Score);
            }

            return -1;
        }
    }
}
