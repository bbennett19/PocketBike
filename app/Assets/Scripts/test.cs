using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class test : MonoBehaviour {
    public Text countTxt;
    public Text latTxt;
    public Text lonTxt;
    public Text error;
    public Text at;
    public Text bt;
    public Text ct;
    public Text dt;
    AndroidJavaClass serviceLauncher;
    AndroidJavaClass unityClass;
    AndroidJavaClass unityActivity;
    int count = 0;
    private float elapsed = 0f;

    // Use this for initialization
    void Start()
    {
        try
        {
            if (Input.location.isEnabledByUser)
            {
                AndroidGPSServiceCallback callback = new AndroidGPSServiceCallback();
                //callback.SetFields(countTxt, latTxt, lonTxt);
                callback.OnUpdateLocation += GetUpdate;
                // Get the unity android activity
                //unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                //unityActivity = unityClass.GetStatic<AndroidJavaClass>("currentActivity");
                AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
                serviceLauncher = new AndroidJavaClass("com.pocketbike.gpstrackingservice.ServiceLauncher");
                serviceLauncher.CallStatic("setActivityInstance", activity);
                bt.text = serviceLauncher.CallStatic<bool>("startService", callback).ToString();
            }
            else
            {
                error.text = "NOT ENABLED";
            }
        }
        catch (Exception e)
        {
            error.text = e.Message;
        }
    }

    private void OnApplicationQuit()
    {
        serviceLauncher.CallStatic("stopService");
    }

    public void GetUpdate(double lat, double lon)
    {
        countTxt.text = count.ToString();
        count++;
        latTxt.text = lat.ToString();
        lonTxt.text = lon.ToString();
    }

    private void Update()
    {
        elapsed += Time.deltaTime;

        if (elapsed >= 5f)
        {
            ct.text = serviceLauncher.CallStatic<bool>("queryServiceStatus").ToString();
            dt.text = serviceLauncher.CallStatic<int>("queryUpdateCount").ToString();
            error.text = serviceLauncher.CallStatic<string>("queryServiceError");
            //serviceLauncher.CallStatic("test");
            elapsed = 0f;
        }
    }
}
