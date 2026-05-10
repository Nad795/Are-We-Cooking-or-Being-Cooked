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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(!canCollide) return;

        Debug.Log(
            "Nabrak: " + collision.gameObject.name
        );

        FeatherFlapManager.Instance.GameOver();
    }
}