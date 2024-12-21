using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Monster : MonoBehaviour, IDamageable
{
    public delegate void DamageAction(int howMuchDamage);

    private int _currentMonsterHp;
    [SerializeField] private int maxMonsterHp = 8;
    private int _monsterAttackPower;
    [SerializeField] GameObject coin;
    private Vector3 monsterLastPos;


    public event Action OnAttackPlayer = delegate { };
    public static event DamageAction OnHitPlayer = delegate { };
    public event Action OnReady = delegate { };
    public static event Action OnMonsterDied = delegate { };



    protected virtual void Start()
    {
        enabled = false;
        _currentMonsterHp = maxMonsterHp;
        GetComponent<EnterLevel>().OnEnteredScene += OnEnteredScene;
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

    protected virtual void OnEnteredScene() 
    {
        enabled = true;
        OnReady();
    }

    public virtual void Die()
    {
        //Here will be dead monster animation
        monsterLastPos = transform.position;
        Destroy(gameObject);
        Instantiate(coin, monsterLastPos, Quaternion.identity);

    }

    private void OnDestroy()
    {
        OnMonsterDied();
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
        }
    }

    protected void InvokeOnAttackPlayerEvent()
    {
        OnAttackPlayer();
    }



}
