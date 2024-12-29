using System;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Present : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;

    public static event Action OnCollected = delegate { };

    [Header("Audio Settings")]
    [SerializeField] private AudioClip collectPresentSound; // Collecting present sound effect
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnCollected();
            PlayCollectSound();
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Ground"))
        {
            rb.velocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Static;
        }
    }
    private void PlayCollectSound()
    {
        if (collectPresentSound != null)
        {
            AudioSource.PlayClipAtPoint(collectPresentSound, transform.position);
        }
    }
}
