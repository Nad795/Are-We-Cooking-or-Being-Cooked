using System.Collections;
using UnityEngine;

public class BirdController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WingsInputHandler _wingsInput;
    [SerializeField] private RectTransform _birdRect;
    [SerializeField] private RectTransform _canvasRect;

    [Header("Physics")]
    [SerializeField] private float _flapForce = 400f;
    [SerializeField] private float _gravity = 600f;
    [SerializeField] private float _maxFallSpeed = 700f;
    [SerializeField] private float _maxRiseSpeed = 500f;

    [Header("Screen Bounds")]
    [SerializeField] private float _topMargin = 80f;
    [SerializeField] private float _bottomMargin = 80f;

    [Header("Rotation")]
    [SerializeField] private float _maxRotationUp = -30f;
    [SerializeField] private float _maxRotationDown = 60f;
    [SerializeField] private float _rotationSpeed = 8f;

    private float _velocityY = 0f;
    private bool _isDead = false;
    private bool _isPlaying = false;

    public System.Action OnDeath;

    private void Start()
    {
        if(_wingsInput != null) _wingsInput.OnFlap += Flap;
    }

    private void OnDestroy()
    {
        if(_wingsInput != null) _wingsInput.OnFlap -= Flap;
    }

    public void StartPlaying()
    {
        _isPlaying = true;
        _isDead = false;
        _velocityY = 0f;
    }

    private void Update()
    {
        if(!_isPlaying || _isDead) return;

        _velocityY -= _gravity * Time.deltaTime;
        _velocityY = Mathf.Clamp(_velocityY, -_maxFallSpeed, _maxRiseSpeed);

        Vector2 pos = _birdRect.anchoredPosition;
        pos.y += _velocityY * Time.deltaTime;

        float halfHeight = _birdRect.rect.height / 2f;
        float topBound = halfHeight - _topMargin;
        float bottomBound = -halfHeight + _bottomMargin;

        if(pos.y >= topBound) {pos.y = topBound; _velocityY = 0f;}
        if(pos.y <= bottomBound) {pos.y = bottomBound; Die();}
        _birdRect.anchoredPosition = pos;

        float targetZ = Mathf.Lerp(_maxRotationDown, _maxRotationUp, Mathf.InverseLerp(-_maxFallSpeed, _maxRiseSpeed, _velocityY));
        float currentZ = _birdRect.localEulerAngles.z;
        if(currentZ > 180f) currentZ -= 360f;
        _birdRect.localEulerAngles = new Vector3(0f, 0f, Mathf.Lerp(currentZ, targetZ, Time.deltaTime * _rotationSpeed));
    }

    private void Flap()
    {
        if(!_isPlaying || _isDead) return;
        _velocityY = _flapForce;
    }

    public void Die()
    {
        if(_isDead) return;
        _isDead = true;
        _isPlaying = false;
        StartCoroutine(DeathAnimation());
    }

    private IEnumerator DeathAnimation()
    {
        float elapsed = 0f;
        while(elapsed < 0.5f)
        {
            elapsed += Time.deltaTime;
            _velocityY -= _gravity * Time.deltaTime;
            Vector2 pos = _birdRect.anchoredPosition;
            pos.y += _velocityY * Time.deltaTime;
            _birdRect.anchoredPosition = pos;
            _birdRect.Rotate(0f, 0f, -10f);
            yield return null;
        }
        OnDeath?.Invoke();
    }

    public RectTransform GetRect() => _birdRect;
}