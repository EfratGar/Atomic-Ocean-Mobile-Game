using System.Threading.Tasks;
using UnityEngine;
using TMPro;

public class PlayableCharacter : MonoBehaviour, IDamageable
{
    [SerializeField] TMP_Text DamageText;
    [SerializeField] private float hitCooldown;


    [field: SerializeField] public int PlayerHP { get; private set; } = 100;
    public int CurrentPlayerHP { get; private set; }


    private void Start()
    {
        if(DamageText != null)
            DamageText.gameObject.SetActive(false);
        Monster.OnHitPlayer += OnGotHit;

        HealthBar healthBar = FindObjectOfType<HealthBar>();
        if (healthBar != null)
        {
            healthBar.SetCharacter(this);
        }

        CurrentPlayerHP = PlayerHP;
    }

    public void TakeDamage(int damageTaken)
    {
        if (CurrentPlayerHP > 0)
        {
            CurrentPlayerHP -= damageTaken;
            Debug.Log("Player was hit, current HP is " + CurrentPlayerHP);

        }
        if (CurrentPlayerHP <= 0) 
            Die();
    }

    public void Die()
    {
        // Here will be level end popup + menu + animation
        Debug.Log("You died.");
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Spike"))
        {
            TakeDamage(10);
        }
    }
    private void OnGotHit(int damageTaken)
    {
        TakeDamage(damageTaken);
        DisplayDamage();
    }

    private async void DisplayDamage()
    {
        DamageText.gameObject.SetActive(true);
        await Task.Delay(1000);
        DamageText.gameObject.SetActive(false);
    }

}
