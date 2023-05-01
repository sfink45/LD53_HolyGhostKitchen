using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.RecipeMiniGames
{
    public class StepRank
    {
        public enum Ranks { Perfect, Good, Okay, Poor }
        private static readonly float PERFECT_SCORE = 10;
        private static readonly float GOOD_SCORE = 7;
        private static readonly float OKAY_SCORE = 4;
        private static readonly float POOR_SCORE = 1;


        public static float GetScore(List<Ranks> ranks)
        {
            var score = 0f;
            foreach (var r in ranks)
            {
                score += GetScore(r);
            }
            return score / (ranks.Count * PERFECT_SCORE);
        }

        public static float GetScore(Ranks rank)
        {
            var score = 0f;
            switch (rank)
            {
                case Ranks.Perfect:
                    score += PERFECT_SCORE;
                    break;
                case Ranks.Good:
                    score += GOOD_SCORE;
                    break;
                case Ranks.Okay:
                    score += OKAY_SCORE;
                    break;
                default:
                    score += POOR_SCORE;
                    break;
            }
            return score;
        }
    }
}
