using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image fillImage;
    private PlayableCharacter currentCharacter;

    //Flash red light is on or off
    private bool isFlashing = false;

    public void SetCharacter(PlayableCharacter newCharacter)
    {
        currentCharacter = newCharacter;
        healthSlider.maxValue = currentCharacter.maxHp;
        healthSlider.value = currentCharacter.currentHp;
        UpdateHealthBarColor();
    }

    private void Update()
    {
        if (currentCharacter != null)
        {
            healthSlider.value = currentCharacter.currentHp;
            UpdateHealthBarColor();
        }
    }

    private void UpdateHealthBarColor()
    {
        float healthPercent = (healthSlider.value / healthSlider.maxValue) * 100;

        if (healthPercent >= 63)
        {
            fillImage.color = Color.green;
            StopFlashing();
        }
        else if (healthPercent <= 63 && healthPercent >= 41)
        {
            fillImage.color = Color.yellow;
            StopFlashing();
        }
        else if (healthPercent <= 41)
        {
            if (!isFlashing)
            {
                StartFlashing();
            }
        }

    }

    private void StartFlashing()
    {
        isFlashing = true;
        StartCoroutine(FlashRedContinuously());
    }

    private void StopFlashing()
    {
        if (isFlashing)
        {
            isFlashing = false;
            StopCoroutine(FlashRedContinuously()); // no need i think
            fillImage.color = Color.red; // no good. fix!
        }
    }

    private IEnumerator FlashRedContinuously()
    {
        while (isFlashing)
        {
            fillImage.color = new Color(1, 0, 0, 0.3f);
            yield return new WaitForSeconds(0.5f);
            fillImage.color = Color.red;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
