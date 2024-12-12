using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

public class PlayableCharacter : MonoBehaviour, IDamageable
{
    private int playerHp = 100;
    private int maxAttackPower;
    private int playerLevel = 1;
    [SerializeField] TMP_Text DamageText;

    private void Start()
    {
        DamageText.gameObject.SetActive(false);
    }




    public void PlayerLevelsUp()
    {
        playerLevel++;
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
        if (collision.gameObject.CompareTag("Monster"))
        {
                TakeDamage(25);
                DamageText.gameObject.SetActive(true);
            
        }
    }

    private async void OnTriggerExit2D(Collider2D collision)
    {
        //await Task.Delay(1000);
        DamageText.gameObject.SetActive(false);
    }

}
