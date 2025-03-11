using UnityEngine;

public class BallFactory : MonoBehaviour
{    
    [SerializeField] private BallView _ballPrefab;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _container;

    public BallView CreateBall()
    {        
        BallView ball = Instantiate(_ballPrefab, _spawnPoint.position, Quaternion.identity, _container.transform);
        return ball;
    }

    //public BallView CreateBall(Vector3 position)
    //{
    //    // Create default properties based on type
    //    //Color ballColor = Color.white;
    //    //float ballSpeed = 1.0f;

    //    //switch (type)
    //    //{
    //    //    case Ball.BallType.Small:
    //    //        ballColor = Color.red;
    //    //        ballSpeed = 3.0f;
    //    //        break;
    //    //    case Ball.BallType.Medium:
    //    //        ballColor = Color.green;
    //    //        ballSpeed = 2.0f;
    //    //        break;
    //    //    case Ball.BallType.Large:
    //    //        ballColor = Color.blue;
    //    //        ballSpeed = 1.0f;
    //    //        break;
    //    //}

    //    BallView ball = Instantiate(_ballPrefab, _spawnPoint.position, Quaternion.identity, _container.transform);
    //    return ball;

    //    // Instantiate the ball
    //    //GameObject ballObject = Instantiate(ballPrefab, position, Quaternion.identity);
    //    //Ball ball = ballObject.GetComponent<Ball>();

    //    // Initialize the ball with properties
    //    //ball.Initialize(type, ballColor, ballSpeed);

    //}
}
