using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EggBehaviour: MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image _eggImage;
    [SerializeField] private Image _timerRing;

    [Header("Colours")]
    [SerializeField] private Color _leftColor = new Color(0.2f, 0.6f, 1f);
    [SerializeField] private Color _rightColor = new Color(1f, 0.4f, 0.2f);
    [SerializeField] private Color _urgentColor = Color.red;

    private float _lifetime;
    private float _elapsed;
    private bool _isLeft;
    private System.Action _onTimeout;
    private bool _isActive = false;

    private void Awake()
    {
        if(_eggImage == null) _eggImage = GetComponent<Image>();
    }

    public void Initialize(bool isLeft, float lifetime, System.Action onTimeout)
    {
        _isLeft = isLeft;
        _lifetime = lifetime;
        _onTimeout = onTimeout;
        _elapsed = 0f;
        _isActive = true;

        if(_eggImage != null)
        {
            _eggImage.color = isLeft ? _leftColor : _rightColor;
        }

        if(_timerRing != null)
        {
            _timerRing.color = isLeft ? _leftColor : _rightColor;
            _timerRing.fillAmount = 1f;
        }

        StartCoroutine(SpawnAnimation());
    }

    private void Update()
    {
        if(!_isActive) return;

        _elapsed += Time.deltaTime;
        float remaining = 1f - (_elapsed / _lifetime);

        if(_timerRing != null) _timerRing.fillAmount = remaining;

        if(remaining < 0.3f && _eggImage != null)
        {
            _eggImage.color = Color.Lerp(_urgentColor, _isLeft ? _leftColor : _rightColor, remaining / 0.3f);
        }

        if(_elapsed >= _lifetime)
        {
            _isActive = false;
            _onTimeout?.Invoke();
        }
    }

    public void PlayCollectAnimation(System.Action onComplete)
    {
        _isActive = false;
        StartCoroutine(CollectAnimation(onComplete));
    }

    private IEnumerator SpawnAnimation()
    {
        transform.localScale = Vector3.zero;
        float elapsed = 0f;
        while(elapsed < 0.2f)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, Mathf.SmoothStep(0f, 1f, elapsed / 0.2f));
            yield return null;
        }
        transform.localScale = Vector3.one;
    }

    private IEnumerator CollectAnimation(System.Action onComplete)
    {
        float elapsed = 0f;
        float duration = 0.25f;
        Vector3 startScale = transform.localScale;
        Color startColor = _eggImage != null ? _eggImage.color : Color.white;

        while(elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            transform.localScale = Vector3.Lerp(startScale, startScale * 1.5f, t);
            if(_eggImage != null) _eggImage.color = new Color(startColor.r, startColor.g, startColor.b, 1f - t);
            yield return null;
        }
        onComplete?.Invoke();
    }
}