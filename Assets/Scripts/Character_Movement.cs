using System;
using System.Threading.Tasks;
using UnityEngine;

public class Character_Movement : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Movement")]
    [SerializeField] private float thrustForce = 15.0f;
    [SerializeField] private float tiltAngle = 15.0f;
    [SerializeField] private float tiltSmoothness = 3.0f;
    [SerializeField] private float floatStrength = 0.5f;




    private Camera mainCamera;

    private float baseY;
    private Vector3 _prevPos;
    private InputHandler _inputHandler;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        baseY = transform.position.y; 

        mainCamera = Camera.main;
        _prevPos = transform.position;
        _inputHandler = GetComponent<InputHandler>();
    }

    void Update()
    {
        bool isPressing = _inputHandler.IsPressing();

        if (isPressing)
        {
            Vector3 targetPos = _inputHandler.GetInputPosition();
            MovePlayer(targetPos);
        }
        ApplyTilt();
        ApplyFloatingEffect();
        ClampPosition();
        _prevPos = transform.position;
    }

    private void MovePlayer(Vector3 screenSpacePlayerDestination)
    {
        Vector3 worldPlayerDestination = mainCamera.ScreenToWorldPoint(screenSpacePlayerDestination);
        worldPlayerDestination.y = transform.position.y;
        worldPlayerDestination.z = 0f;
        transform.position = worldPlayerDestination;
    }

    private void ApplyTilt()
    {
        float tiltDirection = (transform.position - _prevPos).x;
        float targetTiltZ = Mathf.Clamp(Time.time * tiltDirection / thrustForce * -tiltAngle, 
            -tiltAngle, tiltAngle);
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetTiltZ);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * tiltSmoothness);
    }

    private void ApplyFloatingEffect()
    {
        float newY = baseY + Mathf.Sin(Time.time * floatStrength) * 0.1f;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    private void ClampPosition()
    {
        float screenHalfWidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
        float shipWidth = GetComponent<SpriteRenderer>().bounds.extents.x;

        float leftBoundary = -screenHalfWidth + shipWidth;
        float rightBoundary = screenHalfWidth - shipWidth;

        if (transform.position.x > rightBoundary)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            transform.position = new Vector3(rightBoundary, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < leftBoundary)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            transform.position = new Vector3(leftBoundary, transform.position.y, transform.position.z);
        }
    }


}
