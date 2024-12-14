using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class JellyFish : Monster
{
    [Header("Animation settings")]
    [SerializeField] private float swaySpeed = 2.0f;
    [SerializeField] private float swayAmount = 10.0f;
    [SerializeField] private float forwardSpeed = 2.0f;
    [SerializeField] private ParticleSystem jellyFishShoot;
    private Vector3 startPosition;
 

    [Header("Enterance settings")]
    [SerializeField] private Transform startDestination;
    [SerializeField] private float transitionToStartPositionDuration;

    private bool bossEnteredScene = false;



    protected async override void Start()
    {
        base.Start();
        startDestination.parent = null;
        await EnterAnimation();
    }

    private void Update()
    {
        if (bossEnteredScene)
        {
            ApplySwimAnimation();
        }
    }


    private async Task EnterAnimation()
    {
        await LerpTo(startDestination.position, transitionToStartPositionDuration);
        jellyFishShoot.Play();

    }

    private async Task LerpTo(Vector3 destination, float duration)
    {
        float time = 0f;
        float percentage = 0f;
        Vector3 startPos = transform.position;
        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPos, destination, percentage);
            float deltaTime = Time.deltaTime;
            time += deltaTime;
            percentage = time / duration;
            await Task.Delay(TimeSpan.FromSeconds(deltaTime));
        }
        bossEnteredScene = true;
    }

    private void ApplySwimAnimation()
    {
        float sway = Mathf.Sin(Time.time * swaySpeed) * swayAmount;
        transform.rotation = Quaternion.Euler(0, 0, sway);

        float swimY = Mathf.Sin(Time.time * swaySpeed * 0.5f) * 0.5f;
        Vector3 destination = new Vector3(transform.position.x, startPosition.y + swimY, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime);
        transform.Translate(forwardSpeed * Time.deltaTime * Vector3.up);
    }

    public override void Die()
    {
        base.Die();
        jellyFishShoot.Stop();
    }


}
