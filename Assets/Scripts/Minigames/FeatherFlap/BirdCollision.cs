using UnityEngine;

public class BirdCollision : MonoBehaviour
{
    bool canCollide;

    void Start()
    {
        Invoke(nameof(EnableCollision), 1f);
    }

    void EnableCollision()
    {
        canCollide = true;
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
