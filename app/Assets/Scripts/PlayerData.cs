using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public float[] bestTimes;
    public bool[] bestTimeDataToUpload;
    public string Name { get; set; }
    public int Points { get; set; }
    public int GeneratedPoints { get; set; }
    public double DistanceTraveled { get; set; }
    public double GeneratedDistance { get; set; }
    public bool PlayerDataToUpload { get; set; }
    public bool PlayerDataHasBeenCreated { get; set; }

    // First time startup stuff
    public bool DisplayGetName { get; set; }
    public bool DisplayGameplayTutorial { get; set; }

    public delegate void UpdateCompleteDelegate(bool success);

    // Events
    [field: NonSerialized]
    public event Action<string> PlayerNameChange;
    [field: NonSerialized]
    public event Action<int> PlayerPointsChange;

    public PlayerData()
    {
        DisplayGetName = true;
        DisplayGameplayTutorial = true;
        SetupData();
    }

    public void SetPlayerNameChangeWithEvent(string name)
    {
        if (PlayerNameChange != null)
        {
            PlayerNameChange(name);
        }
        Name = name;
    }

    public void SetPlayerPointsWithEvent(int points)
    {
        if (PlayerPointsChange != null)
        {
            PlayerPointsChange(points);
        }
        Points = points;
    }


    // Set all default null values. Mainly used to easily update the save file
    public void SetupData()
    {
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
        if(bestTimeDataToUpload == null)
        {
            bestTimeDataToUpload = new bool[3];
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

    public bool RequiresBestTimeUpdate()
    {
        bool res = false;
        foreach(bool b in bestTimeDataToUpload)
        {
            res = res || b;
        }
        return res;
    }

    public int GetLevelIDToUpdate()
    {
        for(int i = 0; i < bestTimeDataToUpload.Length; i++)
        {
            if (bestTimeDataToUpload[i])
                return i;
        }
        return -1;
    }
}
