using UnityEngine;
using System;

namespace PlinkoVertical
{
    public static class GameEvents
    {
        // Ball
        public static event Action OnBallSpawn;
        public static event Action<Vector3> OnBallSpawnAtPos;
        public static event Action<int> OnScoreUpdated;

        public static event Action<Transform> OnCameraTargetAdd;
        public static event Action<Transform> OnCameraTargetRemove;

        public static void RaiseBallSpawn()
        {
            OnBallSpawn?.Invoke();
        }

        public static void RaiseBallSpawnAtPos(Vector3 pos)
        {
            OnBallSpawnAtPos?.Invoke(pos);
        }

        public static void RaiseScoreUpdated(int score)
        {
            OnScoreUpdated?.Invoke(score);
        }

        public static void RaiseCameraTargetAdd(Transform target)
        {
            OnCameraTargetAdd?.Invoke(target);
        }

        public static void RaiseCameraTargetRemove(Transform target)
        {
            OnCameraTargetRemove?.Invoke(target);
        }
    }
}