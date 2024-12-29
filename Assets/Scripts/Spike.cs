using UnityEngine;

public class Spike : MonoBehaviour
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
        if (collision.gameObject.CompareTag("Player"))
        {
            OnBecameInvisible();
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

}
