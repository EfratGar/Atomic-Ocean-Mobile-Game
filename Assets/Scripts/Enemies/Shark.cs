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

    private Vector3 sharkLastPos;

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
        base.Die();
        sharkLastPos = transform.position;
        cancellationTokenSource.Cancel();
        Rigidbody rb = Instantiate(present, sharkLastPos, Quaternion.identity).GetComponent<Rigidbody>();
        Vector3 randomDirection = new Vector3(UnityEngine.Random.Range(-3f, 3f), -1f, UnityEngine.Random.Range(-3f, 3f));
        rb.AddForce(randomDirection, ForceMode.Impulse);
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
