using UnityEngine;

public class JellyFish : Monster
{
    [Header("Animation settings")]
    [SerializeField] private float swaySpeed = 2.0f;
    [SerializeField] private float swayAmount = 10.0f;
    [SerializeField] private float forwardSpeed = 2.0f;
    [SerializeField] private ParticleSystem jellyFishShoot;
    private Vector3 startPosition;


    private void Update()
    {
        ApplySwimAnimation();
    }

    protected override void OnEnteredScene()
    {
        base.OnEnteredScene();
        startPosition = transform.position;
        jellyFishShoot.Play();
    }

    private void ApplySwimAnimation()
    {
        float sway = Mathf.Sin(Time.time * swaySpeed) * swayAmount;
        transform.rotation = Quaternion.Euler(0, 0, sway);

        float swimY = Mathf.Sin(Time.time * swaySpeed * 0.5f) * 0.5f;
        Vector3 destination = new(transform.position.x, startPosition.y + swimY, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime);
    }

    public override void Die()
    {
        base.Die();
        jellyFishShoot.Stop();
    }
}
