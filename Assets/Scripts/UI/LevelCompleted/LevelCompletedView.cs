using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace BallRun3D
{
    public class LevelCompletedView : BaseView
    {
        [SerializeField] private TextMeshProUGUI _textCurrentLevel;
        [SerializeField] private TextMeshProUGUI _textLevelCoins;
        [SerializeField] private TextMeshProUGUI _textTotalCoins;
        public event Action OnNextButtonClicked;        

        public void ButtonNext()
        {
            OnNextButtonClicked?.Invoke();
        }

        public void UpdateCurrentLevel(int level)
        {
            _textCurrentLevel.text = $"LEVEL {level}";
        }

        public void UpdateLevelCoins(int coins)
        {
            _textLevelCoins.text = $"{coins} Collected";
        }

        public void UpdateTotalCoins(int coins)
        {
            _textTotalCoins.text = $"{coins}";
        }
    }
}