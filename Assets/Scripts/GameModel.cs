using Newtonsoft.Json;
using Shared.Core;
using Shared.Core.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallRun3D
{
    public class GameModel
    {
        public int currentLevel;
        public int currentBall;
        public int coinsTotal;
        public int coinsLevel;
        public bool isMusicOn;
        public bool isSfxOn;
        
        public int totalBalls = 3;               
        public List<int> ballsUnlocked = new List<int>();

        public PlayerSaveData playerData = new PlayerSaveData();

        public void ResetGame()
        {
            currentLevel = 0;
            currentBall = 0;
            
            coinsTotal = 0;
            GameEvents.OnTotalCoinsUpdated.Execute(coinsTotal);

            coinsLevel = 0;
            GameEvents.OnLevelCoinsUpdated.Execute(coinsLevel);
        }

        public void ResetLevel()
        {            
            coinsLevel = 0;
            GameEvents.OnLevelCoinsUpdated.Execute(coinsLevel);
        }

        public void UpdateCoinsLevel()
        { 

        }

        public void SetSettingData(bool isMusicOn, bool isSfxOn)
        {            
            this.isMusicOn = isMusicOn;
            this.isSfxOn = isSfxOn;

            // Creating data that could be saved in json
            PlayerSaveData playerData = new PlayerSaveData();
            playerData.levelID = currentLevel;
            playerData.ballID = currentBall;
            playerData.coins = coinsTotal;

            playerData.isMusicOn = isMusicOn;
            playerData.isSfxOn = isSfxOn;

            playerData.ballsUnlocked = ballsUnlocked;

            GameDataHelper.SaveDataLocally<PlayerSaveData>(playerData);

        }

        public void LoadPlayerData()
        {
            playerData = GameDataHelper.LoadDataLocally<PlayerSaveData>();
            if (playerData != null)
            {                
                currentLevel = playerData.levelID;
                currentBall = playerData.ballID;
                coinsTotal = playerData.coins;

                isMusicOn = playerData.isMusicOn;
                isSfxOn = playerData.isSfxOn;

                ballsUnlocked.Clear();

                if (playerData.ballsUnlocked.Count > 0)
                { 
                    for (int i = 0; i < playerData.ballsUnlocked.Count; i++)
                    {
                        int ballID = playerData.ballsUnlocked[i];
                        ballsUnlocked.Add(ballID);
                    }
                }
                
                GameEvents.OnLevelLoad.Execute(currentLevel);                                

                Debug.Log($"LOAD DATA -> Level: {JsonConvert.SerializeObject(playerData)}");

                //var audio = ServiceLocator.Current.Get<IAudioService>();
                //if (isMusicOn)
                //{
                //    audio.PlayClip(AudioClips.BGMusic);
                //}
                //else
                //{
                //    audio.StopClip(AudioClips.BGMusic);
                //}
            }
            else
            {
                currentLevel = 0;
                currentBall = 0;
                coinsTotal = 100;

                isMusicOn = true;
                isSfxOn = true;

                ballsUnlocked.Add(0);
                

                GameEvents.OnLevelLoad.Execute(currentLevel);
                Debug.Log($"LOAD DATA -> FIRST SESSION -> {JsonConvert.SerializeObject(playerData)}");
            }
        }

    }
}