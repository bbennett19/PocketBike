using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerText;
    private float _elapsed = 0f;
    private bool _running = false;

    private void Start()
    {
        timerText.text = _elapsed.ToString("0.00");
        RaceManager.StartRaceEvent += StartTimer;
        RaceManager.PauseRaceEvent += Stop;
		RaceManager.ResumeRaceEvent += StartTimer;
        RaceManager.EndRaceEvent += EndRace;
    }

    private void OnDestroy()
    {
        RaceManager.StartRaceEvent -= StartTimer;
        RaceManager.PauseRaceEvent -= Stop;
		RaceManager.ResumeRaceEvent -= StartTimer;
        RaceManager.EndRaceEvent -= EndRace;
    }

    // Update is called once per frame
    void Update ()
    {
        if (_running)
        {
            _elapsed += Time.deltaTime;
            timerText.text = _elapsed.ToString("0.00");
        }

	}

    public void Stop()
    {
        _running = false;
    }

    public void EndRace(bool c)
    {
        Stop();
    }

    public void StartTimer()
    {
        _running = true;
    }

    public float GetTimer()
    {
        return _elapsed;
    }
}
