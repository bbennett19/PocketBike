using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public bool[] itemsPurchased;
    public float[] bestTimes;
    public string Name { get; set; }
    public int Points { get; set; }
    public int GeneratedPoints { get; set; }
    public double DistanceTraveled { get; set; }
    public double GeneratedDistance { get; set; }
    public bool PlayerDataToUpload { get; set; }
    public bool BestTimeDataToUpload { get; set; }
    public bool PlayerDataHasBeenCreated { get; set; }

    public PlayerData()
    {
        SetupData();
    }

    public void SetupData()
    {
        if (itemsPurchased == null)
        {
            itemsPurchased = new bool[24];
        }
        if (bestTimes == null)
        {
            bestTimes = new float[3];

            for (int i = 0; i < 3; i++)
            {
                bestTimes[i] = 99.9999f;
            }
        }
        if(Name == null)
        {
            Name = "undefined";
        }
    }

    public double GetTotalDistance()
    {
        return GeneratedDistance + DistanceTraveled;
    }

    public int GetTotalPoints()
    {
        return Points + GeneratedPoints;
    }
}
