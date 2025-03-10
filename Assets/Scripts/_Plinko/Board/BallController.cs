using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private BallView _ballViewPrefab;
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
        CreateBall();
    }

    private void HandleBallSpawnAtPos(Vector3 spawnPos)
    {
        CreateBall(spawnPos);
    }

    private void CreateBall()
    {
        BallView ball = Instantiate(_ballViewPrefab, _spawnPoint.position, Quaternion.identity, _container.transform);
        GameEvents.OnCameraTargetAdd.Execute(ball.transform);

        ball.Init();
        _spawnedBalls.Add(ball);
    }

    private void CreateBall(Vector3 spawnPos)
    {
        BallView ball = Instantiate(_ballViewPrefab, spawnPos, Quaternion.identity, _container.transform);
        GameEvents.OnCameraTargetAdd.Execute(ball.transform);

        ball.Init();
        _spawnedBalls.Add(ball);
    }
}
