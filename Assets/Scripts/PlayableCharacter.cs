using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

public class PlayableCharacter : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField] public float maxHp = 100f;
    public float currentHp { get; private set; }
    [SerializeField] TMP_Text DamageText;
    [SerializeField] private float hitCooldown;

    [Header("iFrames")]
    [SerializeField] protected float characterAlpha = 1f;
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private Color characterColor;
    protected SpriteRenderer spriteRenderer;
    bool InFlashing = false;

    private bool dead;

    private Game game;
    private void Start()
    {
        game = FindObjectOfType<Game>();
        if (DamageText == null)
            Debug.LogError("DamageText is not assigned in the Inspector");

        if (DamageText != null)
            DamageText.gameObject.SetActive(false);
        Monster.OnHitPlayer += OnGotHit;
    }
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        characterColor = spriteRenderer.color;
        currentHp = maxHp;
    }

    public void TakeDamage(int damageTaken)
    {
        if (InFlashing)
        {
            return;
        }

        currentHp = Mathf.Clamp(currentHp - damageTaken, 0, maxHp);
        Debug.Log($"Player was hit, current HP is {currentHp}");

        if (currentHp > 0)
        {
            StartCoroutine(inVulnerability());
            //SubmarinePainSound.Play();
        }
        else
        {
            if (!dead)
            {
                Die();
            }
        }
    }

    public void AddHealth(float _value)
    {
        currentHp = Mathf.Clamp(currentHp + _value, 0, maxHp);
    }

    public void Die()
    {
        // Here will be level end popup + menu + animation
        dead = true;
        Debug.Log("You died.");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Spike"))
        {
            TakeDamage(10);
        }
    }
    private IEnumerator inVulnerability()
    {
        InFlashing = true;
        Color currentColor = spriteRenderer.color;
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRenderer.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRenderer.color = currentColor;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
        InFlashing = false;

    }

    protected virtual IEnumerator FadeAlpha(float targetAlpha)
    {
        float currentAlpha = spriteRenderer.color.a;
        float duration = 0.5f;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(currentAlpha, targetAlpha, elapsedTime / duration);
            Color newColor = spriteRenderer.color;
            newColor.a = newAlpha;
            spriteRenderer.color = newColor;

            yield return null;
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

    public bool CharacterIsAlive()
    {
        if (currentHp > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
