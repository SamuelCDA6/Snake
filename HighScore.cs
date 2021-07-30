using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Snake
{
    public class HighScore
    {
        private string _Path = @"HighScoreSnake.txt";
        private List<ScoreLine> _Scores = new();

        public string Path { get => _Path; private set => _Path = value; }
        public List<ScoreLine> Scores { get => _Scores; private set => _Scores = value; }

        public HighScore(string path)
        {
            Path = path;
            MakeHighScoreList();
        }

        public void MakeHighScoreList()
        {

            if (File.Exists(Path))
            {
                StreamReader streamReader = File.OpenText(Path);
                string scoreLine;
                while ((scoreLine = streamReader.ReadLine()) != null)
                {
                    Scores.Add(new(int.Parse(scoreLine.Substring(0, scoreLine.IndexOf("\t")))
                        , scoreLine.Substring(scoreLine.IndexOf("\t") + 1)));
                }

                streamReader.Close();
            }
            else
            {
                StreamWriter sw = File.CreateText(Path);
                sw.Close();
            }
        }

        /// <summary>
        /// Test if the Score is Better than the score in the highscorelist
        /// </summary>
        /// <param name="score">Score to test</param>
        /// <returns>true if the score is better than the current highscores</returns>
        public bool IsHighScore(int score)
        {
            if (Scores.Count > 9)
            {
                foreach (ScoreLine sl in Scores)
                {
                    if (score > sl.Score)
                    {
                        return true;
                    }
                }
            }
            else
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Save the score in the list and in the file
        /// </summary>
        /// <param name="score">the score to save</param>
        /// <param name="name">the name of the player to save</param>
        public void SaveScore(int score, string name)
        {
            int i = 0;
            if (Scores.Count > 0)
            {
                foreach (ScoreLine sl in Scores)
                {
                    if (score > sl.Score)
                    {
                        break;
                    }
                    i++;
                }

                Scores.Insert(i, new ScoreLine(score, name));

                if (Scores.Count > 10)
                {
                    Scores.RemoveAt(Scores.Count - 1);
                }
            }
            else
            {
                Scores.Add(new(score, name));
            }            

            StreamWriter streamWriter = File.CreateText(Path);

            foreach (ScoreLine sl in Scores)
            {
                streamWriter.WriteLine($"{sl.Score}\t{sl.Name}");
            }

            streamWriter.Close();
        }

        public int GetBestScore()
        {
            if (Scores.Count > 0)
            {
                return Scores[0].Score;
            }
            else
            {
                return 0;
            }
        }

    }
}
