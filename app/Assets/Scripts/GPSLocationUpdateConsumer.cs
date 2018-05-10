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
    private Queue<Vector2> _updates = new Queue<Vector2>();
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
        _updates.Enqueue(new Vector2((float)lat, (float)lon));
        if (_updates.Count == 5)
        {
            Vector2 sum = new Vector2();
            Vector2 start = _updates.Dequeue();
            Vector2 end = new Vector2();
            Vector2 vec = new Vector2();

            for (int i = 0; i < 4; i++)
            {
                end = _updates.Dequeue();
                vec = (end - start).normalized;
                sum += vec;
            }

            sum = (sum/4f).normalized;

            if (Vector2.Dot(sum, vec) > 0f)
            {
                float angle = Mathf.Acos(Vector2.Dot(vec, sum));
                float dist = (end - start).magnitude * Mathf.Cos(angle);
                Vector2 loc = start + (sum * dist);
                _updates.Enqueue(loc);
                lock (lockObj)
                {
                    _updateQueue.Enqueue(new GPSLocation(loc.x, loc.y, speed));
                }
            }
            else
            {
                _updates.Enqueue(start);
            }
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
