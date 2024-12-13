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

    [SerializeField] private float attackRate;
    public event Action<Vector2> OnAttackPlayer = delegate { };
    public static event Action OnHitPlayer = delegate { };
    public event Action OnNavAgentBasedMovementEnded = delegate { };
    private CancellationTokenSource cancellationTokenSource;


    private void Start()
    {
        _currentMonsterHp = maxMonsterHp;
        cancellationTokenSource = new CancellationTokenSource();
        AttackPlayer();
        GetComponent<EnemyAI>().OnAttackEnded += OnAttackEnded;
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

    public void Die()
    {
        //Here will be dead monster animation
        Destroy(gameObject);
        cancellationTokenSource.Cancel();
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

    private async void AttackPlayer()
    {
        await Task.Delay(TimeSpan.FromSeconds(attackRate));
        if (cancellationTokenSource.Token.IsCancellationRequested)
            return;
        OnAttackPlayer(transform.position);
    }

    private void OnAttackEnded()
    {
        OnNavAgentBasedMovementEnded();
        AttackPlayer();
    }



}
