using Shared.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BallRun3D
{
    public class HomeViewController : MonoBehaviour
    {
        [SerializeField] private HomeView _viewObject;
        private GameModel _gameModel;

        private void Awake()
        {
            _gameModel = GetGameModel();

            _viewObject.OnPlayButtonClicked += OnPlayButtonClicked;            
            _viewObject.OnBallSelectButtonClicked += OnBallSelectButtonClicked;
            GameEvents.OnLevelLoad.Register(HandleLevelLoad);
        }

        private void OnDestroy()
        {
            _viewObject.OnPlayButtonClicked -= OnPlayButtonClicked;            
            _viewObject.OnBallSelectButtonClicked -= OnBallSelectButtonClicked;
            GameEvents.OnLevelLoad.Unregister(HandleLevelLoad);
        }

        private void OnPlayButtonClicked()
        {
            //SceneManager.LoadScene("PlinkoLevel1");
            SharedEvents.OnGameStateChanged.Execute(GameState.Gameplay);
        }
        
        private void OnBallSelectButtonClicked()
        {
            SharedEvents.OnGameStateChanged.Execute(GameState.CharacterSelect);
        }

        private void HandleLevelLoad(int level)
        {
            _viewObject.UpdateCurrentLevel(level + 1);                
            _viewObject.UpdateTotalCoins(_gameModel.coinsTotal);                
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene(0);
            }
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