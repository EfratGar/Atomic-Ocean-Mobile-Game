using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private bool _isPressing;
    private Vector3 _inputPosition;

    void Update()
    {
        _isPressing = false;
        HandleInput();
    }

    private void HandleInput()
    {
#if UNITY_EDITOR
        // Mouse Input
        if (Input.GetMouseButton(0))
        {
            _inputPosition = Input.mousePosition;
            _isPressing = true;
        }
#else
        // Mobile Input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            _inputPosition = touch.position;
            _isPressing = true;
        }
#endif
    }

    public bool IsPressing()
    {
        return _isPressing;
    }

    public Vector3 GetInputPosition()
    {
        return _inputPosition;
    }
}
