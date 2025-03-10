using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PegX4 : MonoBehaviour, IPeg
{
    public PegConfig pegConfig;
    [SerializeField] private float _offset = 1f;
    private Collider2D _collider;

    void Start()
    {
        _collider = GetComponent<Collider2D>();
    }

    public void Interact()
    {
        Debug.Log($"Hit with peg of type -> {pegConfig.pegType}");
        _collider.enabled = false;
        StartCoroutine(RoutineSpawnBalls(4));
    }

    private IEnumerator RoutineSpawnBalls(int ballsCount)
    {
        for (int i = 0; i < ballsCount; i++)
        {
            yield return new WaitForSeconds(0.1f);
            GameEvents.OnBallSpawnAtPos.Execute(this.transform.position + Vector3.down * _offset);            
        }

        yield return new WaitForSeconds(1f);
        
        _collider.enabled = true;
    }
}
