using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace BallRun3D
{
    public class HomeView : BaseView
    {
        [SerializeField] private TextMeshProUGUI _textCurrentLevel;
        [SerializeField] private TextMeshProUGUI _textCoinsTotal;

        public event Action OnPlayButtonClicked;
        public event Action OnSettingsButtonClicked;
        public event Action OnBallSelectButtonClicked;

        public void ButtonPlay()
        {
            OnPlayButtonClicked?.Invoke();       
        }

        public void ButtonSettings()
        {
            OnSettingsButtonClicked?.Invoke();
        }

        public void ButtonBallSelect()
        {
            OnBallSelectButtonClicked?.Invoke();
        }

        public void UpdateCurrentLevel(int level)
        {
            _textCurrentLevel.text = $"LEVEL {level}";
        }

        public void UpdateTotalCoins(int coins)
        {
            _textCoinsTotal.text = $"{coins}";
        }

    }
}