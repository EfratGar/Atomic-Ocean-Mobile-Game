using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image fillImage;
    private PlayableCharacter currentCharacter;

    public void SetCharacter(PlayableCharacter newCharacter)
    {
        currentCharacter = newCharacter;
        healthSlider.maxValue = newCharacter.PlayerHP;
        healthSlider.value = newCharacter.CurrentPlayerHP;

        UpdateHealthBar();
    }

    private void Update()
    {
        if (currentCharacter != null)
        {
            healthSlider.value = currentCharacter.CurrentPlayerHP;
            UpdateHealthBar();
        }
    }

    private void UpdateHealthBar()
    {
        fillImage.fillAmount = healthSlider.value / healthSlider.maxValue;
    }
}
