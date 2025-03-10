using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LineSlotView : MonoBehaviour
{
    [SerializeField] private int _lineNumber;
    public int LineNumber => _lineNumber;

    [SerializeField] private Color _defaultColor = Color.white;
    [SerializeField] private Color _selectedColor = Color.white;
    [SerializeField] private TextMeshProUGUI _textLineNumber;
    [SerializeField] private Image _image;

    public event Action<int> OnLinesChanged = (line) => { };
    
    public void Init()
    {
        _textLineNumber.text = _lineNumber.ToString();
        ChangeColorDefault();
    }

    public void ButtonSelectLines()
    {
        OnLinesChanged?.Invoke(_lineNumber);
    }

    public void ChangeColorDefault()
    {
        _image.color = _defaultColor;
    }

    public void ChangeColorActive()
    {
        _image.color = _selectedColor;
    }
}
