using Shared.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDScoreController : MonoBehaviour
{
    [SerializeField] private HUDScoreView _viewObject;

    private ScoreModel _scoreModel;

    private void Awake()
    {
        _scoreModel = GetGameModel();
        GameEvents.OnScoreUpdated.Register(HandleScoreUpdated);
        GameEvents.OnScoreAdded.Register(HandleScoreChanged);
        GameEvents.OnScoreMultiplied.Register(HandleScoreMultiplied);

        _scoreModel.SetScore(0);

    }

    private void OnDestroy()
    {
        GameEvents.OnScoreUpdated.Unregister(HandleScoreUpdated);
        GameEvents.OnScoreAdded.Unregister(HandleScoreChanged);
        GameEvents.OnScoreMultiplied.Unregister(HandleScoreMultiplied);
    }

    private void HandleScoreMultiplied(int multiplier)
    {
        var currScore = _scoreModel.GetScore();
        _scoreModel.SetScore(currScore * multiplier);
    }

    private void HandleScoreChanged(int scoreToBeAdded)
    {
        var currScore =_scoreModel.GetScore();
        _scoreModel.SetScore(currScore + scoreToBeAdded);
    }

    private void HandleScoreUpdated(int score)
    {
        _viewObject.SetScore(score);
    }

    private ScoreModel GetGameModel()
    {
        ScoreModel scoreModel = ModelStore.Get<ScoreModel>();

        if (scoreModel == null)
        {
            scoreModel = new ScoreModel();
            ModelStore.Register<ScoreModel>(scoreModel);
        }

        return scoreModel;
    }
}
