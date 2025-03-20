using Shared.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlinkoVertical
{
    public class HUDScoreController : MonoBehaviour
    {
        [SerializeField] private HUDScoreView _viewObject;
        
        private int _score;
        public int Score => _score;

        private void Awake()
        {                      
            GameEvents.OnScoreUpdated += HandleScoreUpdated;

            _viewObject.SetScore(0);
        }

        private void OnDestroy()
        {            
            GameEvents.OnScoreUpdated -= HandleScoreUpdated;
        }

        private void HandleScoreUpdated(int score)
        {
            _viewObject.SetScore(score);
        }


    }
}