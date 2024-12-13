using UnityEngine;

public class BubbleFloatBKAnimation : MonoBehaviour
{ 
    [SerializeField] private float floatSpeed = 1.0f;       
    [SerializeField] private float sideMovementRange = 0.5f; 
    [SerializeField] private float sideSpeed = 1.0f;       
    [SerializeField] private float resetHeight = 10.0f;    
    [SerializeField] private float startY = -6.0f;         

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float newX = startPosition.x + Mathf.Sin(Time.time * sideSpeed) * sideMovementRange;
        float newY = transform.position.y + floatSpeed * Time.deltaTime;

        transform.position = new Vector3(newX, newY, transform.position.z);

        if (transform.position.y > resetHeight)
        {
            transform.position = new Vector3(startPosition.x, startY, startPosition.z);
        }
    }
}
