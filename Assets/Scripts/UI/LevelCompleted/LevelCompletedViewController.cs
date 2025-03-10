using Shared.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallRun3D
{
    public class LevelCompletedViewController : MonoBehaviour
    {
        [SerializeField] private LevelCompletedView _viewObject;
        private GameModel _gameModel;

        private void Awake()
        {
            _gameModel = GetGameModel();

            _viewObject.OnNextButtonClicked += OnNextButtonClicked;
            SharedEvents.OnGameStateChanged.Register(HandleGameStateChanged);
            GameEvents.OnLevelCompleted.Register(HandleLevelCompleted);

            GameEvents.OnTotalCoinsUpdated.Register(HandleTotalCoinsUpdated);
        }

        private void OnDestroy()
        {        
            _viewObject.OnNextButtonClicked -= OnNextButtonClicked;
            SharedEvents.OnGameStateChanged.Unregister(HandleGameStateChanged);
            GameEvents.OnLevelCompleted.Unregister(HandleLevelCompleted);

            GameEvents.OnTotalCoinsUpdated.Unregister(HandleTotalCoinsUpdated);
        }

        private void OnNextButtonClicked()
        {            
            GameEvents.OnLevelLoad.Execute(_gameModel.currentLevel);
            SharedEvents.OnGameStateChanged.Execute(GameState.Home);

        }

        private void HandleGameStateChanged(GameState gameState)
        {
            if (gameState == GameState.LevelCompleted)
            {
                _viewObject.Show();
            }
        }

        private void HandleLevelCompleted(int level)
        {
            _viewObject.UpdateCurrentLevel(level + 1);
            _viewObject.UpdateLevelCoins(_gameModel.coinsLevel);
        }

        private void HandleTotalCoinsUpdated(int totalCoins)
        {
            _viewObject.UpdateTotalCoins(totalCoins);
        }

        private GameModel GetGameModel()
        {
            GameModel gameModel = ModelStore.Get<GameModel>();

            if (gameModel == null)
            {
                gameModel = new GameModel();
                ModelStore.Register<GameModel>(gameModel);
            }

            return gameModel;
        }
    }
}