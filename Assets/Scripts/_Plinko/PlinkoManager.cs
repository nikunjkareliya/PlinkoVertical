using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlinkoManager : MonoBehaviour
{
    [Header("Disc Settings")]
    public GameObject discPrefab;
    public float discDropHeight = 5f;
    public float movementRange = 2.5f;
    public int maxDiscs = 10;
    private int discsRemaining;

    [Header("Board Settings")]
    public int minPegRows = 8;
    public int maxPegRows = 16;
    private int currentPegRows;
    public float pegSpacing = 0.5f;
    public GameObject pegPrefab;
    private List<GameObject> pegs = new List<GameObject>();
    private List<GameObject> bins = new List<GameObject>();

    [Header("UI References")]
    public Button decreaseRowsButton;
    public Button increaseRowsButton;
    public Text rowCountText;
    public TextMesh scoreText;

    [Header("Scoring")]
    public int[] binValues = { 10, 50, 100, 500, 100, 50, 10 };
    private int currentScore = 0;

    private Vector3 dropPosition;
    private bool canDrop = true;
    private bool isResetting = false;

    void Start()
    {
        // Initialize with minimum rows
        currentPegRows = minPegRows;

        // Setup initial game state
        discsRemaining = maxDiscs;
        dropPosition = new Vector3(0, discDropHeight, 0);

        // Setup UI buttons
        if (decreaseRowsButton != null)
            decreaseRowsButton.onClick.AddListener(DecreaseRows);

        if (increaseRowsButton != null)
            increaseRowsButton.onClick.AddListener(IncreaseRows);

        // Generate initial board
        GenerateBoard();
        UpdateUI();
    }

    void Update()
    {
        if (isResetting)
            return;

        // Move disc drop position left/right
        float horizontalInput = Input.GetAxis("Horizontal");
        dropPosition.x += horizontalInput * Time.deltaTime * 3f;
        dropPosition.x = Mathf.Clamp(dropPosition.x, -movementRange, movementRange);

        // Visual indicator of where disc will drop
        Debug.DrawRay(dropPosition, Vector3.down * 0.5f, Color.red);

        // Drop a disc
        if (Input.GetKeyDown(KeyCode.Space) && canDrop && discsRemaining > 0)
        {
            StartCoroutine(DropDisc());
        }
    }

    void GenerateBoard()
    {
        ClearBoard();
        GeneratePegs();
        //GenerateBins();
    }

    void ClearBoard()
    {
        // Clear existing pegs
        foreach (GameObject peg in pegs)
        {
            Destroy(peg);
        }
        pegs.Clear();

        // Clear existing bins
        foreach (GameObject bin in bins)
        {
            Destroy(bin);
        }
        bins.Clear();
    }

    void GeneratePegs()
    {
        int pegsPerRow = currentPegRows + 1;
        float startX = -(pegsPerRow - 1) * pegSpacing / 2f;
        float startY = discDropHeight - 1f;

        for (int row = 0; row < currentPegRows; row++)
        {
            // Offset every other row for triangular pattern
            float rowOffset = (row % 2 == 0) ? 0 : pegSpacing / 2f;
            int pegsInThisRow = pegsPerRow - (row % 2);

            for (int col = 0; col < pegsInThisRow; col++)
            {
                Vector3 position = new Vector3(
                    startX + col * pegSpacing + rowOffset,
                    startY - row * pegSpacing,
                    0
                );

                GameObject peg = Instantiate(pegPrefab, position, Quaternion.identity);
                pegs.Add(peg);
            }
        }
    }

    //void GenerateBins()
    //{
    //    // Create scoring bins at the bottom
    //    float binWidth = movementRange * 2 / binValues.Length;
    //    float startX = -movementRange;
    //    float binY = discDropHeight - (currentPegRows + 1) * pegSpacing;

    //    for (int i = 0; i < binValues.Length; i++)
    //    {
    //        Vector3 position = new Vector3(startX + binWidth * (i + 0.5f), binY, 0);
    //        GameObject bin = new GameObject("Bin_" + i);
    //        bin.transform.position = position;

    //        // Add collider and script to detect when discs enter
    //        BoxCollider2D collider = bin.AddComponent<BoxCollider2D>();
    //        collider.isTrigger = true;
    //        collider.size = new Vector2(binWidth * 0.9f, 0.5f);

    //        BinController binController = bin.AddComponent<BinController>();
    //        binController.binValue = binValues[i];
    //        binController.manager = this;

    //        // Add visual representation
    //        TextMesh binText = new GameObject("BinText").AddComponent<TextMesh>();
    //        binText.transform.parent = bin.transform;
    //        binText.transform.localPosition = Vector3.zero;
    //        binText.text = binValues[i].ToString();
    //        binText.alignment = TextAlignment.Center;
    //        binText.anchor = TextAnchor.MiddleCenter;
    //        binText.fontSize = 14;

    //        bins.Add(bin);
    //    }
    //}

    IEnumerator DropDisc()
    {
        canDrop = false;
        discsRemaining--;

        // Instantiate disc
        GameObject disc = Instantiate(discPrefab, dropPosition, Quaternion.identity);

        // Wait until disc comes to rest or is destroyed
        yield return new WaitForSeconds(0.5f);

        // Allow dropping again
        canDrop = true;
        UpdateUI();
    }

    public void AddScore(int points)
    {
        currentScore += points;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore + "\nDiscs: " + discsRemaining;
        }

        if (rowCountText != null)
        {
            rowCountText.text = "Rows: " + currentPegRows;
        }

        // Update button interactability
        if (decreaseRowsButton != null)
            decreaseRowsButton.interactable = (currentPegRows > minPegRows);

        if (increaseRowsButton != null)
            increaseRowsButton.interactable = (currentPegRows < maxPegRows);

        if (discsRemaining <= 0)
        {
            // Game over state
            Debug.Log("Game Over! Final Score: " + currentScore);
        }
    }

    public void DecreaseRows()
    {
        if (currentPegRows > minPegRows)
        {
            currentPegRows--;
            StartCoroutine(ResetBoard());
        }
    }

    public void IncreaseRows()
    {
        if (currentPegRows < maxPegRows)
        {
            currentPegRows++;
            StartCoroutine(ResetBoard());
        }
    }

    IEnumerator ResetBoard()
    {
        isResetting = true;

        // Wait for any active discs to clear
        yield return new WaitForSeconds(1.0f);

        // Remove any remaining discs
        GameObject[] activeDiscs = GameObject.FindGameObjectsWithTag("Disc");
        foreach (GameObject disc in activeDiscs)
        {
            Destroy(disc);
        }

        // Reset game state (optionally)
        // discsRemaining = maxDiscs;
        // currentScore = 0;

        // Generate new board with updated row count
        GenerateBoard();

        isResetting = false;
        UpdateUI();
    }
}