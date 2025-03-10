using Newtonsoft.Json;
using Shared.Core;
using Shared.Core.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BallRun3D
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private LevelView[] _levels;
        private GameModel _gameModel;
        [SerializeField] private BallSelectConfig _ballSelectConfig;

        private void Awake()
        {
            _gameModel = GetGameModel();
            
            SharedEvents.OnGameStateChanged.Register(HandleGameStateChanged);
            GameEvents.OnLevelLoad.Register(HandleLevelLoad);
            GameEvents.OnLevelCompleted.Register(HandleLevelCompleted);
        }

        private void Start()
        {                                 
            GameInit();
        }

        /// <summary>
        /// Game Initialization
        /// </summary>
        private void GameInit()
        {            
            _gameModel.LoadPlayerData();

            GameEvents.OnGameInitialized.Execute();

        }

        private void OnDestroy()
        {            
            SharedEvents.OnGameStateChanged.Unregister(HandleGameStateChanged);
            GameEvents.OnLevelLoad.Unregister(HandleLevelLoad);
            GameEvents.OnLevelCompleted.Register(HandleLevelCompleted);
        }

        private void HandleGameStateChanged(GameState gameState)
        {
            
        }

        private void HandleLevelLoad(int levelID)
        {
            _gameModel.ResetLevel();

            if (levelID > 0)
            { 
                for (int i = 0; i < _levels.Length; i++)
                {
                    if (i == levelID - 1)
                    {
                        _levels[i].ShowLock();
                        _levels[i].HideLevelEndGate();
                    }
                    else
                    {
                        _levels[i].HideLock();
                        _levels[i].ShowLevelEndGate();
                    }
                }
            }
        }

        private void HandleLevelCompleted(int levelID)
        {
            _gameModel.coinsTotal += _gameModel.coinsLevel;
            GameEvents.OnTotalCoinsUpdated.Execute(_gameModel.coinsTotal);

            if (_gameModel.currentLevel >= 2)
            {
                _gameModel.currentLevel = 0;
            }
            else
            { 
                _gameModel.currentLevel++;            
            }

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

        //private void Update()
        //{            
        //    if (Input.GetKeyDown(KeyCode.R))
        //    {
        //        SceneManager.LoadScene(0);
        //    }
        //    else if (Input.GetKeyDown(KeyCode.Q))
        //    {
        //        Application.Quit();
        //    }
        //}
    }
    
}