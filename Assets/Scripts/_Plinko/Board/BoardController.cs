using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{                     
    [SerializeField] private Transform _container;
    [SerializeField] private Camera _mainCamera; // Reference to the main camera

    [Header("Camera Settings")]
    public float minCameraSize = 5f;
    public float padding = 1f; // Extra padding around the board

    [Header("Peg Settings")]
    public PegView pegPrefab;
    public float pegSpacing = 0.5f;
    public float boardCenterX = 0f;
    public float boardTopY = 4f;

    [Header("Slot Settings")]
    public SlotView slotPrefab;
    public float verticalOffset = 0.5f;    
    
    private List<PegView> _pegObjects = new List<PegView>();
    private List<SlotView> _slotObjects = new List<SlotView>();
    
    void Awake()
    {                        
        GameEvents.OnLinesCountChanged.Register(HandleLinesCountChanged);

        // If camera reference not set in inspector, get the main camera
        if (_mainCamera == null)
            _mainCamera = Camera.main;
    }

    private void OnDestroy()
    {        
        GameEvents.OnLinesCountChanged.Unregister(HandleLinesCountChanged);
    }

    private void HandleLinesCountChanged(int linesCount)
    {
        RegenerateBoard(linesCount);
    }

    private void RegenerateBoard(int linesCount)
    {
        ClearBoard();
        GenerateBoard(linesCount);
        AdjustCameraToFitBoard(linesCount);
    }

    private void ClearBoard()
    {
        // Remove all existing pegs
        foreach (var peg in _pegObjects)
        {
            Destroy(peg.gameObject);
        }
        _pegObjects.Clear();

        // Remove all existing slots
        foreach (var slot in _slotObjects)
        {
            Destroy(slot.gameObject);
        }
        _slotObjects.Clear();
    }

    private void GenerateBoard(int lines)
    {
        // Generate triangular peg pattern
        GeneratePegs(lines);

        // Generate slots at the bottom
        float bottomY = boardTopY - (lines * pegSpacing);
        GenerateSlots(lines, bottomY);
    }

    private void GeneratePegs(int lines)
    {
        // Start with 3 pegs at the top row and increase by 1 for each row
        for (int row = 0; row < lines; row++)
        {
            int pegsInRow = row + 3; // Start with 3 pegs in the first row

            // Calculate the starting X position to center the row
            float startX = boardCenterX - (pegsInRow - 1) * pegSpacing / 2f;
            float rowY = boardTopY - (row * pegSpacing);

            for (int i = 0; i < pegsInRow; i++)
            {
                Vector3 pegPosition = new Vector3(startX + (i * pegSpacing), rowY, 0);
                PegView peg = Instantiate(pegPrefab, pegPosition, Quaternion.identity, _container.transform);
                _pegObjects.Add(peg);
                peg.name = $"Peg_R{row}_C{i}";
            }
        }
    }

    private void GenerateSlots(int lines, float slotY)
    {
        // The bottom row of pegs has (lines + 2) pegs
        // We need (lines + 1) slots - one under each gap between pegs
        int slotCount = lines + 1;

        // Calculate the position of the first peg in the last row
        float lastRowFirstPegX = boardCenterX - ((lines + 2) - 1) * pegSpacing / 2f;
        // Find the middle slot index
        float middleIndex = (slotCount - 1) / 2f;

        for (int i = 0; i < slotCount; i++)
        {
            // Position each slot between two pegs
            // For a slot at position i, it goes between peg i and peg i+1 of the bottom row
            float slotX = lastRowFirstPegX + i * pegSpacing + pegSpacing / 2f;

            Vector3 slotPosition = new Vector3(slotX, slotY - verticalOffset, 0);
            SlotView slot = Instantiate(slotPrefab, slotPosition, Quaternion.identity, _container.transform);
            _slotObjects.Add(slot);
            slot.name = $"Slot_{i}";


            // Calculate multiplier based on distance from center
            float distanceFromCenter = Math.Abs(i - middleIndex);
            float normalizedDistance = distanceFromCenter / middleIndex; // 0 to 1

            // Calculate multiplier: range from min (1) to max (lines)
            int multiplier = Mathf.RoundToInt(1 + normalizedDistance * (lines - 1));

            // Set the multiplier on the slot
            slot.Init(multiplier);
        }
    }

    // Add this new method to your class
    private void AdjustCameraToFitBoard(int lines)
    {
        if (_mainCamera == null || !_mainCamera.orthographic)
            return;

        // Calculate board dimensions
        float boardHeight = (lines * pegSpacing) + verticalOffset + padding;

        // Calculate the width of the bottom row (widest part of the board)
        int bottomRowPegs = lines + 2;
        float boardWidth = (bottomRowPegs - 1) * pegSpacing + padding;

        // Calculate required camera size based on screen aspect ratio
        float aspectRatio = Screen.width / (float)Screen.height;
        float verticalSize = boardHeight / 2f;
        float horizontalSize = boardWidth / (2f * aspectRatio);

        // Use the larger of the two sizes to ensure the board fits
        float requiredSize = Mathf.Max(verticalSize, horizontalSize);

        // Ensure minimum camera size
        requiredSize = Mathf.Max(requiredSize, minCameraSize);

        // Set the camera size
        _mainCamera.orthographicSize = requiredSize;

        // Center the camera on the board
        float boardCenterY = boardTopY - (boardHeight / 2f);
        _mainCamera.transform.position = new Vector3(boardCenterX, boardCenterY, _mainCamera.transform.position.z);
    }


}
