using System;
using UnityEngine;

public class SharkMovement : MonoBehaviour
{
    [SerializeField] private float swaySpeed = 2.0f;
    [SerializeField] private float swayAmount = 10.0f;
    [SerializeField] private float forwardSpeed = 2.0f;

    [SerializeField] private Transform rightTail;
    [SerializeField] private float tailSwaySpeed = 2.0f;
    [SerializeField] private float tailSwayAmount = 15.0f;

    [SerializeField] private Transform leftFin;
    [SerializeField] private Transform rightFin;
    [SerializeField] private float finSwaySpeed = 3.0f;
    [SerializeField] private float finSwayAmount = 15.0f;

    private Vector3 startPosition;
    private bool _shouldMove;

    void Start()
    {
        enabled = false;
        Shark shark = GetComponent<Shark>();
        shark.OnAttackPlayer += () => _shouldMove = false;
        shark.OnReturnedToOriginalPosition += ReturnedToOriginalPosition;
        shark.OnReady += StartMoving;

        startPosition = transform.position;
    }

    void Update()
    {
        ApplySwimAnimation();
        AnimateTail();
        AnimateFins();
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

    private void AnimateTail()
    {
        if (rightTail != null)
        {
            float tailSway = Mathf.Sin(Time.time * tailSwaySpeed) * tailSwayAmount;
            float swayY = Mathf.Sin(Time.time * tailSwaySpeed * 0.5f) * 5.0f; 
            rightTail.localRotation = Quaternion.Euler(swayY, 0, tailSway);
            Debug.Log($"Tail Rotation: {rightTail.localRotation.eulerAngles}");
        }
        else
        {
            Debug.LogError("RightTail is not assigned in the Inspector!");
        }
    }


    private void AnimateFins()
    {
        if (leftFin != null)
        {
            float leftSway = Mathf.Sin(Time.time * finSwaySpeed) * finSwayAmount;
            leftFin.localRotation = Quaternion.Euler(0, 0, leftSway);
        }

        if (rightFin != null)
        {
            float rightSway = Mathf.Sin(Time.time * finSwaySpeed) * finSwayAmount;
            rightFin.localRotation = Quaternion.Euler(0, 0, -rightSway);
        }
    }

    private void StartMoving()
    {
        startPosition = transform.position;
        enabled = true;
        _shouldMove = true;
    }

    private void ReturnedToOriginalPosition()
    {
        _shouldMove = true;
    }
}
