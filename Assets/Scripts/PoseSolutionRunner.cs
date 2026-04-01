using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Mediapipe.Tasks.Vision.PoseLandmarker;
using Mediapipe.Tasks.Core;
using Mediapipe.Tasks.Vision.Core;
 
using mptcc = Mediapipe.Tasks.Components.Containers;
public class PoseSolutionRunner : MonoBehaviour
{
    [Header("MediaPipe Settings")]
    [Tooltip("Nama file .task di dalam StreamingAssets folder")]
    [SerializeField] private string _modelFileName = "pose_landmarker_lite.task";
    [SerializeField] private int    _numPoses      = 1;
    [SerializeField] private float  _minPoseDetectionConfidence = 0.5f;
    [SerializeField] private float  _minPosePresenceConfidence  = 0.5f;
    [SerializeField] private float  _minTrackingConfidence      = 0.5f;
 
    [Header("Webcam Settings")]
    [SerializeField] private int      _webcamWidth  = 640;
    [SerializeField] private int      _webcamHeight = 480;
    [SerializeField] private int      _webcamFPS    = 30;
    [SerializeField] private RawImage _webcamDisplay;
 
    [Header("References")]
    [SerializeField] private MediapipeBridge _bridge;
 
    private WebCamTexture  _webcamTexture;
    private PoseLandmarker _poseLandmarker;
    private Texture2D      _inputTexture;
    private bool           _isRunning = false;
 
    private void Start()
    {
        StartCoroutine(InitializeAsync());
    }
 
    private void OnDestroy()
    {
        if (_webcamTexture != null && _webcamTexture.isPlaying)
            _webcamTexture.Stop();
 
        _poseLandmarker?.Close();
    }
 
    private IEnumerator InitializeAsync()
    {
        yield return StartWebcam();
        if (_webcamTexture == null || !_webcamTexture.isPlaying) yield break;
 
        yield return InitializeMediaPipe();
        _isRunning = true;
        Debug.Log("[PoseSolutionRunner] Ready!");
    }
 
    private IEnumerator StartWebcam()
    {
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
 
        if (!Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            Debug.LogError("[PoseSolutionRunner] Webcam permission denied!");
            yield break;
        }
 
        var devices = WebCamTexture.devices;
        if (devices.Length == 0)
        {
            Debug.LogError("[PoseSolutionRunner] Tidak ada webcam ditemukan!");
            yield break;
        }
 
        _webcamTexture = new WebCamTexture(devices[0].name, _webcamWidth, _webcamHeight, _webcamFPS);
        _webcamTexture.Play();
 
        if (_webcamDisplay != null)
            _webcamDisplay.texture = _webcamTexture;
 
        _inputTexture = new Texture2D(_webcamWidth, _webcamHeight, TextureFormat.RGBA32, false);
 
        yield return new WaitUntil(() => _webcamTexture.didUpdateThisFrame);
        Debug.Log("[PoseSolutionRunner] Webcam started.");
    }
 
    private IEnumerator InitializeMediaPipe()
    {
        // Load model dari StreamingAssets
        string modelPath = System.IO.Path.Combine(Application.streamingAssetsPath, _modelFileName);
 
        var baseOptions = new BaseOptions(modelAssetPath: modelPath);
 
        var options = new PoseLandmarkerOptions(
            baseOptions:                   baseOptions,
            runningMode:                   RunningMode.LIVE_STREAM,
            numPoses:                      _numPoses,
            minPoseDetectionConfidence:    _minPoseDetectionConfidence,
            minPosePresenceConfidence:     _minPosePresenceConfidence,
            minTrackingConfidence:         _minTrackingConfidence,
            resultCallback:  OnPoseResult
        );
 
        _poseLandmarker = PoseLandmarker.CreateFromOptions(options);
 
        yield return null;
        Debug.Log("[PoseSolutionRunner] PoseLandmarker initialized.");
    }
 
    private void Update()
    {
        if (!_isRunning || _webcamTexture == null || !_webcamTexture.didUpdateThisFrame)
            return;
 
        _inputTexture.SetPixels32(_webcamTexture.GetPixels32());
        _inputTexture.Apply();
 
        // Timestamp dalam microseconds
        long timestampUs = (long)(Time.realtimeSinceStartup * 1_000_000);
 
        using var mpImage = new Mediapipe.Image(
            Mediapipe.ImageFormat.Types.Format.Srgba,
            _inputTexture
        );
 
        // LIVE_STREAM: hasil dikembalikan via callback OnPoseResult
        _poseLandmarker.DetectAsync(mpImage, timestampUs);
    }
 
    // Callback dari MediaPipe — bisa dipanggil dari thread berbeda
    private void OnPoseResult(PoseLandmarkerResult result, Mediapipe.Image image, long timestampMillisec)
    {
        // Dispatch ke main thread
        UnityMainThreadDispatcher.Instance?.Enqueue(() =>
        {
            if (result.poseLandmarks == null || result.poseLandmarks.Count == 0)
                return;
 
            // Ambil pose pertama (single player)
            _bridge?.OnLandmarksReceived(result.poseLandmarks[0]);
        });
    }
}