using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public static ObstacleSpawner Instance { get; private set; }

    [Header("References")]
    [SerializeField] private RectTransform _canvasRect;
    [SerializeField] private GameObject _obstaclePairPrefab;
    [SerializeField] private BirdController _bird;

    [Header("Settings")]
    [SerializeField] private float _obstacleSpeed = 300f;
    [SerializeField] private float _gapSize = 250f;
    [SerializeField] private float _spawnInterval = 2f;
    [SerializeField] private float _gapYRange = 150f;
    [SerializeField] private float difficultyRate = 0.02f;

    private List<RectTransform> _activeObstacles = new List<RectTransform>();
    private float _spawnTimer = 0f;
    private float _currentDifficulty = 0f;
    private bool _isRunning = false;

    public System.Action OnObstaclePassed;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void StartSpawning()
    {
        _isRunning = true;
        _spawnTimer = _spawnInterval * 0.5f;
    }

    public void StopSpawning()
    {
        _isRunning = false;
    }

    private void Update()
    {
        if(!_isRunning) return;

        MoveObstacles();

        _spawnTimer -= Time.deltaTime;
        if(_spawnTimer <= 0f)
        {
            SpawnPair();
            _spawnTimer = Mathf.Max(1f, _spawnInterval - _currentDifficulty * 0.5f);
        }

        CleanUpObstacles();
        CheckCollisions();
    }

    private void SpawnPair()
    {
        float canvasHeight = _canvasRect.rect.height;
        float canvasWidth = _canvasRect.rect.width;
        float currentGap = Mathf.Max(150f, _gapSize - _currentDifficulty * 30f);
        float gapCenterY = Random.Range(-_gapYRange, _gapYRange);

        GameObject pair = Instantiate(_obstaclePairPrefab, transform);
        RectTransform rt = pair.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(canvasWidth / 2f + 100f, gapCenterY);

        var behaviour = pair.GetComponent<ObstaclePairBehaviour>();
        if(behaviour != null)        {
            behaviour.Initialize(currentGap, canvasHeight);
            behaviour.OnPassed = () => OnObstaclePassed?.Invoke();
        }

        _activeObstacles.Add(rt);
        _currentDifficulty += difficultyRate;
    }

    private void MoveObstacles()
    {
        float speed = _obstacleSpeed + _currentDifficulty * 50f;
        foreach(var obs in _activeObstacles)
        {
            if(obs != null)
            {
                obs.anchoredPosition = new Vector2(obs.anchoredPosition.x - speed * Time.deltaTime, obs.anchoredPosition.y);
            }
        }
    }

    private void CleanUpObstacles()
    {
        float leftBound = -_canvasRect.rect.width / 2f - 200f;
        var toRemove = new List<RectTransform>();

        foreach(var obs in _activeObstacles)
        {
            if(obs != null && obs.anchoredPosition.x < leftBound)
            {
                toRemove.Add(obs);
            }
        }

        foreach(var obs in toRemove)
        {
            _activeObstacles.Remove(obs);
            Destroy(obs.gameObject);
        }
    }

    private void CheckCollisions()
    {
        if(_bird == null) return;
        Rect birdRect = GetWorldRect(_bird.GetRect());

        foreach(var obsPair in _activeObstacles)
        {
            if(obsPair == null) continue;
            var behaviour = obsPair.GetComponent<ObstaclePairBehaviour>();
            if(behaviour != null && behaviour.CheckCollision(birdRect))
            {
                _bird.Die();
                StopSpawning();
                return;
            }
        }
    }

    private Rect GetWorldRect(RectTransform rt)
    {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);
        return new Rect(corners[0].x, corners[0].y, (corners[2].x - corners[0].x), (corners[2].y - corners[0].y));
    }
}