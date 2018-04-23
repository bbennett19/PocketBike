using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ActivityTracker : MonoBehaviour
{
    public Text distanceText;
	public Text latText;
	public Text lonText;
    public Text updateText;
    public Text pointsText;
    public Text errorText;
    public Button collectButton;
    public UIUpdater uiUpdater;

    private LocationInfo lastLoc;
    private bool gotLocation = false;
    private int updateCount = 0;

    // Use this for initialization
    void Start ()
    {
        errorText.text = "";
        collectButton.interactable = PlayerPointsAndItems.Instance.playerData.GeneratedPoints != 0;
        SetTextFields();
        updateText.text = "Update Count: " + updateCount.ToString();
        StartCoroutine(StartGPSService());
	}

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private IEnumerator StartGPSService()
    {
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("NO GPS");
            yield break;
        }

        Input.location.Start();
        int maxWait = 20;

        while(Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if(maxWait == 0)
        {
            Debug.Log("TIME OUT");
            yield break;
        }

        if(Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("BROKEN");
            yield break;
        }
    }

    private void Update()
    {
		if(Input.location.status == LocationServiceStatus.Running && 
			(Input.location.lastData.latitude != lastLoc.latitude || Input.location.lastData.longitude != lastLoc.longitude))
        {
			latText.text = "Lat: " + Input.location.lastData.latitude.ToString ();
			lonText.text = "Lon: " + Input.location.lastData.longitude.ToString ();
            if(!gotLocation)
            {
                lastLoc = Input.location.lastData;
                gotLocation = true;
            }
            else
            {
                updateCount++;
                updateText.text = "Update: " + updateCount.ToString();
                PlayerPointsAndItems.Instance.playerData.GeneratedDistance += CalcDistance(lastLoc, Input.location.lastData);
				lastLoc = Input.location.lastData;
                PlayerPointsAndItems.Instance.playerData.GeneratedPoints = (int)(PlayerPointsAndItems.Instance.playerData.GeneratedDistance * 100);
                SetTextFields();

                if(PlayerPointsAndItems.Instance.playerData.GeneratedPoints > 0)
                {
                    collectButton.interactable = true;
                }
            }
        }
    }

    private double CalcDistance(LocationInfo loc1, LocationInfo loc2)
    {
        // Haversine formula for calculating distance between two lat/lon values
        double lat1 = DegToRad(loc1.latitude);
        double lat2 = DegToRad(loc2.latitude);
        double deltaLat = DegToRad(loc2.latitude - loc1.latitude);
        double deltaLon = DegToRad(loc2.longitude - loc1.longitude);

        double a = Math.Pow(Math.Sin(deltaLat / 2.0), 2.0) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Pow(Math.Sin(deltaLon / 2.0), 2.0);
        double c = 2.0 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        // 3959 is radius of earth in miles
        return 3959.0 * c;
    }

    private double DegToRad(double deg)
    {
        return deg * Mathf.Deg2Rad;
    }

    public void CollectPoints()
    {
        string name = PlayerPointsAndItems.Instance.playerData.Name;
        int newPoints = PlayerPointsAndItems.Instance.playerData.GetTotalPoints();
        double newDistance = PlayerPointsAndItems.Instance.playerData.GetTotalDistance();
        StartCoroutine(HTTPRequestHandler.Instance.UpdatePlayerData(SystemInfo.deviceUniqueIdentifier, name, newPoints, newDistance, AddPointsCallback));
    }

    public void AddPointsCallback(bool networkError, bool success)
    {
        if(success)
        {
            // Add points and total distance
            PlayerPointsAndItems.Instance.playerData.Points += PlayerPointsAndItems.Instance.playerData.GeneratedPoints;
            PlayerPointsAndItems.Instance.playerData.DistanceTraveled += PlayerPointsAndItems.Instance.playerData.GeneratedDistance;
            PlayerPointsAndItems.Instance.playerData.GeneratedPoints = 0;
            PlayerPointsAndItems.Instance.playerData.GeneratedDistance = 0.0;
            collectButton.interactable = false;
            SetTextFields();
            uiUpdater.UpdateText();
        }
        if(networkError || !success)
        {
            Debug.Log("Add points error");
        }
    }

    public void ResetData()
    {
        PlayerPointsAndItems.Instance.playerData.Points = 0;
        PlayerPointsAndItems.Instance.playerData.DistanceTraveled = 0f;
    }

    private void SetTextFields()
    {
        pointsText.text = "Points Generated: " + PlayerPointsAndItems.Instance.playerData.GeneratedPoints.ToString();
        distanceText.text = "Distance: " + PlayerPointsAndItems.Instance.playerData.GeneratedDistance.ToString("0.00");
    }

    public void GeneratePoints()
    {
        collectButton.interactable = true;
        PlayerPointsAndItems.Instance.playerData.GeneratedPoints += 10;
        PlayerPointsAndItems.Instance.playerData.GeneratedDistance += .10;
        SetTextFields();
    }
}
