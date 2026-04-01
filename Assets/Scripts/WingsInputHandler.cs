using System.Collections.Generic;
using UnityEngine;
using mptcc = Mediapipe.Tasks.Components.Containers;
 
public class WingsInputHandler : MonoBehaviour
{
    [Header("Detection Thresholds")]
    [SerializeField] private float _flapThreshold = 0.08f;
    [SerializeField] private float _flapCooldown  = 0.25f;
 
    [Header("Debug")]
    [SerializeField] private bool _useKeyboardFallback = true;
 
    private const int LEFT_SHOULDER  = 11;
    private const int RIGHT_SHOULDER = 12;
    private const int LEFT_WRIST     = 15;
    private const int RIGHT_WRIST    = 16;
 
    private float _lastFlapTime = -999f;
    private float _prevWristY   = -1f;
 
    private IReadOnlyList<mptcc.NormalizedLandmark> _currentLandmarks;
 
    public System.Action OnFlap;
    public float WingLiftValue { get; private set; }
 
    private void Update()
    {
        if (_useKeyboardFallback)
        {
            HandleKeyboard();
            return; // ← fix: jangan lanjut ke ProcessWings kalau keyboard mode
        }
 
        if (_currentLandmarks == null || _currentLandmarks.Count < 33) return;
 
        ProcessWings(_currentLandmarks);
    }
 
    public void UpdateLandmarks(IReadOnlyList<mptcc.NormalizedLandmark> landmarks)
    {
        _currentLandmarks = landmarks;
    }
 
    public void UpdateLandmarks(mptcc.NormalizedLandmarks landmarks)
    {
        _currentLandmarks = landmarks.landmarks;
    }
 
    private void ProcessWings(IReadOnlyList<mptcc.NormalizedLandmark> lm)
    {
        float shoulderY = (lm[LEFT_SHOULDER].y + lm[RIGHT_SHOULDER].y) / 2f;
        float wristY    = (lm[LEFT_WRIST].y    + lm[RIGHT_WRIST].y)    / 2f;
 
        // Y axis tidak terpengaruh mirror — deteksi kepak sama saja
        float relativeY = shoulderY - wristY;
        WingLiftValue = Mathf.Clamp01(relativeY / _flapThreshold);
 
        if (_prevWristY >= 0f)
        {
            float delta     = _prevWristY - wristY; // positif = tangan naik
            bool onCooldown = Time.time - _lastFlapTime < _flapCooldown;
 
            if (delta > _flapThreshold && !onCooldown)
            {
                Debug.Log("[WingsInput] FLAP!");
                OnFlap?.Invoke();
                _lastFlapTime = Time.time;
            }
        }
 
        _prevWristY = wristY;
    }
 
    private void HandleKeyboard()
    {
        WingLiftValue = (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow))
            ? 1f
            : Mathf.Max(0f, WingLiftValue - Time.deltaTime * 3f);
 
        if (Time.time - _lastFlapTime < _flapCooldown) return;
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            Debug.Log("[WingsInput] FLAP! (keyboard)");
            OnFlap?.Invoke();
            _lastFlapTime = Time.time;
        }
    }
}