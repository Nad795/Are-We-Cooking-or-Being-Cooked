using System;
using System.Collections.Generic;
using UnityEngine;
public class UnityMainThreadDispatcher : MonoBehaviour
{
    public static UnityMainThreadDispatcher Instance { get; private set; }

    private readonly Queue<Action> _queue = new Queue<Action>();
    private readonly object _lock = new object();

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        // Jalankan semua action yang sudah di-queue di main thread
        lock (_lock)
        {
            while (_queue.Count > 0)
            {
                _queue.Dequeue().Invoke();
            }
        }
    }

    /// <summary>
    /// Enqueue action untuk dijalankan di main thread pada frame berikutnya.
    /// Aman dipanggil dari thread manapun.
    /// </summary>
    public void Enqueue(Action action)
    {
        if (action == null) return;
        lock (_lock)
        {
            _queue.Enqueue(action);
        }
    }
}