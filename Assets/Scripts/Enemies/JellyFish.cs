using System.Collections;
using UnityEngine;

public class JellyFish : Monster
{
    [Header("Animation settings")]
    [SerializeField] private float swaySpeed = 2.0f;
    [SerializeField] private float swayAmount = 10.0f;

    [Header("Tentacle Animation Settings")]
    [SerializeField] private Transform[] leftTentacles; 
    [SerializeField] private Transform[] rightTentacles; 
    [SerializeField] private float tentacleSwaySpeed = 3.0f; 
    [SerializeField] private float tentacleSwayAmount = 15.0f; 

    [Header("Electric Attack")]
    [SerializeField] private ParticleSystem jellyFishShoot; // Particle System for electric attack
    [SerializeField] private AudioClip electricSound; // Electric sound for jellyfish's electric attack
    private AudioSource audioSource;

    [Header("Sound Timing")]
    [SerializeField] private float soundInterval = 2.0f; // Interval between each sound
    private float soundTimer;

    [Header("BigBoss Death Animation")]
    [SerializeField] private GameObject BigBossExplosionPrefab;


    private Vector3 startPosition;

    protected override void Start()
    {
        base.Start();

        // Setup AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = false;
        }
        audioSource.clip = electricSound;

        // Initialize Particle System
        if (jellyFishShoot != null && !jellyFishShoot.isPlaying)
        {
            jellyFishShoot.Play();
        }

        soundTimer = soundInterval;

        StartCoroutine(ElectricAttackRoutine());
    }

    private void Update()
    {
        ApplySwimAnimation();
        AnimateTentacles();
    }

    protected override void OnEnteredScene()
    {
        base.OnEnteredScene();
        startPosition = transform.position;
        jellyFishShoot.Play();
    }

    private void ApplySwimAnimation()
    {
        float swimY = Mathf.Sin(Time.time * swaySpeed * 0.5f) * 0.5f;
        Vector3 destination = new(transform.position.x, startPosition.y + swimY, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime);

        float sway = Mathf.Sin(Time.time * swaySpeed) * swayAmount;
        Quaternion targetRotation = Quaternion.Euler(0, 0, sway);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime);
    }

    private void AnimateTentacles()
    {
        float sway = Mathf.Sin(Time.time * tentacleSwaySpeed) * tentacleSwayAmount;

        foreach (var tentacle in leftTentacles)
        {
            if (tentacle != null)
            {
                tentacle.localRotation = Quaternion.Euler(0, 0, sway);
            }
        }

        foreach (var tentacle in rightTentacles)
        {
            if (tentacle != null)
            {
                tentacle.localRotation = Quaternion.Euler(0, 0, -sway);
            }
        }
    }

    private IEnumerator ElectricAttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(soundInterval);

            if (jellyFishShoot != null)
            {
                jellyFishShoot.Play();

                if (electricSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(electricSound);
                }
            }
        }
    }

    public override void Die()
    {
        StartCoroutine(HandleDeathWithExplosion());
    }
    private IEnumerator HandleDeathWithExplosion()
    {
        if (BigBossExplosionPrefab != null)
        {
            Vector3 JellyFishPosition = transform.position;

            GameObject explosion = Instantiate(BigBossExplosionPrefab, JellyFishPosition, Quaternion.identity);

            // Stop particle system and sound
            if (jellyFishShoot != null)
            {
                jellyFishShoot.Stop();
            }

            if (audioSource != null)
            {
                audioSource.Stop();
            }

            yield return new WaitForSeconds(1.5f);

            //Destroy Jellyfish
            Destroy(gameObject);

            //Destroy explosion effect 
            Destroy(explosion, 5.0f);
        }

        else
        {
            Debug.Log("effect is null");
            base.Die();
            Destroy(gameObject);
        }

    }
}
