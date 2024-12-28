using System.Threading.Tasks;
using UnityEngine;
using System.Collections;
using DG.Tweening;

public class PlayableCharacter : MonoBehaviour, IDamageable
{
    [SerializeField] private float hitCooldown;

    [Header("Death Flicker")]
    [SerializeField] private float flickerSpeed = 0.06f;

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

    public void DisplayDamage()
    {
        StartCoroutine(FlickerEffect());
        playerTransform.DOShakePosition(0.4f, 0.3f, 10, 90, false, true);
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
