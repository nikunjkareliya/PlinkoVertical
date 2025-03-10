using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BallRun3D
{
    public class BallSelectView : BaseView
    {        
        public event Action OnBallSelectButtonClicked;
        public event Action OnBallUnlockButtonClicked;
        public event Action OnLeftClicked;
        public event Action OnRightClicked;
        public event Action OnBackButtonClicked;

        [SerializeField] private ButtonSelectView _buttonSelect;
        [SerializeField] private ButtonUnlockView _buttonUnlock;
        [SerializeField] private TextMeshProUGUI _buttonSelectedText;
        [SerializeField] private TextMeshProUGUI _buttonUnlockText;
        [SerializeField] private TextMeshProUGUI _textCoinsTotal;

        [SerializeField] private BallSelectConfig _ballSelectConfig;
        
        private GameModel _gameModel;

        public void Init(BallSelectConfig ballSelectConfig, GameModel gameModel)
        {
            _ballSelectConfig = ballSelectConfig;            
            _gameModel = gameModel;
        }

        public void ButtonRightClicked()
        {
            OnRightClicked?.Invoke();
        }

        public void ButtonLeftClicked()
        {
            OnLeftClicked?.Invoke();
        }

        public void ButtonBackClicked()
        {
            OnBackButtonClicked?.Invoke();
        }

        public void ButtonBallUnlockClicked()
        {
            OnBallUnlockButtonClicked?.Invoke();
        }

        public void ButtonBallSelectClicked()
        {
            OnBallSelectButtonClicked?.Invoke();
        }

        public void ChangeBallSkin(int ballID)
        { 

        }

        public void UpdateState(int currentPageID)
        {

            if (_gameModel.ballsUnlocked.Contains(currentPageID))
            {
                _buttonSelect.gameObject.SetActive(true);
                _buttonUnlock.gameObject.SetActive(false);

            }
            else
            {
                _buttonSelect.gameObject.SetActive(false);
                _buttonUnlock.gameObject.SetActive(true);
                _buttonUnlockText.text = $"{_ballSelectConfig.balls[currentPageID].cost}";
            }

            for (int i = 0; i < _gameModel.totalBalls; i++)
            {
                if (currentPageID == _gameModel.currentBall)
                {
                    _buttonSelectedText.text = $"SELECTED";
                }
                else
                {
                    _buttonSelectedText.text = $"SELECT";
                }
            }            

        }

        public void UpdateTotalCoins(int coins)
        {
            _textCoinsTotal.text = $"{coins}";
        }
    }
}