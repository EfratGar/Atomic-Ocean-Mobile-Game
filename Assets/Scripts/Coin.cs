using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int value;
    private bool hasTriggered;

    private CoinManager coinManager;
    private Tweener _coinFallTween;
    private SpriteRenderer coinRenderer;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip collectSound; // Collecting coin sound effect

    Rigidbody2D rb;

    private void Start()
    {
        coinManager = CoinManager.instance;
        rb = GetComponent<Rigidbody2D>();
        coinRenderer = GetComponent<SpriteRenderer>();
        CoinFall();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            PlayCollectSound();
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
            FadeCoin();
        }
    }
    private void PlayCollectSound()
    {
        if (collectSound != null)
        {
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
        }
    }

    private void CoinFall()
    {
        _coinFallTween = transform.DORotate(new Vector3(360f, 0f, 0f), 2f, RotateMode.FastBeyond360)
        .SetLoops(-1, LoopType.Restart)
        .SetRelative()
        .SetEase(Ease.Linear);
    }

    private async void FadeCoin()
    {
        await Task.Delay(3000);
        await coinRenderer.DOFade(0f, 1f).AsyncWaitForCompletion();
        Destroy(gameObject);
    }

}
