using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityTrackerRaw : ActivityTrackerBase
{
    private bool gotLocation = false;
    private GPSLocation lastLoc = new GPSLocation(0,0,0);
    private double distance = 0.0;
    private int count = 0;

    public override void UpdateLocation(GPSLocation location)
    {
        count++;
        countText.text = "C: " + count.ToString();
        if (!gotLocation)
        {
            lastLoc = location;
            gotLocation = true;
        }
        else
        {
            double dist = CalcDistance(lastLoc, location);

            AddDistToStats(dist);
            distance += dist;
            distanceText.text = "D: " + distance.ToString();
            lastLoc = location;
        }
    }
}
