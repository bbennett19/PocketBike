using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndRacePanel : MonoBehaviour
{
    public Timer timer;
    public Text idText;
    public Text timeText;
    public Text bestTimeText;
    public Button playAgain;
    public Button goBack;

    private void Start()
    {
        RaceManager.EndRaceEvent += RaceEnd;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        RaceManager.EndRaceEvent -= RaceEnd;
    }

    public void UpdateTextFields(bool crash, float time)
    {
        idText.text = "";
        timeText.text = "";

        if (crash)
        {
            idText.text = "Crash!";
        }
        float currentBestTime = PlayerPointsAndItems.Instance.playerData.bestTimes[0];

        if (time < currentBestTime)
        {
            idText.text = "New Best Time!";
        }
        else
        {
            idText.text = "Better Luck Next Time";
        }

        if (!crash)
        { 
            timeText.text = "Time: " + time.ToString("0.00");
        }
        bestTimeText.text = "Best Time: " + PlayerPointsAndItems.Instance.playerData.bestTimes[0].ToString("0.00");
    }

    public void UpdateTimeCallback(bool networkError, bool success)
    {
        playAgain.interactable = true;
        goBack.interactable = true;
    }

    public void RaceEnd(bool crash)
    {
        float time = timer.GetTimer();
        UpdateTextFields(crash, time);
        gameObject.SetActive(true);

        if (time < PlayerPointsAndItems.Instance.playerData.bestTimes[0])
        {
            PlayerPointsAndItems.Instance.playerData.bestTimes[0] = time;

            // Upload new best time
            // Disable buttons until network operation is complete
            playAgain.interactable = false;
            goBack.interactable = false;
            StartCoroutine(HTTPRequestHandler.Instance.UpdatePlayerBestTime(SystemInfo.deviceUniqueIdentifier, 0.ToString(), time, UpdateTimeCallback));
        }
    }

    
}
