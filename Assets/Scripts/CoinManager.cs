using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;
    [SerializeField] private TMP_Text coinsDisplay;
    private int coins;

    private void Awake()
    {
        if(!instance)
        {
            instance = this;
        }
    }

    private void OnGUI()
    {
        coinsDisplay.text = coins.ToString();
    }

    public void ChangeCoins(int amount)
    {
        coins += amount;
    }

}
