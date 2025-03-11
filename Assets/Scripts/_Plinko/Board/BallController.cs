using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private BallFactory _ballFactory;    
    [SerializeField] private Transform _spawnPoint;    
    [SerializeField] private Transform _container;

    [SerializeField] private List<BallView> _spawnedBalls;

    private void Awake()
    {
        GameEvents.OnBallSpawn.Register(HandleBallSpawn);
        GameEvents.OnBallSpawnAtPos.Register(HandleBallSpawnAtPos);        
    }

    private void OnDestroy()
    {
        GameEvents.OnBallSpawn.Unregister(HandleBallSpawn);
        GameEvents.OnBallSpawnAtPos.Unregister(HandleBallSpawnAtPos);
    }
    
    private void HandleBallSpawn()
    {        
        CreateBall(_spawnPoint.position);
    }

    private void HandleBallSpawnAtPos(Vector3 spawnPos)
    {
        CreateBall(spawnPos);
    }

    private void CreateBall(Vector3 spawnPos)
    {
        BallView ball = _ballFactory.CreateBall();
        ball.transform.SetParent(_container);
        ball.Init(spawnPos);
        
        _spawnedBalls.Add(ball);

        // Adding spawned ball to camera target list
        GameEvents.OnCameraTargetAdd.Execute(ball.transform);
    }
}
