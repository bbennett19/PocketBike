using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPSLocationUpdateConsumer : MonoBehaviour {

    public event Action<GPSLocation> OnLocationUpdate;
#if !UNITY_EDITOR
    public AndroidGPSServiceCallback callback;
#endif
    private Queue<GPSLocation> _updateQueue = new Queue<GPSLocation>();
    private object lockObj = new object();
	
	void Start () {
#if !UNITY_EDITOR
        callback.OnUpdateLocation += UpdateLocation;
#endif
	}

    private void Awake()
    {
#if !UNITY_EDITOR
        callback = new AndroidGPSServiceCallback();
#endif
    }

    // This function may or may not be called on the main unity thread. This callback is called from the Android GPS service thread
    public void UpdateLocation(double lat, double lon, double speed)
    {
        lock (lockObj)
        {
            _updateQueue.Enqueue(new GPSLocation(lat, lon, speed));
        }
    }

	// This will ensure that the Location Update event is called on the main unity thread
	void Update ()
    {
        lock (lockObj)
        {
            if (_updateQueue.Count != 0)
            {
                OnLocationUpdate(_updateQueue.Dequeue());
            }
        }
	}
}
