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

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
