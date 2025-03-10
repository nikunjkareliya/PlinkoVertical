using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class PlinkoLayoutGenerator : EditorWindow
{
    private GameObject pegPrefab;
    private float spacing = 1.0f;
    private int rows = 10;
    private int columns = 10;
    private float radius = 5.0f;
    private int density = 6;
    private float randomOffset = 0.0f;
    private bool useRandomRotation = false;
    private enum LayoutShape { Triangular, Square, Hexagon, Diamond, Snowflake, Ring, Heart, Star, Spiral, Custom }
    private LayoutShape currentShape = LayoutShape.Triangular;
    private Transform parentObject;
    private bool createNewParent = true;
    private string parentName = "PlinkoBoard";

    [MenuItem("Tools/Plinko Layout Generator")]
    public static void ShowWindow()
    {
        GetWindow<PlinkoLayoutGenerator>("Plinko Layout Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Plinko Board Generator", EditorStyles.boldLabel);

        pegPrefab = (GameObject)EditorGUILayout.ObjectField("Peg Prefab", pegPrefab, typeof(GameObject), false);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Layout Settings", EditorStyles.boldLabel);

        currentShape = (LayoutShape)EditorGUILayout.EnumPopup("Layout Shape", currentShape);
        spacing = EditorGUILayout.FloatField("Peg Spacing", spacing);

        // Show relevant parameters based on selected shape
        switch (currentShape)
        {
            case LayoutShape.Triangular:
                rows = EditorGUILayout.IntField("Number of Rows", rows);
                break;

            case LayoutShape.Square:
                rows = EditorGUILayout.IntField("Number of Rows", rows);
                columns = EditorGUILayout.IntField("Number of Columns", columns);
                break;

            case LayoutShape.Hexagon:
                radius = EditorGUILayout.FloatField("Radius (in pegs)", radius);
                break;

            case LayoutShape.Diamond:
                rows = EditorGUILayout.IntField("Diamond Size", rows);
                break;

            case LayoutShape.Snowflake:
                radius = EditorGUILayout.FloatField("Radius (in pegs)", radius);
                density = EditorGUILayout.IntSlider("Branch Density", density, 3, 12);
                break;

            case LayoutShape.Ring:
                radius = EditorGUILayout.FloatField("Radius (in pegs)", radius);
                density = EditorGUILayout.IntSlider("Ring Density", density, 6, 36);
                break;

            case LayoutShape.Heart:
                rows = EditorGUILayout.IntField("Heart Size", rows);
                break;

            case LayoutShape.Star:
                radius = EditorGUILayout.FloatField("Radius (in pegs)", radius);
                density = EditorGUILayout.IntSlider("Points", density, 5, 12);
                break;

            case LayoutShape.Spiral:
                rows = EditorGUILayout.IntField("Number of Turns", rows);
                density = EditorGUILayout.IntSlider("Spiral Density", density, 6, 36);
                break;
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Variation Settings", EditorStyles.boldLabel);
        randomOffset = EditorGUILayout.Slider("Random Position Offset", randomOffset, 0f, 0.5f);
        useRandomRotation = EditorGUILayout.Toggle("Random Rotation", useRandomRotation);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Organization", EditorStyles.boldLabel);
        createNewParent = EditorGUILayout.Toggle("Create Parent Object", createNewParent);
        if (createNewParent)
        {
            parentName = EditorGUILayout.TextField("Parent Name", parentName);
        }
        else
        {
            parentObject = (Transform)EditorGUILayout.ObjectField("Parent Transform", parentObject, typeof(Transform), true);
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Generate Plinko Board") && pegPrefab != null)
        {
            GeneratePlinkoBoard();
        }

        if (GUILayout.Button("Clear Pegs"))
        {
            ClearPegs();
        }
    }

    private void GeneratePlinkoBoard()
    {
        if (pegPrefab == null)
        {
            EditorUtility.DisplayDialog("Error", "Please assign a peg prefab!", "OK");
            return;
        }

        // Create or find parent object
        Transform boardParent;
        if (createNewParent)
        {
            GameObject parent = new GameObject(parentName);
            boardParent = parent.transform;
            Undo.RegisterCreatedObjectUndo(parent, "Create Plinko Board");
        }
        else
        {
            if (parentObject == null)
            {
                EditorUtility.DisplayDialog("Error", "Please assign a parent object or enable 'Create Parent Object'!", "OK");
                return;
            }
            boardParent = parentObject;
        }

        List<Vector2> pegPositions = new List<Vector2>();

        // Generate peg positions based on selected shape
        switch (currentShape)
        {
            case LayoutShape.Triangular:
                pegPositions = GenerateTriangularLayout();
                break;

            case LayoutShape.Square:
                pegPositions = GenerateSquareLayout();
                break;

            case LayoutShape.Hexagon:
                pegPositions = GenerateHexagonLayout();
                break;

            case LayoutShape.Diamond:
                pegPositions = GenerateDiamondLayout();
                break;

            case LayoutShape.Snowflake:
                pegPositions = GenerateSnowflakeLayout();
                break;

            case LayoutShape.Ring:
                pegPositions = GenerateRingLayout();
                break;

            case LayoutShape.Heart:
                pegPositions = GenerateHeartLayout();
                break;

            case LayoutShape.Star:
                pegPositions = GenerateStarLayout();
                break;

            case LayoutShape.Spiral:
                pegPositions = GenerateSpiralLayout();
                break;
        }

        // Instantiate pegs
        foreach (Vector2 position in pegPositions)
        {
            Vector3 pegPos = new Vector3(position.x, position.y, 0);

            // Add random offset if enabled
            if (randomOffset > 0)
            {
                pegPos.x += Random.Range(-randomOffset, randomOffset) * spacing;
                pegPos.y += Random.Range(-randomOffset, randomOffset) * spacing;
            }

            GameObject peg = PrefabUtility.InstantiatePrefab(pegPrefab) as GameObject;
            peg.transform.position = pegPos;
            peg.transform.SetParent(boardParent);

            // Apply random rotation if enabled
            if (useRandomRotation)
            {
                peg.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            }

            Undo.RegisterCreatedObjectUndo(peg, "Create Peg");
        }
    }

    private void ClearPegs()
    {
        Transform parent = createNewParent ? GameObject.Find(parentName)?.transform : parentObject;

        if (parent != null)
        {
            List<GameObject> pegsToDestroy = new List<GameObject>();

            // Collect all child objects
            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                pegsToDestroy.Add(parent.GetChild(i).gameObject);
            }

            // Destroy them
            foreach (GameObject peg in pegsToDestroy)
            {
                Undo.DestroyObjectImmediate(peg);
            }
        }
    }

    #region Layout Generation Methods

    private List<Vector2> GenerateTriangularLayout()
    {
        List<Vector2> positions = new List<Vector2>();

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col <= row; col++)
            {
                float x = (col - row / 2.0f) * spacing;
                float y = -row * spacing * 0.866f; // 0.866 = sin(60°) for equilateral triangle
                positions.Add(new Vector2(x, y));
            }
        }

        return positions;
    }

    private List<Vector2> GenerateSquareLayout()
    {
        List<Vector2> positions = new List<Vector2>();

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                float x = (col - columns / 2.0f + 0.5f) * spacing;
                float y = (row - rows / 2.0f + 0.5f) * spacing * -1; // Invert Y to grow downward
                positions.Add(new Vector2(x, y));
            }
        }

        return positions;
    }

    private List<Vector2> GenerateHexagonLayout()
    {
        List<Vector2> positions = new List<Vector2>();
        int size = Mathf.CeilToInt(radius);

        for (int q = -size; q <= size; q++)
        {
            int r1 = Mathf.Max(-size, -q - size);
            int r2 = Mathf.Min(size, -q + size);

            for (int r = r1; r <= r2; r++)
            {
                // Convert hex coordinates to screen coordinates
                float x = spacing * (3.0f / 2.0f * q);
                float y = spacing * (Mathf.Sqrt(3) / 2.0f * q + Mathf.Sqrt(3) * r) * -1;

                positions.Add(new Vector2(x, y));
            }
        }

        return positions;
    }

    private List<Vector2> GenerateDiamondLayout()
    {
        List<Vector2> positions = new List<Vector2>();
        int size = rows;

        for (int row = -size; row <= size; row++)
        {
            int columnsInRow = size - Mathf.Abs(row) + 1;

            for (int col = -columnsInRow + 1; col < columnsInRow; col++)
            {
                float x = col * spacing;
                float y = row * spacing * -1; // Invert Y to make diamond point down
                positions.Add(new Vector2(x, y));
            }
        }

        return positions;
    }

    private List<Vector2> GenerateSnowflakeLayout()
    {
        List<Vector2> positions = new List<Vector2>();

        // Add center
        positions.Add(Vector2.zero);

        // Generate branches
        for (int branch = 0; branch < density; branch++)
        {
            float angle = branch * (360f / density) * Mathf.Deg2Rad;

            for (int i = 1; i <= radius; i++)
            {
                float x = i * spacing * Mathf.Cos(angle);
                float y = i * spacing * Mathf.Sin(angle) * -1; // Invert Y
                positions.Add(new Vector2(x, y));

                // Add small perpendicular branches for snowflake effect
                if (i > radius / 3 && i % 2 == 0)
                {
                    float perpAngle1 = angle + 60 * Mathf.Deg2Rad;
                    float perpAngle2 = angle - 60 * Mathf.Deg2Rad;

                    float perpLength = spacing * (i / 3f);

                    float px1 = x + perpLength * Mathf.Cos(perpAngle1);
                    float py1 = y + perpLength * Mathf.Sin(perpAngle1) * -1;

                    float px2 = x + perpLength * Mathf.Cos(perpAngle2);
                    float py2 = y + perpLength * Mathf.Sin(perpAngle2) * -1;

                    positions.Add(new Vector2(px1, py1));
                    positions.Add(new Vector2(px2, py2));
                }
            }
        }

        return positions;
    }

    private List<Vector2> GenerateRingLayout()
    {
        List<Vector2> positions = new List<Vector2>();

        // Add concentric rings
        for (float r = 1; r <= radius; r++)
        {
            int pointsInRing = Mathf.RoundToInt(density * r / 2);

            for (int i = 0; i < pointsInRing; i++)
            {
                float angle = i * (360f / pointsInRing) * Mathf.Deg2Rad;
                float x = r * spacing * Mathf.Cos(angle);
                float y = r * spacing * Mathf.Sin(angle) * -1; // Invert Y
                positions.Add(new Vector2(x, y));
            }
        }

        return positions;
    }

    private List<Vector2> GenerateHeartLayout()
    {
        List<Vector2> positions = new List<Vector2>();
        float heartSize = rows * spacing;
        float step = heartSize / 20.0f;

        for (float x = -heartSize; x <= heartSize; x += step)
        {
            for (float y = -heartSize * 1.2f; y <= heartSize * 0.8f; y += step)
            {
                // Heart equation in 2D
                // (x^2 + y^2 - 1)^3 - x^2*y^3 = 0
                float nx = x / heartSize;
                float ny = y / heartSize;

                float value = Mathf.Pow((nx * nx + ny * ny - 1), 3) - (nx * nx * Mathf.Pow(ny, 3));

                if (Mathf.Abs(value) < 0.1f)
                {
                    positions.Add(new Vector2(x, y * -1)); // Invert Y
                }
            }
        }

        return positions;
    }

    private List<Vector2> GenerateStarLayout()
    {
        List<Vector2> positions = new List<Vector2>();
        int points = density;

        // Add center
        positions.Add(Vector2.zero);

        // Generate star points and fill
        for (float r = 1; r <= radius; r++)
        {
            float innerRadius = r * 0.4f; // Inner radius for star shape
            float outerRadius = r;

            for (int i = 0; i < points * 2; i++)
            {
                float angle = i * (360f / (points * 2)) * Mathf.Deg2Rad;
                float currentRadius = i % 2 == 0 ? outerRadius : innerRadius;

                float x = currentRadius * spacing * Mathf.Cos(angle);
                float y = currentRadius * spacing * Mathf.Sin(angle) * -1; // Invert Y

                positions.Add(new Vector2(x, y));
            }

            // Add some infill between points
            if (r > 2)
            {
                for (int p = 0; p < points; p++)
                {
                    float angle1 = p * (360f / points) * Mathf.Deg2Rad;
                    float angle2 = ((p + 1) % points) * (360f / points) * Mathf.Deg2Rad;

                    float midAngle = (angle1 + angle2) / 2;
                    float midRadius = r * 0.7f;

                    float x = midRadius * spacing * Mathf.Cos(midAngle);
                    float y = midRadius * spacing * Mathf.Sin(midAngle) * -1;

                    positions.Add(new Vector2(x, y));
                }
            }
        }

        return positions;
    }

    private List<Vector2> GenerateSpiralLayout()
    {
        List<Vector2> positions = new List<Vector2>();

        // Add center
        positions.Add(Vector2.zero);

        // Generate a spiral
        float angleStep = 360f / density * Mathf.Deg2Rad;
        float radiusStep = spacing / 4;

        float currentRadius = spacing;
        float angle = 0;
        int turns = rows;

        while (angle < turns * 2 * Mathf.PI)
        {
            float x = currentRadius * Mathf.Cos(angle);
            float y = currentRadius * Mathf.Sin(angle) * -1; // Invert Y

            positions.Add(new Vector2(x, y));

            angle += angleStep;
            currentRadius += radiusStep;
        }

        return positions;
    }

    #endregion
}