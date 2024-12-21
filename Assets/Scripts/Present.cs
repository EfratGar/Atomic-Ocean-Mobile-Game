using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Present : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    private bool hasTriggered;

    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            Instantiate(bullet);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Ground"))
        {
            rb.velocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Static;
        }
    }

}
