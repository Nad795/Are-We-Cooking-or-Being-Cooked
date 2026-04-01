using System.Collections.Generic;
using UnityEngine;
using mptcc = Mediapipe.Tasks.Components.Containers;

public class MediapipeBridge : MonoBehaviour
{
    [Header("Level 1 - Shell Jump")]
    [SerializeField] private JumpInputHandler _jumpInputHandler;

    [Header("Level 2 - Egg Beat")]
    [SerializeField] private HipInputHandler _hipInputHandler;

    [Header("Level 3 - Feather Flap")]
    [SerializeField] private WingsInputHandler _wingsInputHandler;

    [Header("Settings")]
    [Tooltip("Mirror X landmark karena webcam default mirror")]
    [SerializeField] private bool _mirrorX = true;

    public void OnLandmarksReceived(mptcc.NormalizedLandmarks landmarks)
    {
        if(landmarks.landmarks == null || landmarks.landmarks.Count < 33)
        {
            Debug.LogWarning("Landmarks data is incomplete or null.");
            return;
        }

        _jumpInputHandler?.UpdateLandmarks(landmarks.landmarks);
        _hipInputHandler?.UpdateLandmarks(landmarks.landmarks);
        _wingsInputHandler?.UpdateLandmarks(landmarks.landmarks);
    }
}
