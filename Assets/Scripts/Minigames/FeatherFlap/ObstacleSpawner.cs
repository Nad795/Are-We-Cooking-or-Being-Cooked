using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject obstaclePrefab;

    [Header("Spawn Settings")]
    public float spawnRate = 2f;

    [Header("Gap Settings")]
    public float gapSize = 3.5f;

    [Header("Center Range")]
    public float minCenterY = -1f;

    public float maxCenterY = 3f;

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

        float centerY =
            Random.Range(
                minCenterY,
                maxCenterY
            );

        topPipe.localPosition =
            new Vector3(
                0,
                centerY + gapSize / 2f,
                0
            );

        bottomPipe.localPosition =
            new Vector3(
                0,
                centerY - gapSize / 2f,
                0
            );

        if(scoreZone != null)
        {
            scoreZone.localPosition =
                new Vector3(
                    0,
                    centerY,
                    0
                );
        }
    }
}