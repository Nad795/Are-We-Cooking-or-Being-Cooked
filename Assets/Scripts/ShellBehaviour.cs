using UnityEngine;
using UnityEngine.UI;

public class ShellBehaviour : MonoBehaviour
{
    [Header("Visual States")]
    [SerializeField] private Color _normalColor = Color.white;
    [SerializeField] private Color _activeColor = Color.yellow;
    [SerializeField] private Color _targetColor = Color.green;

    [SerializeField] private Image _shellImage;

    private void Awake()
    {
        if(_shellImage == null)
        {
            _shellImage = GetComponent<Image>();
        }
    }

    public void SetAsActive(bool isActive)
    {
        if(_shellImage != null)
        {
            _shellImage.color = isActive ? _activeColor : _normalColor;
        }
    }

    public void SetAsTarget(bool isTarget)
    {
        if(_shellImage != null)
        {
            _shellImage.color = isTarget ? _targetColor : _normalColor;
        }
    }

    public void PlayLandAnimation()
    {
        StartCoroutine(BounceAnimation());
    }
 
    private System.Collections.IEnumerator BounceAnimation()
    {
        Vector3 originalScale = transform.localScale;
        Vector3 squishScale = new Vector3(originalScale.x * 1.3f, originalScale.y * 0.7f, 1f);

        float elapsed = 0f;
        float duration = 0.15f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(originalScale, squishScale, elapsed / duration);
            yield return null;
        }

        elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(squishScale, originalScale, elapsed / duration);
            yield return null;
        }

        transform.localScale = originalScale;
    }
}