using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuffFish : Monster
{
    [SerializeField] private float horizontalOffset;
    [SerializeField] private Monster smallPuffFishPrefab;
    [SerializeField] private GameObject explosionPrefab;

    public override void Die()
    {
        Vector3 basePosition = transform.position;

        // Log to debug the position
        Debug.Log($"PuffFish died at position: {basePosition}");

        // Instantiate the explosion effect at the position of the PuffFish
        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, basePosition, Quaternion.identity);

            // Optional: Destroy explosion after its effect is done
            Destroy(explosion, 2.0f); // Adjust time to match the particle system duration
        }

        // Create small PuffFish
        Vector3 offset = Vector3.right * horizontalOffset;
        Instantiate(smallPuffFishPrefab, basePosition + offset, Quaternion.identity);
        Instantiate(smallPuffFishPrefab, basePosition - offset, Quaternion.identity);

        base.Die();
    }





}
