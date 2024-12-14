using System;
using System.Threading.Tasks;
using UnityEngine;

public class SharkMovement : MonoBehaviour
{
    [SerializeField] private float swaySpeed = 2.0f; 
    [SerializeField] private float swayAmount = 10.0f; 
    [SerializeField] private float forwardSpeed = 2.0f;


    private Vector3 startPosition;
    private float _timer;
    private bool _shouldMove;
    

    void Start()
    {
        startPosition = transform.position;
        Shark shark = GetComponent<Shark>();
        shark.OnAttackPlayer += () => _shouldMove = false;
        shark.OnNavAgentBasedMovementEnded += OnStartMoving;
        _timer = 0f;
        _shouldMove = true;
    }

    void Update()
    {
        ApplySwimAnimation();
    }


    private void ApplySwimAnimation()
    {
        float sway = Mathf.Sin(Time.time * swaySpeed) * swayAmount;
        transform.rotation = Quaternion.Euler(0, 0, sway);
        if (_shouldMove)
        {
            float swimY = Mathf.Sin(_timer * swaySpeed * 0.5f) * 0.5f;
            transform.position = new Vector3(transform.position.x, startPosition.y + swimY, transform.position.z);

            transform.Translate(forwardSpeed * Time.deltaTime * Vector3.down);
            _timer += Time.deltaTime;
        }


    }

    private void OnStartMoving()
    {
        startPosition = transform.position;
        _timer = 0f;
        _shouldMove = true;
    }

}
