using Shared.Core;
using UnityEngine;

namespace PlinkoVertical
{
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
        public static readonly GameEvent<Vector3> OnBallSpawnAtPos = new GameEvent<Vector3>();

        public static readonly GameEvent<int> OnScoreUpdated = new GameEvent<int>();

        // Camera
        public static readonly GameEvent<Transform> OnCameraTargetAdd = new GameEvent<Transform>();
        public static readonly GameEvent<Transform> OnCameraTargetRemove = new GameEvent<Transform>();
    }
}