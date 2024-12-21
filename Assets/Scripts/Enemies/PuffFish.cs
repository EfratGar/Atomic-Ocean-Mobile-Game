using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuffFish : Monster
{
    [SerializeField] private float horizontalOffset;
    [SerializeField] private Monster smallPuffFishPrefab;

    public override void Die()
    {
        Vector3 basePosition = transform.position;
        Vector3 offset = Vector3.right * horizontalOffset;
        Instantiate(smallPuffFishPrefab, basePosition + offset, Quaternion.identity);
        Instantiate(smallPuffFishPrefab, basePosition - offset, Quaternion.identity);
        base.Die();

    }




}
