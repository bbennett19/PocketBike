using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPSLocationUpdateConsumer : MonoBehaviour {

    public event Action<GPSLocation> OnLocationUpdate;
    public AndroidGPSServiceCallback callback;

    private Queue<GPSLocation> _updateQueue = new Queue<GPSLocation>();
    private object lockObj = new object();
	
	void Start () {
        callback.OnUpdateLocation += UpdateLocation;
	}

    private void Awake()
    {
        callback = new AndroidGPSServiceCallback();
    }

    // This function may or may not be called on the main unity thread. This callback is called from the Android GPS service thread
    public void UpdateLocation(double lat, double lon)
    {
        lock (lockObj)
        {
            _updateQueue.Enqueue(new GPSLocation(lat, lon));
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
