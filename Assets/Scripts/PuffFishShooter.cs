using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PuffFishShooter : MonoBehaviour
{
    [SerializeField] GameObject spikeBullet;
    //[SerializeField] private Transform spikeFirePoint;
    [SerializeField] private float maxFireRate;
    [SerializeField] private float minFireRate;

    private float shootingTime;

    private bool _canShoot;

    private void Start()
    {
        shootingTime = UnityEngine.Random.Range(minFireRate, maxFireRate);
        ShootCooldown();
    }

    private void Update()
    {
        if (_canShoot)
            ShootSpikes();
    }

    private void ShootSpikes()
    {
        Instantiate(spikeBullet, transform.position, Quaternion.identity);
        _canShoot = false;
        ShootCooldown();
    }

    private async void ShootCooldown()
    {
        await Task.Delay((int)(shootingTime * 1000));
        _canShoot = true;
    }



}
