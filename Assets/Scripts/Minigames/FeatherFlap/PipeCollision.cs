using UnityEngine;

public class PipeCollision : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("PipeCollision: " + gameObject.name + " kena " + other.gameObject.name + " (tag: " + other.tag + ")");

        if (other.CompareTag("Player"))
        {
            FeatherFlapManager.Instance.GameOver();
        }
    }
}
