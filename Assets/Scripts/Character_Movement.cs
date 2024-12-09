using UnityEngine;

public class Character_Movement : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float thrustForce = 15.0f;
    [SerializeField] private float maxSpeed = 15.0f;
    [SerializeField] private float quickTurnMultiplier = 2.0f;
    [SerializeField] private float tiltAngle = 15.0f;
    [SerializeField] private float tiltSmoothness = 3.0f;
    [SerializeField] private float floatStrength = 0.5f;

    // Shooting
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 0.5f;
    private float nextFireTime = 0f;


    private float baseY;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        baseY = transform.position.y; 
    }

    void Update()
    {
        HandleInput();
        ApplyTilt();
        ApplyFloatingEffect();
        ClampPosition();
    }

    private void ApplyThrust(Vector2 direction)
    {
        if (Mathf.Sign(rb.velocity.x) != Mathf.Sign(direction.x) || rb.velocity.x == 0)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            rb.AddForce(direction * thrustForce * quickTurnMultiplier, ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(direction * thrustForce, ForceMode2D.Force);
        }

        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
    }

    private void HandleInput()
    {
        // Mobile Input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
                Shoot();

            if (touch.position.x > Screen.width / 2)
                ApplyThrust(Vector2.right);
            else
                ApplyThrust(Vector2.left);
        }
        // Mouse Input
        else if (Input.GetMouseButtonDown(0))
        {
            Shoot();

            Vector3 mousePosition = Input.mousePosition;
            if (mousePosition.x > Screen.width / 2)
                ApplyThrust(Vector2.right);
            else
                ApplyThrust(Vector2.left);
        }
    }

    private void ApplyTilt()
    {
        float targetTiltZ = Mathf.Clamp(rb.velocity.x / thrustForce * -tiltAngle, -tiltAngle, tiltAngle);
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

    private void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
    }
}
