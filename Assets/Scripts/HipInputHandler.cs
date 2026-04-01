using System.Collections.Generic;
using UnityEngine;
using mptcc = Mediapipe.Tasks.Components.Containers;
 
public class HipInputHandler : MonoBehaviour
{
    [Header("Detection Thresholds")]
    [SerializeField] private float _hipShakeThreshold = 0.06f;
    [SerializeField] private float _shakeCooldown     = 0.4f;
 
    [Header("Mirror")]
    [Tooltip("Aktifkan jika webcam mirror (default true)")]
    [SerializeField] private bool _mirrorX = true;
 
    [Header("Debug")]
    [SerializeField] private bool _useKeyboardFallback = true;
 
    private const int LEFT_HIP  = 23;
    private const int RIGHT_HIP = 24;
 
    private float _hipBaselineX       = -1f;
    private int   _baselineFrameCount = 0;
    private const int BASELINE_FRAMES = 30;
    private float _baselineAccum      = 0f;
    private float _lastShakeTime      = -999f;
 
    private IReadOnlyList<mptcc.NormalizedLandmark> _currentLandmarks;
 
    public System.Action OnShakeLeft;
    public System.Action OnShakeRight;
 
    private void Update()
    {
        if (_useKeyboardFallback) { HandleKeyboard(); return; }
        if (_currentLandmarks == null || _currentLandmarks.Count < 33) return;
        ProcessHip(_currentLandmarks);
    }
 
    public void UpdateLandmarks(IReadOnlyList<mptcc.NormalizedLandmark> landmarks)
    {
        _currentLandmarks = landmarks;
    }
 
    public void UpdateLandmarks(mptcc.NormalizedLandmarks landmarks)
    {
        _currentLandmarks = landmarks.landmarks;
    }
 
    public void ResetBaseline()
    {
        _hipBaselineX       = -1f;
        _baselineFrameCount = 0;
        _baselineAccum      = 0f;
    }
 
    private void ProcessHip(IReadOnlyList<mptcc.NormalizedLandmark> lm)
    {
        float hipX = (lm[LEFT_HIP].x + lm[RIGHT_HIP].x) / 2f;
 
        // Kalibrasi baseline
        if (_baselineFrameCount < BASELINE_FRAMES)
        {
            _baselineAccum += hipX;
            _baselineFrameCount++;
            if (_baselineFrameCount == BASELINE_FRAMES)
            {
                _hipBaselineX = _baselineAccum / BASELINE_FRAMES;
                Debug.Log($"[HipInput] Baseline established at {_hipBaselineX}");
            }
            return;
        }
 
        float offset    = hipX - _hipBaselineX;
        bool onCooldown = Time.time - _lastShakeTime < _shakeCooldown;
        if (onCooldown) return;
 
        // MediaPipe X: 0 = kiri layar, 1 = kanan layar
        // offset positif = hip geser ke kanan layar
        // Mirror ON  : kanan layar = kiri PEMAIN  → OnShakeLeft
        // Mirror OFF : kanan layar = kanan PEMAIN → OnShakeRight
        if (offset > _hipShakeThreshold)
        {
            _lastShakeTime = Time.time;
            if (_mirrorX) { Debug.Log("[HipInput] Shake LEFT");  OnShakeLeft?.Invoke(); }
            else          { Debug.Log("[HipInput] Shake RIGHT"); OnShakeRight?.Invoke(); }
        }
        else if (offset < -_hipShakeThreshold)
        {
            _lastShakeTime = Time.time;
            if (_mirrorX) { Debug.Log("[HipInput] Shake RIGHT"); OnShakeRight?.Invoke(); }
            else          { Debug.Log("[HipInput] Shake LEFT");  OnShakeLeft?.Invoke(); }
        }
    }
 
    private void HandleKeyboard()
    {
        if (Time.time - _lastShakeTime < _shakeCooldown) return;
        if (Input.GetKeyDown(KeyCode.LeftArrow)  || Input.GetKeyDown(KeyCode.A))
        { _lastShakeTime = Time.time; OnShakeLeft?.Invoke(); }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        { _lastShakeTime = Time.time; OnShakeRight?.Invoke(); }
    }
}