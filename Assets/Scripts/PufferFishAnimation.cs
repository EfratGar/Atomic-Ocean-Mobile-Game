using UnityEngine;

public class PufferFishAnimation : MonoBehaviour
{
    [Header("Puff Animation Settings")]
    [SerializeField] private float inflateSpeed = 2.0f;   
    [SerializeField] private float deflateSpeed = 2.0f;   
    [SerializeField] private float maxScale = 1.2f;       
    [SerializeField] private float minScale = 0.8f;       

    private bool isInflating = true; 
    private Vector3 startScale; 

    void Start()
    {
        startScale = transform.localScale; 
    }

    void Update()
    {

        if (isInflating)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, startScale * maxScale, Time.deltaTime * inflateSpeed);

            if (transform.localScale.x >= startScale.x * (maxScale - 0.05f))
                isInflating = false;
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, startScale * minScale, Time.deltaTime * deflateSpeed);

            if (transform.localScale.x <= startScale.x * (minScale + 0.05f))
                isInflating = true;
        }
    }
}
