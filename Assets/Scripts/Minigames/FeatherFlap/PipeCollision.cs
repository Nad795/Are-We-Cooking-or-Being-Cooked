using UnityEngine;

public class PipeCollision : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            FeatherFlapManager.Instance.GameOver();
        }
    }
}
