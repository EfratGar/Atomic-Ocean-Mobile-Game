using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

public class SharkMovement : MonoBehaviour
{
    [SerializeField] private float swaySpeed = 2.0f; 
    [SerializeField] private float swayAmount = 10.0f; 
    [SerializeField] private float forwardSpeed = 2.0f;


    private Vector3 startPosition;
    private bool _shouldMove;
    

    void Start()
    {
        enabled = false;
        Shark shark = GetComponent<Shark>();
        shark.OnAttackPlayer += () => _shouldMove = false;
        shark.OnNavAgentBasedMovementEnded += OnStartMovingBackUp;
        shark.OnReady += StartMoving;
    }

    void Update()
    {
        ApplySwimAnimation();
    }


    private void ApplySwimAnimation()
    {
        float sway = Mathf.Sin(Time.time * swaySpeed) * swayAmount;
        Quaternion targetRotation = Quaternion.Euler(0, 0, sway);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime);
        if (_shouldMove)
        {
            float swimY = Mathf.Sin(Time.time * swaySpeed * 0.5f) * 0.5f;
            Vector3 destination = new(transform.position.x, startPosition.y + swimY, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime);
        }
    }

    private void StartMoving()
    {
        startPosition = transform.position;
        enabled = true;
        _shouldMove = true;
    }

    private void OnStartMovingBackUp()
    {
        startPosition = transform.position;
        _shouldMove = true;
    }

}
