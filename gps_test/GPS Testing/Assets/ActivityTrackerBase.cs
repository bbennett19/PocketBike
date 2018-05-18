using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ActivityTrackerBase : MonoBehaviour
{
    public Text distanceText;
    public Text countText;
    public Text debug1Text;
	public Text debug2Text;
    public Text avgText;
    public Text stdDevText;
    public Button calcButton;
    public const double METER_TO_MILE = 0.000621371;
    private List<double> distanceVals = new List<double>();
    
    // Use this for initialization
    public virtual void Start ()
    {
        calcButton.onClick.AddListener(CalculateAvgAndStdDev);
		GPSLocationUpdateConsumer.OnLocationUpdate += UpdateLocation;
	}

    private void OnDestroy()
    {
        calcButton.onClick.RemoveListener(CalculateAvgAndStdDev);
        GPSLocationUpdateConsumer.OnLocationUpdate -= UpdateLocation;
    }

    public abstract void UpdateLocation(GPSLocation location);

    protected double CalcDistance(GPSLocation loc1, GPSLocation loc2)
    {
        // Haversine formula for calculating distance between two lat/lon values
        double lat1 = DegToRad(loc1.Latitude);
        double lat2 = DegToRad(loc2.Latitude);
        double deltaLat = DegToRad(loc2.Latitude - loc1.Latitude);
        double deltaLon = DegToRad(loc2.Longitude - loc1.Longitude);
        double haversine = Math.Pow(Math.Sin(deltaLat / 2.0), 2f) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Pow(Math.Sin(deltaLon / 2.0), 2f);
        double a = Math.Asin(Math.Min(Math.Sqrt(haversine), 1.0));
        return a * 2.0 * 3958.756;
    }

    protected double CalcDistance(Vector2 loc1, Vector2 loc2)
    {
        // Haversine formula for calculating distance between two lat/lon values
        double lat1 = DegToRad(loc1.x);
        double lat2 = DegToRad(loc2.x);
        double deltaLat = DegToRad(loc2.x - loc1.x);
        double deltaLon = DegToRad(loc2.y - loc1.y);
        double haversine = Math.Pow(Math.Sin(deltaLat / 2.0), 2f) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Pow(Math.Sin(deltaLon / 2.0), 2f);
        double a = Math.Asin(Math.Min(Math.Sqrt(haversine), 1.0));
        return a * 2.0 * 3958.756;
    }

    protected double DegToRad(double deg)
    {
        return deg * Mathf.Deg2Rad;
    }

    protected void AddDistToStats(double dist)
    {
        distanceVals.Add(dist/METER_TO_MILE);
    }

    protected void CalculateAvgAndStdDev()
    {
        if (distanceVals.Count > 0)
        {
            double sum = 0.0;

            foreach (double d in distanceVals)
            {
                sum += d;
            }

            double avg = sum / distanceVals.Count;

            double stdDev = 0.0;

            foreach (double d in distanceVals)
            {
                stdDev += Math.Pow((d - avg), 2);
            }

            stdDev = Math.Sqrt(stdDev / distanceVals.Count);

            avgText.text = "Avg: " + avg.ToString();
            stdDevText.text = "StdDev: "+ stdDev.ToString();
        }

    }

}
