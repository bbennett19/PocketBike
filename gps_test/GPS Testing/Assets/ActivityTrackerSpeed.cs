using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityTrackerSpeed : ActivityTrackerBase
{
    private bool gotLocation = false;
    private GPSLocation lastLoc = new GPSLocation(0, 0, 0);
    private int updateCount = 0;
    private double distance = 0.0;

    public override void UpdateLocation(GPSLocation location)
    {
        updateCount++;
        countText.text = "C:" + updateCount.ToString();
        debug1Text.text = "Spe: " + location.Speed.ToString("0.000");

        if (!gotLocation)
        {
            lastLoc = location;
            gotLocation = true;
        }
        else
        {
            double time = (location.Timestamp - lastLoc.Timestamp).TotalMilliseconds;
            debug2Text.text = "Time: " + time.ToString("0.00");
            double dist = (time / 1000f) * METER_TO_MILE * location.Speed;

            if (dist != double.NaN)
            {
                AddDistToStats(dist);
                if (dist >= minDistInMeters * METER_TO_MILE)
                {
                    debug3Text.text = "Dist: " + dist.ToString("0.000");
                    distance += dist;
                    distanceText.text = "D: " + distance.ToString("0.000");
                    lastLoc = location;
                }
            }
        }
    }
}
