using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    [SerializeField] Transform target;
    [SerializeField] private float stoppingDistance;

    NavMeshAgent agent;

    private float _initPosY;
    public event Action OnAttackEnded = delegate { };
    private bool _shouldUpdateDetination;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.enabled = false;

        GetComponent<Monster>().OnAttackPlayer += BeginAttackOnPlayer;
        Monster.OnHitPlayer += OnHitPlayer;
    }

    private void Update()
    {
        if(_shouldUpdateDetination)
            agent.SetDestination(target.position);
    }

    private void BeginAttackOnPlayer(Vector2 initPos)
    {
        _initPosY = initPos.y;
        SetNavAgentState(true);
    }

    private void SetNavAgentState(bool isActive)
    {
        agent.enabled = isActive;
        _shouldUpdateDetination = isActive;
    }

    private void OnHitPlayer()
    {
        Vector2 newPos = new(transform.position.x, _initPosY);
        //SetNavAgentState(false);
        agent.SetDestination(newPos);
        _shouldUpdateDetination = false;
        HasReturnedToOriginalPosition();
    }

    private async void HasReturnedToOriginalPosition()
    {
        while (agent.remainingDistance > agent.stoppingDistance || agent.hasPath)
            await Task.Yield();

        Debug.Log("Returned to original position");
        OnAttackEnded();
        agent.enabled = false;
    }

}
