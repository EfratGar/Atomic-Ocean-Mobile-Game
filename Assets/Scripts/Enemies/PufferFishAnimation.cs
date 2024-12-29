using UnityEngine;

public class PufferFishAnimation : MonoBehaviour
{
    [Header("Puff Animation Settings")]
    [SerializeField] private float inflateSpeed = 2.0f;
    [SerializeField] private float deflateSpeed = 2.0f;
    [SerializeField] private float maxScale = 1.2f;
    [SerializeField] private float minScale = 0.8f;

    [Header("Fin Animation Settings")]
    [SerializeField] private Transform leftFin;
    [SerializeField] private Transform rightFin;
    [SerializeField] private float finSwaySpeed = 3.0f;
    [SerializeField] private float finSwayAmount = 15.0f;

    [Header("Hover Animation Settings")]
    [SerializeField] private float hoverSpeed = 1.5f;
    [SerializeField] private float hoverAmount = 0.1f;

    private bool isInflating = true;
    private Vector3 startScale;

    private float hoverOffsetY = 0f;

    void Start()
    {
        startScale = transform.localScale;
    }

    void Update()
    {
        AnimateBody();
        AnimateFins();
        AnimateHover();
    }

    private void AnimateBody()
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

    private void AnimateHover()
    {
        // Calculate the hover offset on Y
        hoverOffsetY = Mathf.Sin(Time.time * hoverSpeed) * hoverAmount;

        // Add the hover offset to the current position
        transform.position += new Vector3(0, hoverOffsetY, 0) * Time.deltaTime;
    }
}
