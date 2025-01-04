using UnityEngine;

public class Bullet : Spike
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Crab"))
        {
            Vector2 normal = (Vector2)transform.position - collision.ClosestPoint(transform.position);
            normal.Normalize();
            OnBounceBullet(normal);
        }
        else base.OnTriggerEnter2D(collision);
    }

    private void OnBounceBullet(Vector2 downDirection)
    {
        float direction = Mathf.Sign(downDirection.x);
        Vector2 reflectedDirection = Vector2.Reflect(direction * transform.right , downDirection);
        rb.velocity = reflectedDirection * rb.velocity.magnitude;
    }
}
