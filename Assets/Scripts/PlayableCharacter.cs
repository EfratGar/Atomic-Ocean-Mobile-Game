using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableCharacter : MonoBehaviour
{
    private int playerHp = 100;
    private int maxAttackPower;
    private int playerLevel = 1;



    public void PlayerLevelsUp()
    {
        playerLevel++;
    }

}
