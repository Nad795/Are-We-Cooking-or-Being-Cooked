using UnityEngine;

public class ObstacleMove : MonoBehaviour
{
    public float speed = 3f;

    void Update()
    {
        transform.Translate(
            Vector3.left *
            speed *
            Time.deltaTime
        );

        if(transform.position.x < -20f)
        {
            Destroy(gameObject);
        }
    }
}