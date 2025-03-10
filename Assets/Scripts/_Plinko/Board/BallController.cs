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
    }

    private void OnDestroy()
    {
        GameEvents.OnBallSpawn.Register(HandleBallSpawn);
    }

    private void HandleBallSpawn()
    {
        CreateBall();
    }

    private void CreateBall()
    {
        BallView ball = Instantiate(_ballViewPrefab, _spawnPoint.position, Quaternion.identity, _container.transform);
        ball.Init();
        _spawnedBalls.Add(ball);
    }
}
