using UnityEngine;
using UnityEditor;

public class TriangularPlinkoGenerator : EditorWindow
{
    private int totalNumberOfLines = 10;
    private int startLineNumber = 0;     // Starting line number (0 = full triangle)
    private int endLineOffset = 0;       // Number of lines to remove from the end
    private GameObject pegPrefab;
    private float horizontalSpacing = 1.0f;
    private float verticalSpacing = 0.8f;
    private Transform parentTransform;
    private bool createNewParent = true;
    private string parentName = "PlinkoBoard";

    [MenuItem("Tools/Triangular Plinko Generator")]
    public static void ShowWindow()
    {
        GetWindow<TriangularPlinkoGenerator>("Plinko Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Triangular Plinko Board Generator", EditorStyles.boldLabel);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Triangle Configuration", EditorStyles.boldLabel);
        totalNumberOfLines = EditorGUILayout.IntSlider("Total Number of Lines", totalNumberOfLines, 1, 30);
        startLineNumber = EditorGUILayout.IntSlider("Skip Lines at Start", startLineNumber, 0, Mathf.Max(0, totalNumberOfLines - 1));

        // Ensure endLineOffset doesn't make the triangle disappear
        int maxEndOffset = Mathf.Max(0, totalNumberOfLines - startLineNumber - 1);
        endLineOffset = EditorGUILayout.IntSlider("Skip Lines at End", endLineOffset, 0, maxEndOffset);

        // Show actual number of lines that will be generated
        int actualLines = totalNumberOfLines - startLineNumber - endLineOffset;
        EditorGUILayout.LabelField($"Lines to generate: {actualLines}");

        EditorGUILayout.Space();
        pegPrefab = EditorGUILayout.ObjectField("Peg Prefab", pegPrefab, typeof(GameObject), false) as GameObject;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Spacing Settings", EditorStyles.boldLabel);
        horizontalSpacing = EditorGUILayout.FloatField("Horizontal Spacing", horizontalSpacing);
        verticalSpacing = EditorGUILayout.FloatField("Vertical Spacing", verticalSpacing);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Parent Settings", EditorStyles.boldLabel);
        createNewParent = EditorGUILayout.Toggle("Create New Parent", createNewParent);

        if (!createNewParent)
        {
            parentTransform = EditorGUILayout.ObjectField("Parent Transform", parentTransform, typeof(Transform), true) as Transform;
        }
        else
        {
            parentName = EditorGUILayout.TextField("Parent Name", parentName);
        }

        EditorGUILayout.Space();

        GUI.enabled = pegPrefab != null && actualLines > 0;
        if (GUILayout.Button("Generate Plinko Board"))
        {
            GeneratePlinkoBoard();
        }
        GUI.enabled = true;

        if (pegPrefab == null)
        {
            EditorGUILayout.HelpBox("Please assign a Peg Prefab before generating the board.", MessageType.Warning);
        }

        if (actualLines <= 0)
        {
            EditorGUILayout.HelpBox("Invalid configuration. No lines would be generated.", MessageType.Error);
        }
    }

    private void GeneratePlinkoBoard()
    {
        // Create or get parent object
        Transform boardParent;
        if (createNewParent)
        {
            GameObject parentGO = new GameObject(parentName);
            boardParent = parentGO.transform;
            Undo.RegisterCreatedObjectUndo(parentGO, "Create Plinko Board Parent");
        }
        else
        {
            boardParent = parentTransform;
            if (boardParent == null)
            {
                Debug.LogError("Parent transform is not assigned!");
                return;
            }
        }

        int endLine = totalNumberOfLines - endLineOffset;
        int actualLinesGenerated = 0;

        // Start creation of pegs
        for (int line = startLineNumber; line < endLine; line++)
        {
            // Calculate the number of pegs in this line
            int pegsInLine = line + 1;

            // Calculate the starting X position to center the line
            float startX = -(pegsInLine - 1) * horizontalSpacing * 0.5f;

            // Calculate Y position for this line (top to bottom)
            // Adjust Y position by startLineNumber to make the board start at Y=0
            float yPos = -(line - startLineNumber) * verticalSpacing;

            // Create pegs for this line
            for (int pegIndex = 0; pegIndex < pegsInLine; pegIndex++)
            {
                float xPos = startX + pegIndex * horizontalSpacing;
                Vector3 pegPosition = new Vector3(xPos, yPos, 0);

                GameObject peg = PrefabUtility.InstantiatePrefab(pegPrefab) as GameObject;
                Undo.RegisterCreatedObjectUndo(peg, "Create Plinko Peg");

                peg.transform.SetParent(boardParent);
                peg.transform.localPosition = pegPosition;
                peg.name = $"Peg_Line{line}_Pos{pegIndex}";
            }

            actualLinesGenerated++;
        }

        Debug.Log($"Successfully created Plinko board with {actualLinesGenerated} lines (from line {startLineNumber} to {endLine - 1}).");
    }
}