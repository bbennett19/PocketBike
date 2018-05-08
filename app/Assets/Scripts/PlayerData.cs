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

    // First time startup stuff
    public bool DisplayGetName { get; set; }
    public bool DisplayGameplayTutorial { get; set; }

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
