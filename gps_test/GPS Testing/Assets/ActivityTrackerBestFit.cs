using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityTrackerBestFit : ActivityTrackerBase
{
    
    public int listSize = 5;
    private List<Vector2> points = new List<Vector2>();
    private double distance = 0.0;
    private int filterCount = 0;
    private int count = 0;

    public override void UpdateLocation(GPSLocation location)
    {
        points.Add(new Vector2((float)location.Latitude, (float)location.Longitude));

        if(points.Count == listSize)
        {
            Vector2 lineOfBestFit = CalculateBestFit(points);
            Vector2 last = points[0];
            double dist = 0f;
            count++;
            countText.text = "C: " + count.ToString();
                
            for(int i = 1; i < points.Count; i++)
            {
                float projection = Vector2.Dot(lineOfBestFit, points[i] - last);
                dist += CalcDistance(last, last + (lineOfBestFit * projection));
                last = points[i];
            }

            AddDistToStats(dist);

            //debug1Text.text = "Dist: " + dist.ToString();
            distance += dist;
            distanceText.text = "D: " + distance.ToString();
            points.Clear();
            points.Add(last);
        }
    }

    // Filters out any points that are not directly between the first and last point in vectorList
    private void FilterList(List<Vector2> vectorList)
    {
        List<Vector2> itemsToRemove = new List<Vector2>();

        Vector2 firstToLast = vectorList[vectorList.Count - 1] - vectorList[0];
        Vector2 firstToPrev = new Vector2();

        for(int i = 1; i < vectorList.Count-1; i++)
        {
            Vector2 firstToNext = vectorList[i] - vectorList[0];

            if(Vector2.Dot(firstToNext, firstToLast) <= 0f || firstToNext.sqrMagnitude >= firstToLast.sqrMagnitude || firstToPrev.sqrMagnitude >= firstToNext.sqrMagnitude)
            {
                itemsToRemove.Add(vectorList[i]);
            }

            firstToPrev = firstToNext;
        }

        filterCount += itemsToRemove.Count;

        foreach(Vector2 item in itemsToRemove)
        {
            vectorList.Remove(item);
        }
    }

    private Vector2 CalculateBestFit(List<Vector2> vectorList)
    {
        Vector2 avg = new Vector2();

        foreach(Vector2 v in vectorList)
        {
            avg += v;
        }

        avg /= vectorList.Count;

        double rise = 0f;
        double run = 0f;

        foreach(Vector2 v in vectorList)
        {
            rise += (v.x - avg.x) * (v.y - avg.y);
            run += Math.Pow((v.x - avg.x), 2);
        }

        double slope = rise / run;
        return new Vector2(1f, (float)slope).normalized;
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
        float angle = Mathf.Acos(Mathf.Clamp(Vector2.Dot(vec, sum), -1f, 1f));
        float dist = (end - start).magnitude * Mathf.Cos(angle);
        return start + (sum * dist);
    }
}
