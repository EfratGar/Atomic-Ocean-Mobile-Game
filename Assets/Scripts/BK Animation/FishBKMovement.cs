using UnityEngine;

public class FishBKMovement : MonoBehaviour
{
    [SerializeField] private float floatAmplitude = 0.5f; 
    [SerializeField] private float floatFrequency = 1.0f;
    [SerializeField] private float moveSpeed = 2.0f;

    private Vector3 startPosition;
    private Vector2 moveDirection;

    void Start()
    {
        startPosition = transform.position;
        moveDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
    }

    void Update()
    {
        float verticalOffset = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, startPosition.y + verticalOffset, transform.position.z);
    }
}
