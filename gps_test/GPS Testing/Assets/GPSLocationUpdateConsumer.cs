using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GPSLocationUpdateConsumer : MonoBehaviour
{
    public Text timerText;
    public Text accAvgText;
    public Text accStdDevText;
    public Text accMinText;
    public Text accMaxText;
    public Text speedAvgText;
    public Text speedStdDevText;
    public Text speedMinText;
    public Text speedMaxText;
    public Text updateCountText;
    public Button calcButton;
    public Button stopBtn;

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
    private int updateCount = 0;
    private List<double> speedReadings = new List<double>();
    private List<float> accuracyReadings = new List<float>();

    void Start () {
        
        if (Input.location.isEnabledByUser)
        {
            calcButton.onClick.AddListener(CaculateAndDisplay);
            callback = new AndroidGPSServiceCallback();
            callback.OnUpdateLocation += UpdateLocation;
            // Get the unity android activity
            AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
            _serviceLauncher = new AndroidJavaClass("com.pocketbike.gpstrackingservice.ServiceLauncher");
            _serviceLauncher.CallStatic("setActivityInstance", activity);
            _gpsServiceActive = _serviceLauncher.CallStatic<bool>("startService", callback);
        }
	}

    public void StartStop()
    {
        timerStart = !timerStart;
        collect = !collect;
    }

    // This function may or may not be called on the main unity thread. This callback is called from the Android GPS service thread
    public void UpdateLocation(double lat, double lon, double speed, float accuracy)
    {
        if (collect)
        {
            lock (lockObj)
            {
                speedReadings.Add(speed);
                accuracyReadings.Add(accuracy);
                updateCount++;
                _updateQueue.Enqueue(new GPSLocation(lat, lon, speed));
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
            stopBtn.interactable = true;
            elapsed += Time.deltaTime;
            timerText.text = "Timer: " + elapsed.ToString("0.00");

            if(stopAfterTime && elapsed >= collectionLength)
            {
                collect = false;
                timerStart = false;
            }
        }
        lock (lockObj)
        {
            if (_updateQueue.Count != 0)
            {
                timerStart = true;
                OnLocationUpdate(_updateQueue.Dequeue());
            }
        }
	}

    public void CaculateAndDisplay()
    {
        if(speedReadings.Count > 0)
        {
            float accuracySum = 0.0f;
            float accuracyMin = float.MaxValue;
            float accuracyMax = 0f;

            float speedSum = 0.0f;
            float speedMin = float.MaxValue;
            float speedMax = 0f;

            for(int i = 0; i < speedReadings.Count; i++)
            {
                accuracyMin = Math.Min(accuracyMin, accuracyReadings[i]);
                accuracyMax = Math.Max(accuracyMax, accuracyReadings[i]);
                accuracySum += accuracyReadings[i];

                speedMin = (float)Math.Min(speedMin, speedReadings[i]);
                speedMax = (float)Math.Max(speedMax, speedReadings[i]);
                speedSum += (float)speedReadings[i];
            }

            float accuracyAvg = accuracySum / accuracyReadings.Count;
            double speedAvg = speedSum / speedReadings.Count;

            double accuracyStdDev = 0.0f;
            double speedStdDev = 0.0;

            for(int i = 0; i < speedReadings.Count; i++)
            {
                accuracyStdDev += Math.Pow(accuracyReadings[i] - accuracyAvg, 2);
                speedStdDev += Math.Pow(speedReadings[i] - speedAvg, 2);
            }

            accuracyStdDev = Math.Sqrt(accuracyStdDev / accuracyReadings.Count);
            speedStdDev = Math.Sqrt(speedStdDev / speedReadings.Count);

            updateCountText.text = "Count: " + speedReadings.Count.ToString();

            accAvgText.text = "Acc Avg: " + accuracyAvg.ToString("0.00000");
            accStdDevText.text = "Acc StdDev: " + accuracyStdDev.ToString("0.00000");
            accMinText.text = "Acc Min: " + accuracyMin.ToString();
            accMaxText.text = "Acc Max: " + accuracyMax.ToString();

            speedAvgText.text = "Spe Avg: " + speedAvg.ToString("0.00000");
            speedStdDevText.text = "Spe StdDev: " + speedStdDev.ToString("0.00000");
            speedMinText.text = "Spe Min: " + speedMin.ToString();
            speedMaxText.text = "Spe Max: " + speedMax.ToString();
        }
    }
}
