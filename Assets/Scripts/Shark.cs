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

    private void Start()
    {
        cancellationTokenSource = new CancellationTokenSource();
        AttackPlayer();
        GetComponent<EnemyAI>().OnAttackEnded += OnAttackEnded;
    }

    public override void Die()
    {
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

    private void OnAttackEnded()
    {
        OnNavAgentBasedMovementEnded();
        AttackPlayer();
    }
}
