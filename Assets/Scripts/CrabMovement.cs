using UnityEngine;

public class CrabMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3.0f;     
    [SerializeField] private float moveRange = 2.0f;     
    [SerializeField] private float moveFrequency = 2.0f; 

    private Vector3 startPosition;
    private Vector3 originalScale;

    void Start()
    {
        startPosition = transform.position;
        originalScale = transform.localScale;
    }

    void Update()
    {
        MoveSideToSide();
    }

    private void MoveSideToSide()
    {
        float offsetX = Mathf.Sin(Time.time * moveFrequency) * moveRange;

        transform.position = startPosition + new Vector3(offsetX, 0, 0);

        if (offsetX > 0)
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
        else
            transform.localScale = originalScale;
    }
}
