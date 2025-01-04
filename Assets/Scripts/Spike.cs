using UnityEditor.UIElements;
using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] protected float bulletSpeed = 10f;
    [SerializeField] protected string opposingTag;
    protected Rigidbody2D rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {
        rb.velocity = new Vector2(0, bulletSpeed);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(opposingTag))
        {
            OnBecameInvisible();
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
