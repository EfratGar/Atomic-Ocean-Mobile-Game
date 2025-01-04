using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 10f;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        rb.velocity = new Vector2(0, bulletSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            OnBecameInvisible();
        }
        else if(collision.gameObject.CompareTag("Crab"))
        {
            Vector2 normal = (Vector2)transform.position - collision.ClosestPoint(transform.position);
            normal.Normalize();
            OnBounceBullet(normal);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnBounceBullet(Vector2 downDirection)
    {
        float direction = Mathf.Sign(downDirection.x);
        Vector2 reflectedDirection = Vector2.Reflect(direction * transform.right , downDirection);
        rb.velocity = reflectedDirection * rb.velocity.magnitude;
    }

}
