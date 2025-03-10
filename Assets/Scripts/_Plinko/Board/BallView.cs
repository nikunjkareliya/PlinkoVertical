using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallView : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _initialForce = 20f;
    
    void Start()
    {
        
    }

    public void Init()
    {
        float randomForce = Random.Range(15f, _initialForce);
        // Randomly make it positive or negative
        if (Random.value < 0.5f)
        {
            randomForce = -randomForce;
        }

        _rb.AddForce(new Vector2(randomForce, 0));

        //_rb.AddForce(new Vector2(Random.Range(-_initialForce, _initialForce), 0));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var slot = other.GetComponent<SlotView>();

        if (slot != null)
        {
            GetComponent<Collider2D>().enabled = false;
            Debug.Log($"TRIGGER!! -> {slot.GetMultiplier()}");
            GameEvents.OnScoreMultiplied.Execute(slot.GetMultiplier());

            Destroy(gameObject, 0.2f);
        }
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        var peg = other.collider.GetComponent<PegView>();

        if (peg != null)
        {
            peg.Interact();
        }
    }
}
