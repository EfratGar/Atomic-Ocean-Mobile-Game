using DG.Tweening;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int value;
    private bool hasTriggered;

    private CoinManager coinManager;
    private Tweener _coinFallTween;

    Rigidbody2D rb;

    private void Start()
    {
        coinManager = CoinManager.instance;
        rb = GetComponent<Rigidbody2D>();
        CoinFall();

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
            _coinFallTween.Kill();
            transform.rotation = Quaternion.identity;
            transform.DORotate(new Vector3(0f, 360f, 0f), 2f, RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Restart)
                .SetRelative()
                .SetEase(Ease.Linear);

            rb.velocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Static;
        }
    }

    private void CoinFall()
    {
        _coinFallTween = transform.DORotate(new Vector3(360f, 0f, 0f), 2f, RotateMode.FastBeyond360)
        .SetLoops(-1, LoopType.Restart)
        .SetRelative()
        .SetEase(Ease.Linear);

    }

}
