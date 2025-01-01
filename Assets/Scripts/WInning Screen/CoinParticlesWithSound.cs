using UnityEngine;

public class CoinParticlesWithSound : MonoBehaviour
{
    [SerializeField] private AudioSource coinSound; // AudioSource for coin sound
    [SerializeField] private float soundDelay = 0.5f; // Delay before the sound plays

    private void Start()
    {
        // Ensure the sound plays with a delay
        if (coinSound != null)
        {
            Invoke(nameof(PlayCoinSound), soundDelay);
        }
        else
        {
            Debug.LogError("CoinSound AudioSource is not assigned!");
        }
    }

    private void PlayCoinSound()
    {
        if (coinSound != null)
        {
            coinSound.Play();
        }
    }
}
