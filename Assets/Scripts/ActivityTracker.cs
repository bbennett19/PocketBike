using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ActivityTracker : MonoBehaviour
{
    public Text distanceText;
	public Text latText;
	public Text lonText;
    public Text updateText;
    public Text pointsText;

    private LocationInfo lastLoc;
    private bool gotLocation = false;
    private double totalDist = 0f;
    private int updateCount = 0;
    private int points = 0;
    private int init_points = 0;
    private double init_dist = 0f;

    // Use this for initialization
    void Start ()
    {
        pointsText.text = "Generated: " + points.ToString() + " points!";
        distanceText.text = "Distance: " + totalDist.ToString();
        updateText.text = "Update Count: " + updateCount.ToString();
        init_points = PlayerPointsAndItems.Instance.data.Points;
        init_dist = PlayerPointsAndItems.Instance.data.DistanceTraveled;
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
                updateText.text = "Update Count: " + updateCount.ToString();
                totalDist += CalcDistance(lastLoc, Input.location.lastData);
                distanceText.text = "Distance: " + totalDist.ToString();
				lastLoc = Input.location.lastData;
                points = (int)(totalDist * 100);
                PlayerPointsAndItems.Instance.data.Points = init_points + points;
                PlayerPointsAndItems.Instance.data.DistanceTraveled = init_dist + totalDist;
                pointsText.text = "Generated: " + points.ToString() + " points!";
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

    public void ResetData()
    {
        PlayerPointsAndItems.Instance.data.Points = 0;
        PlayerPointsAndItems.Instance.data.DistanceTraveled = 0f;
    }

    public void GenPoints()
    {
        points += 10;
        PlayerPointsAndItems.Instance.data.Points = init_points + points;
        pointsText.text = "Generated: " + points.ToString() + " points!";
    }
}
