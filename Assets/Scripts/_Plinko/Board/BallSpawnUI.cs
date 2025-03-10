using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawnUI : MonoBehaviour
{

    public void BallSpawn()
    {
        GameEvents.OnBallSpawn.Execute();
    }
}
