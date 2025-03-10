using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BallRun3D
{
    public class GameplayView : BaseView
    {
        [SerializeField] private TextMeshProUGUI _textCoinsLevel;
        public event Action OnBackButtonClicked = () => { };        
        public event Action OnBallSpawned = () => { };        
        
        public void ButtonBack()
        {
            OnBackButtonClicked?.Invoke();
        }

        public void ButtonBallSpawn()
        {
            OnBallSpawned?.Invoke();
        }

        public void UpdateCoinsLevel(int collectedCoins)
        {
            _textCoinsLevel.text = $"{collectedCoins}";
        }
    }
}