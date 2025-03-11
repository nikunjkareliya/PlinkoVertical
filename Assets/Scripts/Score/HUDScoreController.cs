using Shared.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlinkoVertical
{
    public class HUDScoreController : MonoBehaviour
    {
        [SerializeField] private HUDScoreView _viewObject;

        private ScoreModel _scoreModel;

        private void Awake()
        {
            _scoreModel = GetGameModel();
            GameEvents.OnScoreUpdated.Register(HandleScoreUpdated);

            _scoreModel.SetScore(0);

        }

        private void OnDestroy()
        {
            GameEvents.OnScoreUpdated.Unregister(HandleScoreUpdated);
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
}