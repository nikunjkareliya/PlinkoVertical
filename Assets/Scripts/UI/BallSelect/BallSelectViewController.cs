using Shared.Core;
using Shared.Core.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallRun3D
{
    public class BallSelectViewController : MonoBehaviour
    {
        [SerializeField] private BallSelectView _viewObject;
        [SerializeField] private BallSelectConfig _ballSelectConfig;
        private GameModel _gameModel;
        [SerializeField] private int _current = 0;

        private void Awake()
        {
            _gameModel = GetGameModel();
            SharedEvents.OnGameStateChanged.Register(HandleGameStateChanged);

            _viewObject.OnBackButtonClicked += OnBackButtonClicked;

            _viewObject.OnLeftClicked += OnLeftClicked;
            _viewObject.OnRightClicked += OnRightClicked;
            _viewObject.OnBallSelectButtonClicked += OnBallSelectButtonClicked;
            _viewObject.OnBallUnlockButtonClicked += OnBallUnlockButtonClicked;


            GameEvents.OnTotalCoinsUpdated.Register(HandleTotalCoinsUpdated);            
            GameEvents.OnGameInitialized.Register(HandleGameInitialized);            
        }

        private void OnDestroy()
        {
            SharedEvents.OnGameStateChanged.Unregister(HandleGameStateChanged);            
            
            _viewObject.OnBackButtonClicked -= OnBackButtonClicked;

            _viewObject.OnLeftClicked -= OnLeftClicked;
            _viewObject.OnRightClicked -= OnRightClicked;
            _viewObject.OnBallSelectButtonClicked -= OnBallSelectButtonClicked;
            _viewObject.OnBallUnlockButtonClicked -= OnBallUnlockButtonClicked;

            GameEvents.OnTotalCoinsUpdated.Unregister(HandleTotalCoinsUpdated);
            GameEvents.OnGameInitialized.Unregister(HandleGameInitialized);
        }

        private void HandleGameInitialized()
        {            
            _viewObject.Init(_ballSelectConfig, _gameModel);
            
        }

        private void HandleTotalCoinsUpdated(int totalCoins)
        {
            _viewObject.UpdateTotalCoins(totalCoins);
        }

        private void OnBackButtonClicked()
        {
            _viewObject.Hide();

            SharedEvents.OnGameStateChanged.Execute(GameState.Home);
           

            // Creating data that could be saved in json
            PlayerSaveData playerData = new PlayerSaveData();
            playerData.levelID = _gameModel.currentLevel;
            playerData.ballID = _gameModel.currentBall;
            playerData.coins = _gameModel.coinsTotal;

            playerData.isMusicOn = _gameModel.isMusicOn;
            playerData.isSfxOn = _gameModel.isSfxOn;

            playerData.ballsUnlocked = _gameModel.ballsUnlocked;

            GameDataHelper.SaveDataLocally<PlayerSaveData>(playerData);
        }
        
        private void OnLeftClicked()
        {
            if (_current > 0)
            {
                _current--;
               
                _viewObject.UpdateState(_current);
            }
            //Debug.Log($"LEFT -> {_current}");
        }

        private void OnRightClicked()
        {
            if (_current < _ballSelectConfig.balls.Length - 1) //2)
            { 
                _current++;
                
                _viewObject.UpdateState(_current);
            }
            //Debug.Log($"RIGHT -> {_current}");
        }

        private void OnBallUnlockButtonClicked()
        {
            if (_gameModel.coinsTotal > _ballSelectConfig.balls[_current].cost)
            {                
                _gameModel.coinsTotal -= _ballSelectConfig.balls[_current].cost;
                GameEvents.OnTotalCoinsUpdated.Execute(_gameModel.coinsTotal);

                _gameModel.currentBall = _current;
                
                if (!_gameModel.ballsUnlocked.Contains(_current))
                { 
                    _gameModel.ballsUnlocked.Add(_current);
                }

                _viewObject.UpdateState(_current);
            }
            else
            { 
                Debug.Log($"You don't have enough coins!!");            
            }
        }

        private void OnBallSelectButtonClicked()
        {
            _gameModel.currentBall = _current;
            _viewObject.UpdateState(_current);
        }

        private void HandleGameStateChanged(GameState gameState)
        {
            if (gameState == GameState.CharacterSelect)
            {
                _current = _gameModel.currentBall; // 0
                _viewObject.Show();
                _viewObject.UpdateTotalCoins(_gameModel.coinsTotal);
                _viewObject.UpdateState(_current);
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