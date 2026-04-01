using System.Collections;
using UnityEngine;

public class ObstaclePairBehaviour : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform _topObstacle;
    [SerializeField] private RectTransform _bottomObstacle;

    private bool _passed = false;

    public System.Action OnPassed;

    public void Initialize(float gapSize, float canvasHeight)
    {
        float halfGap = gapSize / 2f;
        float halfCanvas = canvasHeight / 2f;

        if(_topObstacle != null)
        {
            float h = halfCanvas - halfGap;
            _topObstacle.sizeDelta = new Vector2(_topObstacle.sizeDelta.x, h);
            _topObstacle.anchoredPosition = new Vector2(0f, halfGap + h / 2f);
        }

        if(_bottomObstacle != null)
        {
            float h = halfCanvas - halfGap;
            _bottomObstacle.sizeDelta = new Vector2(_bottomObstacle.sizeDelta.x, h);
            _bottomObstacle.anchoredPosition = new Vector2(0f, -halfGap - h / 2f);
        }
    }

    private void Update()
    {
        if(_passed) return;

        RectTransform rt = GetComponent<RectTransform>();
        if(rt != null && rt.anchoredPosition.x < -50f)
        {
            _passed = true;
            OnPassed?.Invoke();
        }
    }

    public bool CheckCollision(Rect birdRect)
    {
        if(_topObstacle != null && RectOverlaps(birdRect, GetWorldRect(_topObstacle))) return true;
        if(_bottomObstacle != null && RectOverlaps(birdRect, GetWorldRect(_bottomObstacle))) return true;
        return false;
    }

    private Rect GetWorldRect(RectTransform rt)
    {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);
        return new Rect(corners[0].x, corners[0].y, (corners[2].x - corners[0].x), (corners[2].y - corners[0].y));
    }

    private bool RectOverlaps(Rect a, Rect b)
    {
        return a.xMin < b.xMax && a.xMax > b.xMin && a.yMin < b.yMax && a.yMax > b.yMin;
    }
}