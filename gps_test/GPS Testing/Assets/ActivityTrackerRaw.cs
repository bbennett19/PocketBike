using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityTrackerRaw : ActivityTrackerBase
{
    private bool gotLocation = false;
    private GPSLocation lastLoc = new GPSLocation(0,0,0);
    private int updateCount = 0;
    private double distance = 0.0;

    public override void UpdateLocation(GPSLocation location)
    {
        updateCount++;
        countText.text = "C:" + updateCount.ToString();
        debug1Text.text = "Lat: " + location.Latitude.ToString("0.0000");
        debug2Text.text = "Lon: " + location.Longitude.ToString("0.0000");

        if (!gotLocation)
        {
            lastLoc = location;
            gotLocation = true;
        }
        else
        {
            double dist = CalcDistance(lastLoc, location);

            AddDistToStats(dist);
            if (dist >= minDistInMeters * METER_TO_MILE)
            {
                debug3Text.text = "Dist: " + dist.ToString("0.0000");
                distance += dist;
                distanceText.text = "D: " + distance.ToString("0.0000");
                lastLoc = location;
            }
        }
    }
}
