using System.Collections;
using UnityEngine;

public class EggSpawner : MonoBehaviour
{
    public static EggSpawner Instance { get; private set; }

    [Header("References")]
    [SerializeField] private GameObject _eggPrefab;
    [SerializeField] private RectTransform _canvasRect;

    [Header("Spawn Settings")]
    [SerializeField] private float _spawnXOffset = 300f;
    [SerializeField] private float _spawnYRange = 150f;
    [SerializeField] private float _eggLifetime = 3f;
    [SerializeField] private float _difficultyIncreaseRate = 0.05f;

    private GameObject _activeEgg = null;
    private bool _isLeft = false;
    private bool _lastWasLeft = false;
    private float _currentDifficulty = 0f;

    public System.Action<bool> OnEggSpawned; // Parameter indicates if it's a left (true) or right (false) egg
    public System.Action OnEggExpired;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SpawnNextEgg()
    {
        if(_activeEgg != null) Destroy(_activeEgg);

        float sameChance = 0.1f + _currentDifficulty * 0.2f;
        _isLeft = Random.value > sameChance ? !_lastWasLeft : _lastWasLeft;

        _lastWasLeft = _isLeft;

        float x = _isLeft ? -_spawnXOffset : _spawnXOffset;
        float y = Random.Range(-_spawnYRange, _spawnYRange);

        _activeEgg = Instantiate(_eggPrefab, transform);
        _activeEgg.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);

        float currentLifetime = Mathf.Max(1.2f, _eggLifetime - _currentDifficulty * 0.5f);
        _activeEgg.GetComponent<EggBehaviour>().Initialize(_isLeft, currentLifetime, OnEggTimeOut);

        _currentDifficulty += _difficultyIncreaseRate;
        OnEggSpawned?.Invoke(_isLeft);
        Debug.Log($"[EggSpawner] Spawned {( _isLeft ? "Left" : "Right")}");
    }

    private void OnEggTimeOut()
    {
        _activeEgg = null;
        OnEggExpired?.Invoke();
        SpawnNextEgg();
    }

    public bool TryCollect(bool playerShookLeft)
    {
        if(_activeEgg == null) return false;

        if(playerShookLeft != _isLeft) return false;

        var behaviour = _activeEgg.GetComponent<EggBehaviour>();
        if(behaviour != null)
        {
            behaviour.PlayCollectAnimation(() => {
                Destroy(_activeEgg);
                _activeEgg = null;
                SpawnNextEgg();
            });
        }
        else
        {
            Destroy(_activeEgg);
            _activeEgg = null;
            SpawnNextEgg();
        }
        return true;
    }

    public bool IsEggOnLeft() => _isLeft;
    public bool HasEggActive() => _activeEgg != null;
}