using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlinkoVertical
{
    public class ScoreModel
    {
        private int _score;
        public int Score => _score;

        public void SetScore(int score)
        {
            _score = score;
            GameEvents.OnScoreUpdated.Execute(score);
        }

    }
}