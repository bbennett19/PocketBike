using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityTrackerAvgMethod : ActivityTrackerBase
{
    
    public int listSize = 5;
    private List<Vector2> updates = new List<Vector2>();
    private bool gotLocation = false;
    private Vector2 lastLoc = new Vector2();
    private double distance = 0.0;
    private int filterCount = 0;
    private int count = 0;

    public override void UpdateLocation(GPSLocation location)
    {
        updates.Add(new Vector2((float)location.Latitude, (float)location.Longitude));

        if (updates.Count == listSize)
        {
            FilterList(updates);
            debug1Text.text = "F: " + filterCount.ToString();
            if(updates.Count == listSize)
            {
                Vector2 point = CalculateNewPoint(updates);
                count++;
                countText.text = "C: " + count.ToString();
                if(!gotLocation)
                {
                    lastLoc = point;
                    gotLocation = true;
                }
                else
                {
                    double dist = CalcDistance(lastLoc, point);
                    if (dist != double.NaN)
                    {
                        AddDistToStats(dist);
                        if (dist >= minDistInMeters * METER_TO_MILE)
                        {
                            debug2Text.text = "Dist: " + dist.ToString("0.0000");
                            distance += dist;
                            distanceText.text = "D: " + distance.ToString("0.0000");
                            lastLoc = point;
                        }
                    }
                }
                updates.Clear();
                updates.Add(point);
            }
        }
    }

    // Filters out any points that are not directly between the first and last point in vectorList
    private void FilterList(List<Vector2> vectorList)
    {
        List<Vector2> itemsToRemove = new List<Vector2>();

        Vector2 firstToLast = vectorList[vectorList.Count - 1] - vectorList[0];

        for(int i = 1; i < vectorList.Count-1; i++)
        {
            Vector2 firstToNext = vectorList[i] - vectorList[0];

            if(Vector2.Dot(firstToNext, firstToLast) <= 0 || firstToNext.magnitude >= firstToLast.magnitude)
            {
                itemsToRemove.Add(vectorList[i]);
            }
        }

        filterCount += itemsToRemove.Count;
        debug1Text.text = "F: " + filterCount.ToString();

        foreach(Vector2 item in itemsToRemove)
        {
            vectorList.Remove(item);
        }
    }

    private Vector2 CalculateNewPoint(List<Vector2> vectorList)
    {
        Vector2 sum = new Vector2();
        Vector2 start = vectorList[0];
        Vector2 end = new Vector2();
        Vector2 vec = new Vector2();

        for (int i = 1; i < vectorList.Count; i++)
        {
            end = vectorList[i];
            vec = (end - start).normalized;
            sum += vec;
        }

        sum = (sum / (float)(vectorList.Count-1)).normalized;
        float angle = Mathf.Acos(Vector2.Dot(vec, sum));
        float dist = (end - start).magnitude * Mathf.Cos(angle);
        return start + (sum * dist);
    }
}
