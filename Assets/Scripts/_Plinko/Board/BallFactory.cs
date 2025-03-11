using UnityEngine;

public class BallFactory : MonoBehaviour
{    
    [SerializeField] private BallView _ballPrefab;        

    public BallView CreateBall()
    {        
        BallView ball = Instantiate<BallView>(_ballPrefab);
        return ball;
    }

}
