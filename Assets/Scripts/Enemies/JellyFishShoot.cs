using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyFishShoot : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        if(other.CompareTag("Player"))
        {
            PlayableCharacter player = other.GetComponent<PlayableCharacter>();
            if(player != null)
            {
                player.OnGotHit(15);
            }
        }
    }



}
