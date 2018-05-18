using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityTrackerSpeed : ActivityTrackerBase
{
    private bool gotLocation = false;
    private GPSLocation lastLoc = new GPSLocation(0, 0, 0, 0);
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
            double time = (location.Timestamp - lastLoc.Timestamp).TotalMilliseconds;
            double dist = (time / 1000f) * METER_TO_MILE * location.Speed;

            AddDistToStats(dist);
            distance += dist;
            distanceText.text = "D: " + distance.ToString();
            lastLoc = location;
        }
    }
}
