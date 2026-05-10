using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject obstaclePrefab;

    [Header("Spawn Settings")]
    public float spawnRate = 2f;

    [Header("Gap Settings")]
    public float gapSize = 3.5f;

    [Header("Top Pipe Limits")]
    public float topMinY = 2f;
    public float topMaxY = 6f;

    [Header("Bottom Pipe Limits")]
    public float bottomMinY = -6f;
    public float bottomMaxY = -2f;

    void Start()
    {
        InvokeRepeating(
            nameof(SpawnObstacle),
            1f,
            spawnRate
        );
    }

    void SpawnObstacle()
    {
        GameObject obstacle =
            Instantiate(
                obstaclePrefab,
                transform.position,
                Quaternion.identity
            );

        Transform topPipe =
            obstacle.transform.Find("TopPipe");

        Transform bottomPipe =
            obstacle.transform.Find("BottomPipe");

        Transform scoreZone =
            obstacle.transform.Find("ScoreZone");

        if(topPipe == null ||
           bottomPipe == null)
        {
            Debug.LogError(
                "TopPipe / BottomPipe tidak ditemukan!"
            );

            return;
        }

        // Random posisi top pipe
        float topY =
            Random.Range(topMinY, topMaxY);

        // Bottom otomatis mengikuti gap
        float bottomY =
            topY - gapSize;

        // Clamp supaya bottom tetap aman
        bottomY =
            Mathf.Clamp(
                bottomY,
                bottomMinY,
                bottomMaxY
            );

        // Set posisi
        topPipe.localPosition =
            new Vector3(
                0,
                topY,
                0
            );

        bottomPipe.localPosition =
            new Vector3(
                0,
                bottomY,
                0
            );

        // Score zone di tengah gap
        if(scoreZone != null)
        {
            float centerY =
                (topY + bottomY) / 2f;

            scoreZone.localPosition =
                new Vector3(
                    0,
                    centerY,
                    0
                );
        }
    }
}