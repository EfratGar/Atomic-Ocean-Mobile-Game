using UnityEngine;

public class JellyFish : Monster
{
    [Header("Animation settings")]
    [SerializeField] private float swaySpeed = 2.0f;
    [SerializeField] private float swayAmount = 10.0f;
    [SerializeField] private float forwardSpeed = 2.0f;
    [SerializeField] private ParticleSystem jellyFishShoot; // Particle System for electric attack
    private Vector3 startPosition;

    [Header("Sound settings")]
    [SerializeField] private AudioClip electricSound; // Electric sound for jellyfish's electric attack
    private AudioSource audioSource;
    private bool isSoundPlaying = false;

    [Header("Sound Timing")]
    [SerializeField] private float soundInterval = 2.0f; // Interval between each sound
    private float soundTimer;

    protected override void Start()
    {
        base.Start();

        // Setup AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = false; // Ensure the sound does not loop
        }
        audioSource.clip = electricSound;

        // Start particle system
        if (jellyFishShoot != null && !jellyFishShoot.isPlaying)
        {
            jellyFishShoot.Play();
        }

        soundTimer = soundInterval; // Initialize timer
    }

    private void Update()
    {
        ApplySwimAnimation();
        CheckAndPlayElectricSound();
    }

    private void CheckAndPlayElectricSound()
    {
        if (jellyFishShoot != null && jellyFishShoot.isPlaying)
        {
            // Timer to control intervals between sounds
            soundTimer -= Time.deltaTime;

            if (soundTimer <= 0f)
            {
                PlayElectricSound();
                soundTimer = soundInterval; // Reset the timer
            }
        }
        else
        {
            // Stop sound if particle system stops
            audioSource.Stop();
            isSoundPlaying = false;
        }
    }

    private void PlayElectricSound()
    {
        if (electricSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(electricSound);
            isSoundPlaying = true;
        }
    }

    private void ApplySwimAnimation()
    {
        transform.rotation = Quaternion.identity;

        Vector3 screenCenter = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.7f, Camera.main.transform.position.z * -1));
        screenCenter.z = transform.position.z;

        float swayY = Mathf.Sin(Time.time * swaySpeed) * 0.5f;
        Vector3 targetPosition = screenCenter + new Vector3(0, swayY, 0);

        transform.position = Vector3.Lerp(transform.position, targetPosition, forwardSpeed * Time.deltaTime);

        float sway = Mathf.Sin(Time.time * swaySpeed) * swayAmount;
        transform.rotation = Quaternion.Euler(0, 0, sway);
    }

    public override void Die()
    {
        base.Die();

        // Stop particle system and sound
        if (jellyFishShoot != null)
            jellyFishShoot.Stop();

        if (audioSource != null)
            audioSource.Stop();

        isSoundPlaying = false;
    }
}
