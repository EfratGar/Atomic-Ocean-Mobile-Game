using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

public class PlayableCharacter : MonoBehaviour, IDamageable
{
    private int playerHp = 100;
    [SerializeField] TMP_Text DamageText;
    [SerializeField] private float hitCooldown;

    private void Start()
    {
        if(DamageText != null)
            DamageText.gameObject.SetActive(false);
        Monster.OnHitPlayer += OnGotHit;
    }

    public void TakeDamage(int damageTaken)
    {
        if (playerHp > 0)
        {
            playerHp -= damageTaken;
            Debug.Log("Player was hit, current HP is " + playerHp);

        }
        if (playerHp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        // Here will be level end popup + menu + animation
        Debug.Log("You died.");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Spike"))
        {
            TakeDamage(10);
        }
    }

    private void OnGotHit()
    {
        TakeDamage(25);
        DisplayDamage();
    }

    private async void DisplayDamage()
    {
        DamageText.gameObject.SetActive(true);
        await Task.Delay(1000);
        DamageText.gameObject.SetActive(false);
    }

}
