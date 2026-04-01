using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpriteController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private JumpInputHandler _jumpInputHandler;
    [SerializeField] private RectTransform _spriteRect;

    [Header("Movement Settings")]
    [SerializeField] private float _jumpDuration = 0.3f;
    [SerializeField] private float _jumpArcHeight = 80f;
    [SerializeField] private AnimationCurve _jumpCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Offset")]
    [SerializeField] private float _spriteYOffset = 60f;

    private bool _isJumping = false;
    private RectTransform _targetShell;

    private void Start()
    {
        if(_jumpInputHandler != null)
        {
            _jumpInputHandler.OnJumpLeft += HandleJumpLeft;
            _jumpInputHandler.OnJumpRight += HandleJumpRight;
        }

        if(ShellSpawner.Instance != null)
        {
            ShellSpawner.Instance.OnNextShellSpawned += OnNextShellSpawned;
        }

        PlaceCurrentShell();
    }

    private void OnDestroy()
    {
        if(_jumpInputHandler != null)
        {
            _jumpInputHandler.OnJumpLeft -= HandleJumpLeft;
            _jumpInputHandler.OnJumpRight -= HandleJumpRight;
        }

        if(ShellSpawner.Instance != null)
        {
            ShellSpawner.Instance.OnNextShellSpawned -= OnNextShellSpawned;
        }
    }

    private void OnNextShellSpawned(RectTransform nextShell)
    {
        _targetShell = nextShell;
    }

    private void HandleJumpLeft()
    {
        if(!CanJump()) return;

        if(_targetShell != null)
        {
            float targetX = _targetShell.anchoredPosition.x;
            float spriteX = _spriteRect.anchoredPosition.x;

            if(targetX <= spriteX + 50f)
            {
                JumpToTarget();
            }
            else
            {
                Debug.Log("[Sprite] Wrong direction! Target is to the RIGHT!");
            }
        }
    }

    private void HandleJumpRight()
    {
        if(!CanJump()) return;

        if(_targetShell != null)
        {
            float targetX = _targetShell.anchoredPosition.x;
            float spriteX = _spriteRect.anchoredPosition.x;

            if(targetX >= spriteX - 50f)
            {
                JumpToTarget();
            }
            else
            {
                Debug.Log("[Sprite] Wrong direction! Target is to the LEFT!");
            }
        }
    }

    private bool CanJump()
    {
        if(_isJumping) return false;
        if(ShellSpawner.Instance != null && ShellSpawner.Instance.IsScrolling) return false;
        if(ShellJumpManager.Instance != null && !ShellJumpManager.Instance.IsPlaying) return false;
        return true;
    }

    private void JumpToTarget()
    {
        if(_targetShell == null) return;

        Vector2 targetPos = _targetShell.anchoredPosition + Vector2.up * _spriteYOffset;
        StartCoroutine(JumpAnimation(_spriteRect.anchoredPosition, targetPos));
    }

    private IEnumerator JumpAnimation(Vector2 startPos, Vector2 endPos)
    {
        _isJumping = true;
        float elapsed = 0f;

        while(elapsed < _jumpDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / _jumpDuration);
            float curveT = _jumpCurve.Evaluate(t);

            Vector2 currentPos = Vector2.Lerp(startPos, endPos, curveT);
            float arcOffset = Mathf.Sin(t * Mathf.PI) * _jumpArcHeight;
            currentPos.y += arcOffset;

            _spriteRect.anchoredPosition = currentPos;
            yield return null;
        }

        _spriteRect.anchoredPosition = endPos;
        _targetShell.GetComponent<ShellBehaviour>()?.PlayLandAnimation();
        _isJumping = false;
        ShellSpawner.Instance?.OnPlayerReachedNextShell();
    }

    private void PlaceCurrentShell()
    {
        var currentShell = ShellSpawner.Instance?.GetCurrentShell();
        if(currentShell != null && _spriteRect != null)
        {
            _spriteRect.anchoredPosition = currentShell.anchoredPosition + Vector2.up * _spriteYOffset;
        }
    }
}