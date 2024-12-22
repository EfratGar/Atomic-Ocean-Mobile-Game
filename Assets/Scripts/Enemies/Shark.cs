using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Shark : Monster
{
    [SerializeField] private float attackRate;
    public event Action OnNavAgentBasedMovementEnded = delegate { };
    private CancellationTokenSource cancellationTokenSource;
    [SerializeField] private GameObject present;
    [SerializeField] private int damagePlayerTakes;
    [SerializeField] private GameObject explosionPrefab;


    protected override void Start()
    {
        base.Start();
        cancellationTokenSource = new CancellationTokenSource();

        GetComponent<EnemyAI>().OnAttackEnded += OnAttackEnded;
    }

    protected override void OnEnteredScene()
    {
        base.OnEnteredScene();
        AttackPlayer();
    }

    public override void Die()
    {

        if (explosionPrefab != null)
        {
            Vector3 sharkPosition = transform.position;

            GameObject explosion = Instantiate(explosionPrefab, sharkPosition, Quaternion.identity);

            Destroy(explosion, 2.0f); 
        }

        base.Die();

        cancellationTokenSource.Cancel();
    }


    private async void AttackPlayer()
    {
        await Task.Delay(TimeSpan.FromSeconds(attackRate));
        if (cancellationTokenSource.Token.IsCancellationRequested)
            return;
        InvokeOnAttackPlayerEvent();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.gameObject.CompareTag("Player"))
            InvokeOnHitPlayerEvent(damagePlayerTakes);
    }

    private void OnAttackEnded()
    {
        OnNavAgentBasedMovementEnded();
        AttackPlayer();
    }
}
