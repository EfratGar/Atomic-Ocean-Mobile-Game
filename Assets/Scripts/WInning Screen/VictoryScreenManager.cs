using UnityEngine;

public class VictoryScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject victoryScreen; // Reference to the victory screen UI

    public void Show()
    {
        if (victoryScreen != null)
        {
            victoryScreen.SetActive(true); // Activate the victory screen UI
            Debug.Log("Victory screen activated!");
        }
        else
        {
            Debug.LogWarning("Victory screen GameObject is not assigned!");
        }
    }
}
