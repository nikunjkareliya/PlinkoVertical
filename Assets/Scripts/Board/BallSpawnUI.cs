using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlinkoVertical
{
    public class BallSpawnUI : MonoBehaviour
    {
        public void BallSpawn()
        {
            GameEvents.OnBallSpawn.Execute();
        }
    }
}