using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int value;
    private bool hasTriggered;

    private CoinManager coinManager;

    Rigidbody2D rb;

    private void Start()
    {
        coinManager = CoinManager.instance;
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            coinManager.ChangeCoins(value);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Ground"))
        {
            rb.velocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Static;
        }
    }

}
