using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceManager : MonoBehaviour {
    public static event Action StartRaceEvent;
    public static event Action PauseRaceEvent;
    public static event Action<bool> EndRaceEvent;

	public static void StartRace()
    {
        if(StartRaceEvent != null)
        {
            StartRaceEvent();
        }
    }

    public static void PauseRace()
    {
        if(PauseRaceEvent != null)
        {
            PauseRaceEvent();
        }
    }

    public static void EndRace(bool crash)
    {
        if(EndRaceEvent != null)
        {
            EndRaceEvent(crash);
        }
    }
}
