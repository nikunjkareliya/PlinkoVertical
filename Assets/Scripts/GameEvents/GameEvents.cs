using Shared.Core;
using UnityEngine;

public static class GameEvents
{
    // GameState
    public static readonly GameEvent OnGameInitialized = new GameEvent();
    
    public static readonly GameEvent<int> OnLevelLoad = new GameEvent<int>();
    public static readonly GameEvent<int> OnLevelCompleted = new GameEvent<int>();

    public static readonly GameEvent OnLevelFailed = new GameEvent();

    public static readonly GameEvent<int> OnLevelCoinsUpdated = new GameEvent<int>();
    public static readonly GameEvent<int> OnTotalCoinsUpdated = new GameEvent<int>();

    // Lines
    public static readonly GameEvent<int> OnLinesCountChanged = new GameEvent<int>();

    // Ball
    public static readonly GameEvent OnBallSpawn = new GameEvent();

    public static readonly GameEvent<int> OnScoreUpdated = new GameEvent<int>();
    public static readonly GameEvent<int> OnScoreAdded = new GameEvent<int>();
    public static readonly GameEvent<int> OnScoreMultiplied = new GameEvent<int>();
}
