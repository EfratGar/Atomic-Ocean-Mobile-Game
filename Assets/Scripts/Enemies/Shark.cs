using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Shark : Monster
{
    [SerializeField] private float attackRate;
    public event Action OnNavAgentBasedMovementEnded = delegate { };
    private CancellationTokenSource cancellationTokenSource;
    [SerializeField] private GameObject present;
    [SerializeField] private int damagePlayerTakes;
    [SerializeField] private GameObject sharkExplosionPrefab;

    [Header("Shark Parts")]
    [SerializeField] private Transform head;
    [SerializeField] private Transform leftFin;
    [SerializeField] private Transform rightFin;
    [SerializeField] private Transform tail;

    [SerializeField] private float partExplosionForce = 3f; 

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
        if (sharkExplosionPrefab != null)
        {
            Vector3 sharkPosition = transform.position;

            GameObject explosion = Instantiate(sharkExplosionPrefab, sharkPosition, Quaternion.identity);

            Destroy(explosion, 2.0f);
        }

        BreakApart();

        base.Die();
        sharkLastPos = transform.position;
        cancellationTokenSource.Cancel();
        Rigidbody rb = Instantiate(present, sharkLastPos, Quaternion.identity).GetComponent<Rigidbody>();
        Vector3 randomDirection = new Vector3(UnityEngine.Random.Range(-3f, 3f), -1f, UnityEngine.Random.Range(-3f, 3f));
        rb.AddForce(randomDirection, ForceMode.Impulse);
    }

    private void BreakApart()
    {
        if (head != null) DetachAndExplode(head);
        if (leftFin != null) DetachAndExplode(leftFin);
        if (rightFin != null) DetachAndExplode(rightFin);
        if (tail != null) DetachAndExplode(tail);
    }

    private void DetachAndExplode(Transform part)
    {
        part.SetParent(null);

        SceneManager.MoveGameObjectToScene(part.gameObject, SceneManager.GetActiveScene());

        Rigidbody2D rb = part.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = part.gameObject.AddComponent<Rigidbody2D>();
        }

        Vector2 explosionDirection = UnityEngine.Random.insideUnitCircle.normalized;

        rb.AddForce(explosionDirection * partExplosionForce, ForceMode2D.Impulse);

        float randomTorque = UnityEngine.Random.Range(-10f, 10f);
        rb.AddTorque(randomTorque, ForceMode2D.Impulse);

        rb.gravityScale = 0f;

        Destroy(part.gameObject, 4f);
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
