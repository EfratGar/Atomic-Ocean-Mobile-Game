using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightRaysRotation : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private float rotationAngle = 30f;
    [SerializeField] private float rotationSpeed = 30f;

    [Header("Scale Pulsing Settings")]
    [SerializeField] private float scaleSpeed = 2f; 
    [SerializeField] private float minScale = 0.8f; 
    [SerializeField] private float maxScale = 1.2f; 

    private float startRotation;

    private void Start()
    {
        startRotation = transform.rotation.eulerAngles.z;
    }

    private void Update()
    {
        float angle = Mathf.PingPong(Time.time * rotationSpeed, rotationAngle * 2) - rotationAngle;
        transform.rotation = Quaternion.Euler(0f, 0f, startRotation + angle);

        // Scale Pulsing
        float scale = Mathf.Lerp(minScale, maxScale, Mathf.PingPong(Time.time * scaleSpeed, 1));
        transform.localScale = new Vector3(scale, scale, 1f);
    }
}
