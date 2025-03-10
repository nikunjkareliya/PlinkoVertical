using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    [SerializeField] private List<LineSlotView> _lineSlots;

    private void Awake()
    {
        for (int i = 0; i < _lineSlots.Count; i++)
        {
            _lineSlots[i].Init();
        }

        for (int i = 0; i < _lineSlots.Count; i++)
        {
            _lineSlots[i].OnLinesChanged += OnLinesChanged;
        }
        
    }

    private void Start()
    {
        OnLinesChanged(8);
    }

    private void OnDestroy()
    {
        for (int i = 0; i < _lineSlots.Count; i++)
        {
            _lineSlots[i].OnLinesChanged -= OnLinesChanged;
        }
    }

    private void OnLinesChanged(int linesCount)
    {
        Debug.Log($"LINES COUNT -> {linesCount}");

        GameEvents.OnLinesCountChanged.Execute(linesCount);

        for (int i = 0; i < _lineSlots.Count; i++)
        {
            if (_lineSlots[i].LineNumber == linesCount)
            {
                _lineSlots[i].ChangeColorActive();
            }
            else
            {
                _lineSlots[i].ChangeColorDefault();
            }
        }
    }
}