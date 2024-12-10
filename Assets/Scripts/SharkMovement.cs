using UnityEngine;

public class SharkMovement : MonoBehaviour
{
    [SerializeField] private float swaySpeed = 2.0f; 
    [SerializeField] private float swayAmount = 10.0f; 
    [SerializeField] private float forwardSpeed = 2.0f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position; 
    }

    void Update()
    {
        ApplySwimAnimation();
    }

    private void ApplySwimAnimation()
    {
        float sway = Mathf.Sin(Time.time * swaySpeed) * swayAmount;
        transform.rotation = Quaternion.Euler(0, 0, sway); 

        float swimY = Mathf.Sin(Time.time * swaySpeed * 0.5f) * 0.5f; 
        transform.position = new Vector3(transform.position.x, startPosition.y + swimY, transform.position.z);

        transform.Translate(Vector3.up * forwardSpeed * Time.deltaTime);
    }
}
