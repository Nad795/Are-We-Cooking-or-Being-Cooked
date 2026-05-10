using UnityEngine;

public class ScoreZone : MonoBehaviour
{
    bool scored;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(scored) return;

        if(collision.CompareTag("Player"))
        {
            scored = true;

            FeatherFlapManager.Instance.AddScore();
        }
    }
}