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
