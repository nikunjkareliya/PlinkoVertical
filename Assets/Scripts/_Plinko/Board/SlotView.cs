using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SlotView : MonoBehaviour
{
    [SerializeField] private int _multiplier;
    [SerializeField] private TextMeshProUGUI _textMultiplier;    

    public void Init(int multiplier)
    {
        _multiplier = multiplier;
        _textMultiplier.text = multiplier.ToString();
    }

    public int GetMultiplier()
    {
        return _multiplier;
    }

}
