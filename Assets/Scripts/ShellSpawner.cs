using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShellSpawner : MonoBehaviour
{
    public static ShellSpawner Instance { get; private set;}

    [Header("References")]
    [SerializeField] private GameObject _shellPrefab;
    [SerializeField] private RectTransform _canvasRect;

    [Header("Spawn Settings")]
    [SerializeField] private float _verticalSpacing = 200f;
    [SerializeField] private float _horizontalMargin = 150f;
    [SerializeField] private float _scrollSpeed = 300f;
    [SerializeField] private float _difficultyIncreaseRate = 0.05f;

    [Header("Difficulty")]
    [SerializeField] private float _minHorizontalDistance = 100f;
    private float _currentDifficulty = 0f;

    private List<RectTransform> _activeShells = new List<RectTransform>();
    private RectTransform _currentShell;
    private RectTransform _nextShell;
    private bool _isScrolling = false;

    public System.Action<RectTransform> OnNextShellSpawned;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        SpawnInitialShells();
    }

    private void SpawnInitialShells()
    {
        float startY = _canvasRect.rect.height * 0.3F;
        float startX = 0f;

        _currentShell = SpawnShell(startX, startY);
        _currentShell.GetComponent<ShellBehaviour>().SetAsActive(true);

        SpawnNextShell();
    }

    private void SpawnNextShell()
    {
        if(_currentShell == null) return;

        float currentX = _currentShell.anchoredPosition.x;
        float currentY = _currentShell.anchoredPosition.y;

        float halfWidth = _canvasRect.rect.width * 0.5f - _horizontalMargin;

        float newX;
        float minDist = _minHorizontalDistance + (_currentDifficulty * 50f);

        int attempts = 0;
        do
        {
            newX = Random.Range(-halfWidth, halfWidth);
            attempts++;
        } while (Mathf.Abs(newX - currentX) < minDist && attempts < 20);

        float newY = currentY + _verticalSpacing;

        _nextShell = SpawnShell(newX, newY);
        _nextShell.GetComponent<ShellBehaviour>().SetAsTarget(true);

        OnNextShellSpawned?.Invoke(_nextShell);
    }

    private RectTransform SpawnShell(float x, float y)
    {
        GameObject shellObj = Instantiate(_shellPrefab, transform);
        RectTransform shellRect = shellObj.GetComponent<RectTransform>();
        shellRect.anchoredPosition = new Vector2(x, y);
        _activeShells.Add(shellRect);
        return shellRect;
    }

    public void OnPlayerReachedNextShell()
    {
        if(_isScrolling) return;

        _currentDifficulty += _difficultyIncreaseRate;

        if(_currentShell != null)
        {
            _currentShell.GetComponent<ShellBehaviour>().SetAsActive(false);
        }

        _currentShell = _nextShell;
        _currentShell.GetComponent<ShellBehaviour>().SetAsActive(true);
        _currentShell.GetComponent<ShellBehaviour>().SetAsTarget(false);

        ShellJumpManager.Instance?.AddScore(1);

        StartCoroutine(ScrollAllObjects());
    }

    private IEnumerator ScrollAllObjects()
    {
        _isScrolling = true;

        float targetY = -_canvasRect.rect.height * 0.3f;
        float currentShellY = _currentShell.anchoredPosition.y;
        float scrollAmount = currentShellY - targetY;

        float elapsed = 0f;
        float duration = scrollAmount / _scrollSpeed;

        Dictionary<RectTransform, float> startPositions = new Dictionary<RectTransform, float>();
        foreach(var shell in _activeShells)
        {
            if(shell != null)
            {
                startPositions[shell] = shell.anchoredPosition.y;
            }
        }

        while(elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            foreach(var shell in _activeShells)
            {
                if(shell != null && startPositions.ContainsKey(shell))
                {
                    float newY = startPositions[shell] - (scrollAmount * smoothT);
                    shell.anchoredPosition = new Vector2(shell.anchoredPosition.x, newY);
                }
            }

            yield return null;
        }

        CleanupOffscreenShells();

        SpawnNextShell();

        _isScrolling = false;
    }

    private void CleanupOffscreenShells()
    {
        float bottomY = -_canvasRect.rect.height * 0.6f;
        List<RectTransform> toRemove = new List<RectTransform>();

        foreach(var shell in _activeShells)
        {
            if(shell != null && shell.anchoredPosition.y < bottomY && shell != _currentShell)
            {
                toRemove.Add(shell);
            }
        }

        foreach(var shell in toRemove)
        {
            _activeShells.Remove(shell);
            Destroy(shell.gameObject);
        }
    }

    public RectTransform GetCurrentShell() => _currentShell;
    public RectTransform GetNextShell() => _nextShell;
    public bool IsScrolling => _isScrolling;
}