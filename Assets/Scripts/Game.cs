using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class Game : MonoBehaviour
{

    [SerializeField] private HealthBar healthBar;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private UnityEngine.UI.Image gameOverBackground;
    [SerializeField] private GameObject gameOverMenu;

    void Awake()
    {
        
    }

    private void Start()
    {

        if (gameOverMenu != null)
        {
            gameOverMenu.SetActive(false); // Ensure Game Over menu is hidden initially
        }
    }
    private void Update()
    {

    }

    private void ShowGameOverMenu()
    {
        if (gameOverMenu != null)
        {
            gameOverMenu.SetActive(true); // Activate the Game Over menu
        }

        gameOverBackground.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(true);
    }

}
