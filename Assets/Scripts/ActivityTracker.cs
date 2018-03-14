using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ActivityTracker : MonoBehaviour {
    public Text distanceText;

    private LocationInfo lastLoc;
    private bool gotLocation = false;
    private double totalDist = 0f;
	// Use this for initialization
	void Start () {
        StartCoroutine(StartGPSService());
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
        if(Input.location.status == LocationServiceStatus.Running && !Input.location.lastData.Equals(lastLoc))
        {
            if(!gotLocation)
            {
                lastLoc = Input.location.lastData;
                gotLocation = true;
            }
            else
            {
                totalDist += CalcDistance(lastLoc, Input.location.lastData);
                distanceText.text = "Distance: " + totalDist.ToString();
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

        double a = Math.Pow(Math.Sin(deltaLat / 2.0), 2.0) + Math.Cos(lat1) + Math.Cos(lat2) + Math.Pow(Math.Sin(deltaLon / 2.0), 2.0);
        double c = 2.0 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        // 6371000 is radius of earth in meters
        return 6371000.0 * c;
    }

    private double DegToRad(double deg)
    {
        return deg * Mathf.Deg2Rad;
    }
}
