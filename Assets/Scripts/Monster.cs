using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Monster : MonoBehaviour, IDamageable
{

    private int _currentMonsterHp;
    [SerializeField] private int maxMonsterHp = 8;
    private int _monsterAttackPower;


    public event Action OnAttackPlayer = delegate { };
    public static event Action OnHitPlayer = delegate { };



    private void Start()
    {
        _currentMonsterHp = maxMonsterHp;
    }

    public void TakeDamage(int damageTaken)
    {
        if (_currentMonsterHp > 0)
        {
            _currentMonsterHp -= damageTaken;
            Debug.Log("Monster's damaged");
        }
        if (_currentMonsterHp <= 0)
        {
            Die();
            Debug.Log("Monster died.");
        }
    }

    public virtual void Die()
    {
        //Here will be dead monster animation
        Destroy(gameObject);

    }

    public int MonsterAttack()
    {
        return UnityEngine.Random.Range(3, _monsterAttackPower);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(2);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            OnHitPlayer();
            //TakeDamage(25);
            //DamageText.gameObject.SetActive(true);
        }
    }

    protected void InvokeOnAttackPlayerEvent()
    {
        OnAttackPlayer();
    }



}
