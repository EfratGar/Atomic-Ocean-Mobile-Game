using UnityEngine;

public class SeaWeedAnimation : MonoBehaviour
{
    [Header("Breathing Animation Settings")]
    [SerializeField] private float scaleAmplitude = 0.05f; 
    [SerializeField] private float scaleSpeed = 2.0f;

    private Vector3 startScale;
    private Vector3 startPosition;

    void Start()
    {
        startScale = transform.localScale; 
        startPosition = transform.position; 
    }

    void Update()
    {
        float scaleOffset = Mathf.Sin(Time.time * scaleSpeed) * scaleAmplitude;
        transform.localScale = new Vector3(startScale.x, startScale.y + scaleOffset, startScale.z);

        transform.position = new Vector3(startPosition.x, startPosition.y, startPosition.z);

    }
}
