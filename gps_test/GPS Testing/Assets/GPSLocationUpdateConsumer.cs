using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GPSLocationUpdateConsumer : MonoBehaviour {
    public Text timerText;
    public bool stopAfterTime = false;
    public float collectionLength = 1800;
    private bool collect = true;
    private bool timerStart = false;
    private float elapsed = 0f;
    public static event Action<GPSLocation> OnLocationUpdate;
    public AndroidGPSServiceCallback callback;

    private Queue<GPSLocation> _updateQueue = new Queue<GPSLocation>();
    private Queue<Vector2> _updates = new Queue<Vector2>();
    private object lockObj = new object();
    private AndroidJavaClass _serviceLauncher;
    private bool _gpsServiceActive = false;

    void Start () {
        
        if (Input.location.isEnabledByUser)
        {
            callback = new AndroidGPSServiceCallback();
            callback.OnUpdateLocation += UpdateLocation;
            // Get the unity android activity
            AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
            _serviceLauncher = new AndroidJavaClass("com.pocketbike.gpstrackingservice.ServiceLauncher");
            _serviceLauncher.CallStatic("setActivityInstance", activity);
            _gpsServiceActive = _serviceLauncher.CallStatic<bool>("startService", callback);
        }
	}

    // This function may or may not be called on the main unity thread. This callback is called from the Android GPS service thread
    public void UpdateLocation(double lat, double lon, double speed)
    {
        lock (lockObj)
        {
            _updateQueue.Enqueue(new GPSLocation(lat, lon, speed));
        }
        return;
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

    private void OnApplicationQuit()
    {
        _serviceLauncher.CallStatic("stopService");
    }

    // This will ensure that the Location Update event is called on the main unity thread
    void Update ()
    {
        if(timerStart)
        {
            elapsed += Time.deltaTime;
            timerText.text = elapsed.ToString("0.00");

            if(stopAfterTime && elapsed >= collectionLength)
            {
                collect = false;
                timerStart = false;
            }
        }
        lock (lockObj)
        {
            if (_updateQueue.Count != 0 && collect)
            {
                timerStart = true;
                OnLocationUpdate(_updateQueue.Dequeue());
            }
        }
	}
}
