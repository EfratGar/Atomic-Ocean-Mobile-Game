using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 0.5f;

    [Header("Double Shooting")]
    [SerializeField] private float horizontalOffset;

    private InputHandler _inputHandler;
    private bool _canShoot;
    private bool collectedPresent;


    void Start()
    {
        _canShoot = true;
        _inputHandler = GetComponent<InputHandler>();
        Present.OnCollected += OnCollectedPresent;
    }


    void Update()
    {
        if (_inputHandler.IsPressing() && _canShoot)
        {
            if (!collectedPresent)
                Shoot();
            else
                DoubleShoot();

            _canShoot = false;
            ShootCooldown();
            
        }
    }

    private void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
    }

    private async void ShootCooldown()
    {
        await Task.Delay(TimeSpan.FromSeconds(fireRate));
        _canShoot = true;
    }

    private void DoubleShoot()
    {
        Vector3 basePosition = firePoint.position;
        Vector3 offset = Vector3.right * horizontalOffset;
        Instantiate(bulletPrefab, basePosition + offset, Quaternion.identity);
        Instantiate(bulletPrefab, basePosition - offset, Quaternion.identity);
    }

    private void OnCollectedPresent()
    {
        collectedPresent = true;
    }
}
