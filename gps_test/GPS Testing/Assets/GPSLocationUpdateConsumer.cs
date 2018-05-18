using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public Dropdown dropdown;

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
    private List<GPSLocation> data = new List<GPSLocation>();

    void Start ()
    {
        /*if (!PlaybackManager.Instance.playback)
        {
            UpdateLocation(12.4444, 12.4434, 0f, 1f);
            UpdateLocation(12.4434, 12.4404, 0f, 1f);
            UpdateLocation(12.4424, 12.4414, 0f, 1f);
            UpdateLocation(12.4414, 12.4424, 0f, 1f);
            UpdateLocation(12.4464, 12.4484, 0f, 1f);
            UpdateLocation(12.4474, 12.4474, 0f, 1f);
            UpdateLocation(12.4484, 12.4454, 0f, 1f);
            UpdateLocation(12.4494, 12.4424, 0f, 1f);
            UpdateLocation(12.4444, 12.4484, 0f, 1f);
            UpdateLocation(12.4434, 12.4474, 0f, 1f);
            UpdateLocation(12.4464, 12.4464, 0f, 1f);
            UpdateLocation(12.4424, 12.4454, 0f, 1f);
            UpdateLocation(12.4474, 12.4434, 0f, 1f);
        }*/
        calcButton.onClick.AddListener(CalculateAndDisplay);

        if (PlaybackManager.Instance.playback)
        {
            collect = false;
            LoadData();
            SetupPlayback();
        }
        else if (Input.location.isEnabledByUser)
        {
            calcButton.onClick.AddListener(CalculateAndDisplay);
            callback = new AndroidGPSServiceCallback();
            callback.OnUpdateLocation += UpdateLocation;
            // Get the unity android activity
            AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
            _serviceLauncher = new AndroidJavaClass("com.pocketbike.gpstrackingservice.ServiceLauncher");
            _serviceLauncher.CallStatic("setActivityInstance", activity);
            _gpsServiceActive = _serviceLauncher.CallStatic<bool>("startService", callback);
        }
	}

    private void OnDestroy()
    {
        calcButton.onClick.RemoveListener(CalculateAndDisplay);
    }

    private void SetupPlayback()
    {
        foreach(GPSLocation g in data)
        {
            speedReadings.Add(g.Speed);
            accuracyReadings.Add(g.Accuracy);
            updateCount++;
            _updateQueue.Enqueue(g);
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
                _updateQueue.Enqueue(new GPSLocation(lat, lon, speed, accuracy));
            }
        }
    }

    public void SaveData()
    {
        FileStream fs = File.Open(Application.persistentDataPath + "/" + dropdown.options[dropdown.value].text + ".path", FileMode.OpenOrCreate);
        BinaryFormatter bf = new BinaryFormatter();
        GPSLocation[] d = data.ToArray();
        bf.Serialize(fs, data.ToArray());
        fs.Close();
    }

    public void LoadData()
    {
        dropdown.value = PlaybackManager.Instance.playbackID;
        string fileName = dropdown.options[dropdown.value].text;

        if (File.Exists(Application.persistentDataPath + "/" + fileName + ".path"))
        {
            Debug.Log("load");
            FileStream file = File.Open(Application.persistentDataPath + "/" + fileName + ".path", FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            GPSLocation[] d = bf.Deserialize(file) as GPSLocation[];
            data.AddRange(d);
            file.Close();
        }
    }

    public void PerformPlayback()
    {
        PlaybackManager.Instance.playback = true;
        PlaybackManager.Instance.playbackID = dropdown.value;
        SceneManager.LoadScene(0);
    }

    private void OnApplicationQuit()
    {
        //_serviceLauncher.CallStatic("stopService");
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
                GPSLocation g = _updateQueue.Dequeue();
                data.Add(g);
                OnLocationUpdate(g);
            }
        }
	}

    public void CalculateAndDisplay()
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
