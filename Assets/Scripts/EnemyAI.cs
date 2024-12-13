using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    [SerializeField] Transform target;
    [SerializeField] private float stoppingDistance;

    [Header("Attack settings")]
    [SerializeField] private float attackDuration;
    [SerializeField] private float attackMagnitude;
    [SerializeField] private float postAttackSpeed;

    NavMeshAgent agent;

    private float _initPosY;
    public event Action OnAttackEnded = delegate { };
    private bool _shouldUpdateDetination;
    private float _attackSpeed;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        _attackSpeed = agent.speed;
        agent.enabled = false;
        _initPosY = transform.position.y;
        GetComponent<Monster>().OnAttackPlayer += BeginAttackOnPlayer;
        Monster.OnHitPlayer += OnHitPlayer;
    }

    private void Update()
    {
        if (agent.enabled == false)
            return;

        if(_shouldUpdateDetination)
            agent.SetDestination(target.position);
    }

    private void BeginAttackOnPlayer(Vector2 initPos)
    {
        SetNavAgentState(true);
        agent.speed = _attackSpeed;
    }

    private void SetNavAgentState(bool isActive)
    {
        agent.enabled = isActive;
        _shouldUpdateDetination = isActive;
    }

    private async void OnHitPlayer()
    {
        _shouldUpdateDetination = false;
        agent.enabled = false;
        await AttackAnimation();

        Vector2 newPos = new(transform.position.x, _initPosY);
        agent.enabled = true;
        agent.speed = postAttackSpeed;
        agent.stoppingDistance = stoppingDistance;
        agent.SetDestination(newPos);

        HasReturnedToOriginalPosition();
    }

    private async void HasReturnedToOriginalPosition()
    {
        while (agent.remainingDistance > agent.stoppingDistance || agent.hasPath || agent.pathPending)
            await Task.Yield();

        Debug.Log("Returned to original position");
        OnAttackEnded();
        agent.enabled = false;
    }

    private async Task AttackAnimation()
    {
        Vector3 directionToPlayer = (target.position - transform.position).normalized;
        Vector3 destination = transform.position + directionToPlayer * attackMagnitude;

        await LerpTo(destination, attackDuration);
    }

    private async Task LerpTo(Vector3 destination, float duration)
    {
        float time = 0f;
        while (time < duration)
        {
            transform.position = Vector3.Lerp(transform.position, destination, time);
            time += Time.deltaTime;
            await Task.Yield();
        }
    }

}
