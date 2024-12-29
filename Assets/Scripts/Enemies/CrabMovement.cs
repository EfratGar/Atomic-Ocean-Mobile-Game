using UnityEngine;

public class CrabMovement : MonoBehaviour
{    
    [SerializeField] private float moveRange = 3.0f;     
    [SerializeField] private float moveFrequency = 2.0f;
    private float _moveSpeed;

    private bool _shouldEscape;

    private Vector3 startPosition;
    private float _creationTime;
    private float _timer;

    private void Awake()
    {
        EnterLevel enterLevel = GetComponent<EnterLevel>();
        enterLevel.OnEnteredScene += StartMoving;

        LevelManager levelManager = FindFirstObjectByType<LevelManager>();
        levelManager.LevelCompleted += () => _shouldEscape = true;
        levelManager.MoveToActiveScene(gameObject);

        startPosition = transform.position;
        _creationTime = Time.time;

        enabled = false;
    }

    void Update()
    {
        MoveSideToSide();
    }

    private void MoveSideToSide()
    {
        if (!_shouldEscape)
        {
            float offsetX = Mathf.Sin(_timer * moveFrequency * Mathf.Deg2Rad) * moveRange;
            Vector3 targetPos = startPosition + new Vector3(offsetX, 0, 0);
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * _moveSpeed);
            _timer += Time.deltaTime;
        }
        else Escape();
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void StartMoving()
    {
        _moveSpeed = (transform.position - startPosition).magnitude / (Time.time - _creationTime);
        startPosition = transform.position;
        enabled = true;
    }

    private void Escape()
    {
        transform.Translate(_moveSpeed * Time.deltaTime * Vector3.right);
    }

}
