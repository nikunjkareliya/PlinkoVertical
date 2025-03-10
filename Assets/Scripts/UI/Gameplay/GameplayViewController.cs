using Shared.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BallRun3D
{
    public class GameplayViewController : MonoBehaviour
    {
        [SerializeField] private GameplayView _viewObject;
        private GameModel _gameModel;

        private void Awake()
        {
            _gameModel = GetGameModel();            

            SharedEvents.OnGameStateChanged.Register(HandleGameStateChanged);            
            GameEvents.OnLevelCoinsUpdated.Register(HandleLevelCoinsUpdated);

            _viewObject.OnBackButtonClicked += OnBackButtonClicked;
            _viewObject.OnBallSpawned += OnBallSpawned;
        }

        private void OnDestroy()
        {            
            SharedEvents.OnGameStateChanged.Unregister(HandleGameStateChanged);                        
            GameEvents.OnLevelCoinsUpdated.Unregister(HandleLevelCoinsUpdated);

            _viewObject.OnBackButtonClicked -= OnBackButtonClicked;
            _viewObject.OnBallSpawned -= OnBallSpawned;
        }
        
        void OnBackButtonClicked()
        {
            Debug.Log($"BackButtonClicked");
            //SceneManager.LoadScene("PlinkoMain");
            SharedEvents.OnGameStateChanged.Execute(GameState.Home);
        }

        private void OnBallSpawned()
        {
            GameEvents.OnBallSpawn.Execute();
        }

        private void HandleGameStateChanged(GameState gameState)
        {
            if (gameState == GameState.Gameplay)
            {
                _viewObject.Show();
                _viewObject.UpdateCoinsLevel(0);
            }
        }

        private void HandleLevelCoinsUpdated(int levelCoins)
        {
            _viewObject.UpdateCoinsLevel(levelCoins);
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