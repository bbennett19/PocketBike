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

    public void UpdateTimeCallback(bool success)
    {
        playAgain.interactable = true;
        goBack.interactable = true;
    }

    public void RaceEnd(bool crash)
    {
        // Sometimes collisions are triggered twice forcing this to be called two time
        // Only perform actions if the game object is not active
        if (!gameObject.activeSelf)
        {
            Debug.Log(crash);
            float time = timer.GetTimer();
            bool uploadScore = false;
            playAgain.interactable = false;
            goBack.interactable = false;

            if (time < PlayerPointsAndItems.Instance.playerData.bestTimes[LevelSelectionData.Instance.LevelID] && !crash)
            {
                PlayerPointsAndItems.Instance.playerData.bestTimes[LevelSelectionData.Instance.LevelID] = time;
                PlayerPointsAndItems.Instance.playerData.bestTimeDataToUpload[LevelSelectionData.Instance.LevelID] = true;
                idText.text = "New Best Time!";
                timeText.text = "Time: " + time.ToString("0.00");
                bestTimeText.text = "Best Time: " + PlayerPointsAndItems.Instance.playerData.bestTimes[LevelSelectionData.Instance.LevelID].ToString("0.00");
                uploadScore = true;

            }
            else if (crash)
            {
                idText.text = "Crash!";
                timeText.text = "Time: -:--";
                bestTimeText.text = "Best Time: " + PlayerPointsAndItems.Instance.playerData.bestTimes[LevelSelectionData.Instance.LevelID].ToString("0.00");
            }
            else
            {
                idText.text = "Better Luck Next Time";
                timeText.text = "Time: " + time.ToString("0.00");
                bestTimeText.text = "Best Time: " + PlayerPointsAndItems.Instance.playerData.bestTimes[LevelSelectionData.Instance.LevelID].ToString("0.00");
            }

            gameObject.SetActive(true);

            if (uploadScore)
            {
                // Upload new best time
                NetworkOperations.Instance.UpdatePlayerData(UpdateTimeCallback);
            }
            else
            {
                playAgain.interactable = true;
                goBack.interactable = true;
            }
        }
    }




}
