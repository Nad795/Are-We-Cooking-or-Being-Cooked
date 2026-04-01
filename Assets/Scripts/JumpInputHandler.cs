using System.Collections.Generic;
using UnityEngine;
using mptcc = Mediapipe.Tasks.Components.Containers;

public class JumpInputHandler : MonoBehaviour
{
    [Header("Detection Thresholds")]
    [SerializeField] private float _jumpThreshold = 0.08f;
    [SerializeField] private float _leanThreshold = 0.04f;
    [SerializeField] private float _jumpCooldown  = 0.5f;

    [Header("Mirror")]
    [Tooltip("Aktifkan jika webcam mirror (default true)")]
    [SerializeField] private bool _mirrorX = true;

    [Header("Debug (Editor Only)")]
    [SerializeField] private bool _useKeyboardFallback = true;

    private const int LEFT_SHOULDER  = 11;
    private const int RIGHT_SHOULDER = 12;
    private const int LEFT_HIP       = 23;
    private const int RIGHT_HIP      = 24;
    private const int LEFT_ANKLE     = 27;
    private const int RIGHT_ANKLE    = 28;

    private float _prevHipY     = -1f;
    private float _lastJumpTime = -999f;

    public System.Action OnJumpLeft;
    public System.Action OnJumpRight;

    private IReadOnlyList<mptcc.NormalizedLandmark> _currentLandmarks;

    private void Update()
    {
        if (_useKeyboardFallback) { HandleKeyboardInput(); return; }
        if (_currentLandmarks == null || _currentLandmarks.Count < 33) return;
        ProcessPoseLandmarks(_currentLandmarks);
    }

    public void UpdateLandmarks(IReadOnlyList<mptcc.NormalizedLandmark> landmarks)
    {
        _currentLandmarks = landmarks;
    }

    public void ProcessPoseLandmarks(IReadOnlyList<mptcc.NormalizedLandmark> landmarks)
    {
        float hipY = (landmarks[LEFT_HIP].y + landmarks[RIGHT_HIP].y) / 2f;

        if (_prevHipY < 0f) { _prevHipY = hipY; return; }

        float hipDelta = _prevHipY - hipY;
        bool isJumping = hipDelta > _jumpThreshold;

        if (isJumping && Time.time - _lastJumpTime > _jumpCooldown)
        {
            float shoulderDiff = landmarks[RIGHT_SHOULDER].y - landmarks[LEFT_SHOULDER].y;

            if (shoulderDiff > _leanThreshold)
            {
                // Bahu kanan lebih rendah dari bahu kiri
                // Normal  : condong kiri → lompat kiri
                // Mirror  : condong kanan → lompat kanan
                TriggerJump(_mirrorX ? true : false);
            }
            else if (shoulderDiff < -_leanThreshold)
            {
                // Bahu kiri lebih rendah dari bahu kanan
                // Normal  : condong kanan → lompat kanan
                // Mirror  : condong kiri → lompat kiri
                TriggerJump(_mirrorX ? false : true);
            }
            else
            {
                // Fallback: posisi ankle
                float leftAnkleX  = landmarks[LEFT_ANKLE].x;
                float rightAnkleX = landmarks[RIGHT_ANKLE].x;
                // Normal  : rightAnkleX > leftAnkleX → lompat kanan
                // Mirror  : rightAnkleX > leftAnkleX → lompat kiri
                bool goRight = rightAnkleX > leftAnkleX;
                TriggerJump(_mirrorX ? !goRight : goRight);
            }
        }

        _prevHipY = hipY;
    }

    private void TriggerJump(bool goRight)
    {
        _lastJumpTime = Time.time;
        if (goRight)
        {
            Debug.Log("[JumpInput] Jump RIGHT detected");
            OnJumpRight?.Invoke();
        }
        else
        {
            Debug.Log("[JumpInput] Jump LEFT detected");
            OnJumpLeft?.Invoke();
        }
    }

    private void HandleKeyboardInput()
    {
        if (Time.time - _lastJumpTime < _jumpCooldown) return;
        if (Input.GetKeyDown(KeyCode.LeftArrow)  || Input.GetKeyDown(KeyCode.A)) TriggerJump(false);
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) TriggerJump(true);
    }
}