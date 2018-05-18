using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ActivityTracker : MonoBehaviour
{
    public Text distanceText;
    public Text distanceSpeedText;
	public Text latText;
	public Text lonText;
    public Text speedText;
    public Text updateText;
    public Text pointsText;
    public Text errorText;
    public Button collectButton;
    public GPSLocationUpdateConsumer gpsLocationConsumer;
    public Transform modalParent;
    public GameObject noInternetModalPanel;
    public AudioClip collectSound;

    private GPSLocation lastLoc = new GPSLocation(0,0,0);
    private bool gotLocation = false;
    private int updateCount = 0;
    private float elapsed = 0f;
    private double distSpeed = 0f;

#if !UNITY_EDITOR
    private AndroidJavaClass _serviceLauncher;
#endif

    private bool _gpsServiceActive = false;

    // Use this for initialization
    void Start ()
    {
        errorText.text = "";
        collectButton.interactable = PlayerPointsAndItems.Instance.playerData.GeneratedPoints != 0;
        SetTextFields();
        updateText.text = "Update Count: " + updateCount.ToString();
        LaunchGPSService();

	}

    private void LaunchGPSService()
    {
#if !UNITY_EDITOR 

        if (Input.location.isEnabledByUser)
        {
            gpsLocationConsumer.OnLocationUpdate += UpdateLocation;
            //_androidGPSCallback.tracker = this;
            // Get the unity android activity
            AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
            _serviceLauncher = new AndroidJavaClass("com.pocketbike.gpstrackingservice.ServiceLauncher");
            _serviceLauncher.CallStatic("setActivityInstance", activity);
            _gpsServiceActive = _serviceLauncher.CallStatic<bool>("startService", gpsLocationConsumer.callback);

            if (!_gpsServiceActive)
                errorText.text = "Error on Launch";
            else
                errorText.text = "No error on launch";
        }
#endif
    }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnApplicationQuit()
    {
#if !UNITY_EDITOR
        gpsLocationConsumer.OnLocationUpdate -= UpdateLocation;
        _serviceLauncher.CallStatic("stopService");
#endif
    }

    public void UpdateLocation(GPSLocation location)
    {

        
        if (!gotLocation || (location.Latitude != lastLoc.Latitude || location.Longitude != lastLoc.Longitude))
        {
            updateCount++;
            updateText.text = "Update: " + updateCount.ToString();
            latText.text = "Lat: " + location.Latitude.ToString();
            lonText.text = "Lon: " + location.Longitude.ToString();
            speedText.text = "Speed: " + location.Speed.ToString();

            if (!gotLocation)
            {
                lastLoc = location;
                gotLocation = true;
            }
            else
            {
                distSpeed += CalcDistanceSpeed(lastLoc, location);
                distanceSpeedText.text = "DistanceS: " + distSpeed.ToString("0.00");
                PlayerPointsAndItems.Instance.playerData.GeneratedDistance += CalcDistance(lastLoc, location);
                lastLoc = location;
                PlayerPointsAndItems.Instance.playerData.GeneratedPoints = (int)(PlayerPointsAndItems.Instance.playerData.GeneratedDistance * 100);
                SetTextFields();

                if (PlayerPointsAndItems.Instance.playerData.GeneratedPoints > 0)
                {
                    collectButton.interactable = true;
                }
            }
        }
    }

    private void Update()
    {

    }

    private double CalcDistance(GPSLocation loc1, GPSLocation loc2)
    {
        // Haversine formula for calculating distance between two lat/lon values
        double lat1 = DegToRad(loc1.Latitude);
        double lat2 = DegToRad(loc2.Latitude);
        double deltaLat = DegToRad(loc2.Latitude - loc1.Latitude);
        double deltaLon = DegToRad(loc2.Longitude - loc1.Longitude);

        double a = Math.Pow(Math.Sin(deltaLat / 2.0), 2.0) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Pow(Math.Sin(deltaLon / 2.0), 2.0);
        double c = 2.0 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        // 3959 is radius of earth in miles
        return 3959.0 * c;
    }

    private double CalcDistanceSpeed(GPSLocation loc1, GPSLocation loc2)
    {
        TimeSpan elapsed = loc2.Timestamp.Subtract(loc1.Timestamp);
        return (elapsed.Milliseconds / 1000.0) * loc2.Speed * 0.00062137;

    }

    private double DegToRad(double deg)
    {
        return deg * Mathf.Deg2Rad;
    }

    public void CollectPoints()
    {
        Debug.Log("Collect");
        NetworkOperations.Instance.UpdatePlayerData(AddPointsStatus, true);
        collectButton.interactable = false;
    }

    public void AddPointsStatus(bool success)
    {
        if(success)
        {
            AudioPlayer.Instance.audioSource.PlayOneShot(collectSound);
            PlayerPointsAndItems.Instance.playerData.DistanceTraveled = PlayerPointsAndItems.Instance.playerData.GetTotalDistance();
            PlayerPointsAndItems.Instance.playerData.GeneratedDistance = 0f;
            PlayerPointsAndItems.Instance.playerData.SetPlayerPointsWithEvent(PlayerPointsAndItems.Instance.playerData.GetTotalPoints());
            PlayerPointsAndItems.Instance.playerData.GeneratedPoints = 0;
            SetTextFields();
        }
        else
        {
            Debug.Log("Collect Fail");
            // Display network error
            GameObject g = Instantiate(noInternetModalPanel, modalParent);
            g.GetComponent<BasicModalPanel>().SetTextToDisplay("Unable to connect to the server.");
        }
        collectButton.interactable = true;
    }

    public void ResetData()
    {
        PlayerPointsAndItems.Instance.playerData.Points = 0;
        PlayerPointsAndItems.Instance.playerData.DistanceTraveled = 0f;
    }

    private void SetTextFields()
    {
        pointsText.text = PlayerPointsAndItems.Instance.playerData.GeneratedPoints.ToString();
        distanceText.text = PlayerPointsAndItems.Instance.playerData.GeneratedDistance.ToString("0.00") + " miles";
    }

    public void GeneratePoints()
    {
        collectButton.interactable = true;
        PlayerPointsAndItems.Instance.playerData.GeneratedPoints += 10;
        PlayerPointsAndItems.Instance.playerData.GeneratedDistance += .10;
        SetTextFields();
    }
}
