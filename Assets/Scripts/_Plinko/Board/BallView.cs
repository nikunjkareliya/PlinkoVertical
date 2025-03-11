using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallView : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _initialForce = 20f;
    [SerializeField] private float _minimumForce = 15f;
   
    public void Init(Vector3 pos)
    {
        SetPosition(pos);

        float randomForce = Random.Range(_minimumForce, _initialForce);
        // Randomly make it positive or negative
        if (Random.value < 0.5f)
        {
            randomForce = -randomForce;
        }

        _rb.AddForce(new Vector2(randomForce, 0));
    }

    private void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var peg = other.GetComponent<IPeg>();

        if (peg != null)
        {
            //GetComponent<Collider2D>().enabled = false;
            peg.Interact();
            //Debug.Log($"TRIGGER!! -> {peg}");
        }
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        var peg = other.collider.GetComponent<IPeg>();

        if (peg != null)
        {
            peg.Interact();
        }

        var destroyable = other.collider.GetComponent<IDestroyable>();

        if (destroyable != null)
        {
            GameEvents.OnCameraTargetRemove.Execute(this.transform);

            _rb.isKinematic = true;
            
            transform.DOScale(0f, 0.25f).OnComplete(() => {                
                Destroy(gameObject);
            });
        }
    }
}
