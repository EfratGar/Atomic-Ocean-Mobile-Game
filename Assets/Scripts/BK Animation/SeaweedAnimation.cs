using UnityEngine;

public class SeaweedAnimation : MonoBehaviour
{
    [SerializeField] private float swayAmount = 5f;      
    [SerializeField] private float swaySpeed = 2f;       
    [SerializeField] private float smoothTime = 0.5f;    

    private float currentRotation;
    private float velocity; 

    void Update()
    {
        float targetRotation = Mathf.Sin(Time.time * swaySpeed) * swayAmount;

        currentRotation = Mathf.SmoothDamp(currentRotation, targetRotation, ref velocity, smoothTime);

        transform.rotation = Quaternion.Euler(0, 0, currentRotation);
    }
}
