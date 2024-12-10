using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour, IDamageableMonster
{

    private int currentMonsterHp;
    [SerializeField] private int maxMonsterHp = 8;
    private int monsterAttackPower;

    public void TakeDamage(int damageTaken)
    {
        if (currentMonsterHp > 0)
        {
            currentMonsterHp -= damageTaken;

        }
        else if (currentMonsterHp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("temp die message");

    }

    public int MonsterAttack()
    {
        return Random.Range(3, monsterAttackPower);
    }


}
