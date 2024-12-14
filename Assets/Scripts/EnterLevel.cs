using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EnterLevel : MonoBehaviour
{
    [Header("Enterance settings")]
    [SerializeField] private Transform startDestination;
    [SerializeField] private float transitionToStartPositionDuration;

    public event Action OnEnteredScene = delegate { };

    // Start is called before the first frame update
    async void Start()
    {
        startDestination.parent = null;
        await EnterAnimation();
        OnEnteredScene();
    }

    private async Task EnterAnimation()
    {
        await LerpTo(startDestination.position, transitionToStartPositionDuration);
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
    }
}
