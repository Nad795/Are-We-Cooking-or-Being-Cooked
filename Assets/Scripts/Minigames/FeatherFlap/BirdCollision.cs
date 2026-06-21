using UnityEngine;

public class BirdCollision : MonoBehaviour
{
    bool canCollide;

    CircleCollider2D col;

    void Start()
    {
        col = GetComponent<CircleCollider2D>();

        Invoke(nameof(EnableCollision), 1f);
    }

    void EnableCollision()
    {
        canCollide = true;
    }

    void Update()
    {
        if (!canCollide)
            return;

        Vector2 center = (Vector2)transform.position + col.offset;

        Collider2D hit = Physics2D.OverlapCircle(center, col.radius);

        if (hit != null && hit.CompareTag("Obstacle"))
        {
            FeatherFlapManager.Instance.GameOver();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!canCollide)
            return;

        if (other.CompareTag("Obstacle"))
        {
            FeatherFlapManager.Instance.GameOver();
        }
    }
}
