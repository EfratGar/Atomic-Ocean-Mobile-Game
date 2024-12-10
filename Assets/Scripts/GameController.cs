using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private PlayableCharacter character;
    List<Monster> monsters = new List<Monster>();


    private void Start()
    {
        if (monsters.Count == 0)
        {
            character.PlayerLevelsUp();
        }

    }






}
