using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 10f;
    private Rigidbody2D rb;
    public event Action OnDoubleBullet = delegate { };

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
        else if(collision.gameObject.CompareTag("Present"))
        {
            OnDoubleBullet();
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

}
