using UnityEngine;

public class CrabMovement : MonoBehaviour
{    
    [SerializeField] private float moveRange = 2.0f;     
    [SerializeField] private float moveFrequency = 2.0f;

    private bool _shouldEscape;

    private Vector3 startPosition;

    private void Awake()
    {
        EnterLevel enterLevel = GetComponent<EnterLevel>();
        enterLevel.OnEnteredScene += StartMoving;
        LevelManager levelManager = FindFirstObjectByType<LevelManager>();
        levelManager.LevelCompleted += () => _shouldEscape = true;
        enabled = false;
    }

    void Update()
    {
        MoveSideToSide();
    }

    private void MoveSideToSide()
    {
        float offsetX = moveRange;
        if (!_shouldEscape)
            offsetX *= Mathf.Sin(Time.time * moveFrequency);

        Vector3 targetPos = startPosition + new Vector3(offsetX, 0, 0);
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void StartMoving()
    {
        startPosition = transform.position;
        enabled = true;
    }

}
