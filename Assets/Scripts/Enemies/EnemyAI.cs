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

    [Header("Audio settings")]
    [SerializeField] private AudioClip attackSound; // Sound effect for shark biting the submarine
    [SerializeField] private AudioClip swimSound;   // Sound effect for swimming motion

    private AudioSource audioSource;
    private bool isSwimmingSoundPlaying = false;

    NavMeshAgent agent;

    private float _initPosY;
    public event Action OnAttackEnded = delegate { };
    private bool _shouldUpdateDetination;
    private float _attackSpeed;

    private void Start()
    {
        enabled = false;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        _attackSpeed = agent.speed;
        agent.enabled = false;
        _initPosY = transform.position.y;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.loop = true; // Allow looping for swimming sound
        audioSource.playOnAwake = false;

        Monster monster = GetComponent<Monster>();
        monster.OnAttackPlayer += BeginAttackOnPlayer;
        Monster.OnHitPlayer += (_) => OnHitPlayer();
        monster.OnReady += () => enabled = true;

        target = FindObjectOfType<PlayableCharacter>().transform;
    }

    private void Update()
    {
        if (agent.enabled == false)
            return;

        if (_shouldUpdateDetination)
        {
            agent.SetDestination(target.position);
            PlaySwimmingSound(); // Start swimming sound if not already playing
        }
    }

    private void BeginAttackOnPlayer()
    {
        SetNavAgentState(true);
        agent.speed = _attackSpeed;
        PlaySwimmingSound();
    }

    private void SetNavAgentState(bool isActive)
    {
        agent.enabled = isActive;
        _shouldUpdateDetination = isActive;

        if (!isActive) // Stop swimming sound if agent is not active
        {
            StopSwimmingSound();
        }
    }
    private void PlaySwimmingSound()
    {
        if (swimSound != null && audioSource != null && !isSwimmingSoundPlaying)
        {
            audioSource.clip = swimSound;
            audioSource.Play();
            isSwimmingSoundPlaying = true;
        }
    }
    private void StopSwimmingSound()
    {
        if (audioSource != null && isSwimmingSoundPlaying)
        {
            audioSource.Stop();
            isSwimmingSoundPlaying = false;
        }
    }

    private async void OnHitPlayer()
    {
        _shouldUpdateDetination = false;
        agent.enabled = false;
        StopSwimmingSound(); // Stop swimming sound before attack
        PlayAttackSound();
        await AttackAnimation();

        Vector2 newPos = new(transform.position.x, _initPosY);
        agent.enabled = true;
        agent.speed = postAttackSpeed;
        agent.stoppingDistance = stoppingDistance;
        agent.SetDestination(newPos);

        HasReturnedToOriginalPosition();
    }

    private void PlayAttackSound()
    {
        if (attackSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(attackSound);
        }
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
