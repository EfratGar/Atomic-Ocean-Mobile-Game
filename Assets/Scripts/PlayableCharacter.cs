using System.Threading.Tasks;
using UnityEngine;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;

public class PlayableCharacter : MonoBehaviour, IDamageable
{
    [SerializeField] private float hitCooldown;

    [Header("Death Flicker")]
    [SerializeField] private float flickerSpeed = 0.06f;

    [Header("Submarine Parts")]
    [SerializeField] private List<Transform> submarineParts;

    [Header("Explosion Settings")]
    [SerializeField] private GameObject explosionEffectPrefab; // Explosion VFX
    [SerializeField] private float explosionForce = 10f; // Explosion Power
    [SerializeField] private float torqueForce = 5f; // Rotation power

    [SerializeField] private float MassExplosionParts;
    [SerializeField] private float GravityExplosionParts;

    [Header("Hurt Settings")]
    [SerializeField] private GameObject HurtSubmarineEffectPrefab; // Explosion VFX
    private Material material;
    private Color originalColor;

    private Transform playerTransform;


    [field: SerializeField] public int PlayerHP { get; private set; } = 100;
    public int CurrentPlayerHP { get; private set; }


    private void Start()
    {
        material = GetComponent<SpriteRenderer>().material;
        originalColor = material.color;

        HealthBar healthBar = FindObjectOfType<HealthBar>();
        if (healthBar != null)
        {
            healthBar.SetCharacter(this);
        }

        CurrentPlayerHP = PlayerHP;
        playerTransform = transform;
    }

    public void TakeDamage(int damageTaken)
    {
        if (CurrentPlayerHP > 0)
        {
            CurrentPlayerHP -= damageTaken;
            Debug.Log("Player was hit, current HP is " + CurrentPlayerHP);
            DisplayDamage();

        }
        if (CurrentPlayerHP <= 0) 
            Die();
    }

    public void Die()
    {
        //Starting sumbarine explosion effect
        ExplodeSubmarine();
        // Here will be level end popup + animation
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Spike"))
        {
            TakeDamage(10);
        }
    }

    private void ExplodeSubmarine()
    {
        foreach (Transform part in submarineParts)
        {
            if (part == null) continue;

            part.SetParent(null);

            // Adding Rigidbody2D to a part
            Rigidbody2D rb = part.gameObject.GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                rb = part.gameObject.AddComponent<Rigidbody2D>();
            }
            rb.mass = MassExplosionParts;
            rb.gravityScale = GravityExplosionParts;

            // Adding random explosion power
            Vector2 explosionDirection = Random.insideUnitCircle.normalized;
            rb.AddForce(explosionDirection * explosionForce, ForceMode2D.Impulse);

            // Adding random rotation
            float randomTorque = Random.Range(-torqueForce, torqueForce);
            rb.AddTorque(randomTorque, ForceMode2D.Impulse);

            // Destroying the part after a ceratin time
            Destroy(part.gameObject, 10f);
        }

        // Turning on the explosion
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        // Play explosion sound
        if (explosionEffectPrefab != null)
        {
            AudioClip explosionSound = explosionEffectPrefab.GetComponent<AudioSource>()?.clip;
            if (explosionSound != null)
            {
                AudioSource.PlayClipAtPoint(explosionSound, transform.position); // Adjust volume as needed
            }
            else
            {
                Debug.LogError("No AudioClip found on explosionEffectPrefab.");
            }
        }
    }

    public void DisplayDamage()
    {
        StartCoroutine(FlickerEffect());
        playerTransform.DOShakePosition(0.4f, 0.3f, 10, 90, false, true);

        // Turning on the explosion
        if (HurtSubmarineEffectPrefab != null)
        {
            Instantiate(HurtSubmarineEffectPrefab, transform.position, Quaternion.identity);
        }

        // Play hurt submarine sound
        if (HurtSubmarineEffectPrefab != null)
        {
            AudioClip hurtSubmarine = HurtSubmarineEffectPrefab.GetComponent<AudioSource>()?.clip;
            if (hurtSubmarine != null)
            {
                AudioSource.PlayClipAtPoint(hurtSubmarine, transform.position); // Adjust volume as needed
            }
            else
            {
                Debug.LogError("No AudioClip found on HurtSubmarineEffectPrefab.");
            }
        }
    }

    IEnumerator FlickerEffect()
    {
        int flickerCount = 0;
        while (flickerCount < 2)
        {
            material.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.2f);
            yield return new WaitForSeconds(flickerSpeed);

            material.color = originalColor;
            yield return new WaitForSeconds(flickerSpeed);

            flickerCount++;

        }
    }

}
