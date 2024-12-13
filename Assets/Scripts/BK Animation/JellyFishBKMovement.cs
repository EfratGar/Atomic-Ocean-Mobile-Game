using UnityEngine;

public class JellyFishBKMovement : MonoBehaviour
{
    [SerializeField] private float floatSpeedY = 1.0f; 
    [SerializeField] private float floatAmplitudeX = 0.5f; 
    [SerializeField] private float floatFrequencyX = 1.0f; 

    private Rigidbody2D rb;
    private Vector3 startPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.up * floatSpeedY; 
        }
        startPosition = transform.position;
    }

    void Update()
    {
        float offsetX = Mathf.Sin(Time.time * floatFrequencyX) * floatAmplitudeX;

        transform.position = new Vector3(startPosition.x + offsetX, transform.position.y, transform.position.z);
    }
}
