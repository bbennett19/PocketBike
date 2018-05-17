using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityTrackerAvg : ActivityTrackerBase
{
    public int listCount = 5;
    private bool gotLocation = false;
    private Vector2 lastLoc = new Vector2();
    private int updateCount = 0;
    private double distance = 0.0;
    private List<Vector2> points = new List<Vector2>();

    public override void UpdateLocation(GPSLocation location)
    {
        points.Add(new Vector2((float)location.Latitude, (float)location.Longitude));

        if (points.Count == listCount)
        {
            updateCount++;
            countText.text = "C:" + updateCount.ToString();

            Vector2 avg = new Vector2();

            foreach(Vector2 v in points)
            {
                avg += v;
            }

            avg /= points.Count;
            

            if (!gotLocation)
            {
                lastLoc = avg;
                gotLocation = true;
            }
            else
            {
                double dist = CalcDistance(lastLoc, avg);

                AddDistToStats(dist);
                distance += dist;
                distanceText.text = "D: " + distance.ToString();
                lastLoc = avg;
            }
            points.Clear();
        }
    }
}
