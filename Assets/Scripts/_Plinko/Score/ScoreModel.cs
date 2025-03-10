using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreModel
{
    private int _currentScore;
    public int pegValue;

    public void SetScore(int score)
    {
        _currentScore = score;
        GameEvents.OnScoreUpdated.Execute(score);
    }

    public int GetScore()
    {
        return _currentScore;
    }
}
