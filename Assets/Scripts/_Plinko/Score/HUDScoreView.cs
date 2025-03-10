using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDScoreView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textScore;

    public void SetScore(int score)
    {
        _textScore.text = score.ToString();
    }
}
